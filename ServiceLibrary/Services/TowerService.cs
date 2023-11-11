using DataLibrary.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceLibrary.Abstracts;
using ServiceLibrary.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerFrequencyService;
using TowerNeighborService;
using TowerTableService;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Services
{
    public class TowerService : ServiceBase, ITowerService
    {
        private const string USAGE_CONTROL = "c";

        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerTableService _towerTableService;
        private readonly ITowerNeighborService _towerNeighborService;

        private readonly ITowerRepository _towerRepo;

        public TowerService(ITowerRepository towerRepository, ITowerFrequencyService towerFrequencyService, ITowerNeighborService towerNeighborService, ITowerTableService towerTableService)
        {
            _towerRepo = towerRepository;
            _towerFrequencyService = towerFrequencyService;
            _towerNeighborService = towerNeighborService;
            _towerTableService = towerTableService;
        }

        public Tower Get(int id)
        {
            return _towerRepo.Get(id);
        }

        public Tower GetForTower(int systemID, int towerNumber)
        {
            return _towerRepo.Get(systemID, towerNumber);
        }

        public IEnumerable<TowerFrequency> GetFrequenciesForTower(int systemID, int towerNumber)
        {
            return _towerFrequencyService.GetFrequenciesForTower(systemID, towerNumber);
        }

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks, bool showCompleted)
        {
            await Task.Run(() =>
            {
                foreach (var tower in (IEnumerable<Tower>)records)
                {
                    WriteTower(tower, updateProgress);
                    WriteFrequencies(tower, updateProgress);
                    WriteTables(tower, updateProgress);
                    WriteTowerNeighbors(tower, updateProgress);
                }
            });

            completedTasks(showCompleted);
            return 1;
        }

        private void WriteTower(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var currentTower = _towerRepo.Get(tower.SystemID, tower.TowerNumber);
            var writtenCount = 0;

            writtenCount++;

            if (currentTower == null)
            {
                currentTower = Create<Tower>();

                currentTower.SystemID = tower.SystemID;
                currentTower.TowerNumber = tower.TowerNumber;
                currentTower.FirstSeen = tower.FirstSeen;
                currentTower.LastSeen = tower.LastSeen;
            }
            else
            {
                if (tower.FirstSeen < currentTower.FirstSeen)
                {
                    currentTower.FirstSeen = tower.FirstSeen;
                }

                if (tower.LastSeen > currentTower.LastSeen)
                {
                    currentTower.LastSeen = tower.LastSeen;
                }
            }

            currentTower.TowerNumberHex = tower.TowerNumberHex;
            currentTower.Description = tower.Description;
            currentTower.HitCount = tower.HitCount;
            currentTower.WACN = tower.WACN;
            currentTower.ControlCapabilities = tower.ControlCapabilities;
            currentTower.Flavor = tower.Flavor;
            currentTower.CallSigns = tower.CallSigns;

            writtenCount++;
            updateProgress($"Processing tower ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
            _towerRepo.Write(currentTower);
            tower.ID = currentTower.ID;
        }

        private void WriteFrequencies(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            CheckMissingFrequencies(tower);

            foreach (var frequency in tower.Frequencies)
            {
                var currentFrequency = _towerFrequencyService.GetForFrequency(tower.SystemID, tower.TowerNumber, frequency.Frequency);

                if (currentFrequency == null)
                {
                    currentFrequency = _towerFrequencyService.CreateTowerFrequency(tower.SystemID, tower.TowerNumber, frequency.Usage, frequency.Frequency,
                        (tower.TimeStamp ?? DateTime.MinValue));

                    if (currentFrequency == null)
                    {
                        return;
                    }

                    currentFrequency.FirstSeen = tower.TimeStamp ?? DateTime.MinValue;
                    currentFrequency.LastSeen = tower.TimeStamp ?? DateTime.MinValue;
                }

                currentFrequency.Usage = frequency.Usage;
                currentFrequency.InputChannel = frequency.InputChannel;
                currentFrequency.InputFrequency = frequency.InputFrequency;
                currentFrequency.InputExplicit = frequency.InputExplicit;
                currentFrequency.HitCount = frequency.HitCount;

                writtenCount++;
                updateProgress($"Processing tower frequency ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
                _towerFrequencyService.Write(currentFrequency);
            }
        }

        private void CheckMissingFrequencies(Tower tower)
        {
            var currentFrequencies = _towerFrequencyService.GetForTower(tower.SystemID, tower.TowerNumber);
            var removeFrequencies = currentFrequencies.Where(frq => tower.Frequencies.SingleOrDefault(tf => 
                tf.Frequency.Equals(frq.Frequency, StringComparison.OrdinalIgnoreCase)) == null).ToList();

            foreach (var frequency in removeFrequencies)
            {
                _towerFrequencyService.Delete(frequency.ID);
            }
        }

        private void WriteTables(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            _towerTableService.DeleteForTower(tower.SystemID, tower.TowerNumber);

            foreach (var table in tower.Tables)
            {
                var currentTable = _towerTableService.Get(tower.SystemID, tower.TowerNumber, table.TableID) ??
                                   _towerTableService.Create(tower.SystemID, tower.TowerNumber, table.TableID);

                _towerTableService.Fill(table, currentTable);
                writtenCount++;
                updateProgress($"Processing tower table ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
                _towerTableService.Write(currentTable);
            }
        }

        private void WriteTowerNeighbors(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            _towerNeighborService.DeleteForTower(tower.SystemID, tower.TowerNumber);

            foreach (var neighbor in tower.Neighbors)
            {
                var currentNeighbor = _towerNeighborService.GetForSystemTowerNumber(tower.SystemID, neighbor.TowerNumber, neighbor.NeighborSystemID, neighbor.NeighborTowerID);

                if (currentNeighbor == null)
                {
                    currentNeighbor = CheckTower(tower, neighbor);
                }

                currentNeighbor.FirstSeen = tower.TimeStamp ?? DateTime.Now;
                currentNeighbor.LastSeen = tower.TimeStamp ?? DateTime.Now;

                writtenCount++;
                updateProgress($"Processing tower neighbor ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
                _towerNeighborService.Write(currentNeighbor);
            }
        }

        private TowerNeighbor CheckTower(Tower tower, TowerNeighbor neighbor)
        {
            var existingTower = _towerRepo.Get(neighbor.NeighborSystemID, neighbor.NeighborTowerID);

            if (existingTower != null)
            {
                return _towerNeighborService.Create(tower.SystemID, tower.TowerNumber, neighbor.NeighborSystemID, neighbor.NeighborTowerID,
                    neighbor.NeighborTowerNumberHex, neighbor.NeighborTowerName, neighbor.NeighborFrequency, neighbor.NeighborChannel);
            }

            var newTower = CreateTower(neighbor.SystemID, neighbor.NeighborTowerID, neighbor.NeighborTowerName.IsNullOrWhiteSpace() ?
                $"<Unknown> (T{neighbor.NeighborTowerID})" : neighbor.NeighborTowerName);

            _towerRepo.Write(newTower);
            AddNewTowerFrequency(tower, newTower, neighbor);

            return _towerNeighborService.Create(tower.SystemID, tower.TowerNumber, neighbor.NeighborSystemID, neighbor.NeighborTowerID,
                neighbor.NeighborTowerNumberHex, neighbor.NeighborTowerName, neighbor.NeighborFrequency, neighbor.NeighborChannel);
        }

        private void AddNewTowerFrequency(Tower tower, Tower newTower, TowerNeighbor neighbor)
        {
            var newFrequency = _towerFrequencyService.CreateTowerFrequency(newTower.SystemID, newTower.TowerNumber, USAGE_CONTROL, neighbor.NeighborFrequency,
                tower.TimeStamp ?? DateTime.Now);

            _towerFrequencyService.Write(newFrequency);
        }

        public Tower CreateTower(int systemID, int towerID, string description)
        {
            var tower = Create<Tower>();

            tower.SystemID = systemID;
            tower.TowerNumber = towerID;
            tower.Description = description;

            return tower;
        }
    }
}
