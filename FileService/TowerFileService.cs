using DataLibrary.Interfaces;
using FileService.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TowerFrequencyService;
using TowerNeighborService;
using TowerTableService;
using static FileService.Factory;

namespace FileService
{
    public sealed class TowerFileService : ServiceBase, ITowerFileService
    {
        private const string USAGE_CONTROL = "c";

        private readonly ITowerFrequencyService _towerFrequencyService;
        private readonly ITowerTableService _towerTableService;
        private readonly ITowerNeighborService _towerNeighborService;

        private readonly ITowerRepository _towerRepo;

        public TowerFileService(ITowerRepository towerRepository, ITowerFrequencyService towerFrequencyService, ITowerNeighborService towerNeighborService,
            ITowerTableService towerTableService)
        {
            _towerRepo = towerRepository;
            _towerFrequencyService = towerFrequencyService;
            _towerNeighborService = towerNeighborService;
            _towerTableService = towerTableService;
        }

        public async Task<Tower> GetAsync(int id) => await _towerRepo.GetAsync(id);

        public async Task<int> ProcessRecordsAsync(IEnumerable<IRecord> records, Action<string, string, int, int> updateProgress, Action<bool> completedTasks,
            bool showCompleted)
        {
            await Task.Run(async () =>
            {
                foreach (var tower in (IEnumerable<Tower>)records)
                {
                    await WriteTowerAsync(tower, updateProgress);
                    await WriteFrequenciesAsync(tower, updateProgress);
                    await WriteTablesAsync(tower, updateProgress);
                    await WriteTowerNeighborsAsync(tower, updateProgress);
                }
            });

            completedTasks(showCompleted);
            return 1;
        }

        private async Task WriteTowerAsync(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var currentTower = await _towerRepo.GetAsync(tower.SystemID, tower.TowerNumber);
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
            await _towerRepo.WriteAsync(currentTower);
            tower.ID = currentTower.ID;
        }

        private async Task WriteFrequenciesAsync(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            await CheckMissingFrequenciesAsync(tower);

            foreach (var frequency in tower.Frequencies)
            {
                var currentFrequency = await _towerFrequencyService.GetForFrequencyAsync(tower.SystemID, tower.TowerNumber, frequency.Frequency);

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
                await _towerFrequencyService.WriteAsync(currentFrequency);
            }
        }

        private async Task CheckMissingFrequenciesAsync(Tower tower)
        {
            var currentFrequencies = await _towerFrequencyService.GetForTowerAsync(tower.SystemID, tower.TowerNumber);
            var removeFrequencies = currentFrequencies.Where(frq => tower.Frequencies.SingleOrDefault(tf => 
                tf.Frequency.Equals(frq.Frequency, StringComparison.OrdinalIgnoreCase)) == null).ToList();

            foreach (var frequency in removeFrequencies)
            {
                await _towerFrequencyService.DeleteAsync(frequency.ID);
            }
        }

        private async Task WriteTablesAsync(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            await _towerTableService.DeleteForTowerAsync(tower.SystemID, tower.TowerNumber);

            foreach (var table in tower.Tables)
            {
                var currentTable = await _towerTableService.GetAsync(tower.SystemID, tower.TowerNumber, table.TableID) ??
                                   _towerTableService.Create(tower.SystemID, tower.TowerNumber, table.TableID);

                _towerTableService.Fill(table, currentTable);
                writtenCount++;
                updateProgress($"Processing tower table ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
                await _towerTableService.WriteAsync(currentTable);
            }
        }

        private async Task WriteTowerNeighborsAsync(Tower tower, Action<string, string, int, int> updateProgress)
        {
            var writtenCount = 0;

            await _towerNeighborService.DeleteForTowerAsync(tower.SystemID, tower.TowerNumber);

            foreach (var neighbor in tower.Neighbors)
            {
                var currentNeighbor = await _towerNeighborService.GetForSystemTowerNumberAsync(tower.SystemID, neighbor.TowerNumber,
                    neighbor.NeighborSystemID, neighbor.NeighborTowerID);

                if (currentNeighbor == null)
                {
                    currentNeighbor = await CheckTower(tower, neighbor);
                }

                currentNeighbor.FirstSeen = tower.TimeStamp ?? DateTime.Now;
                currentNeighbor.LastSeen = tower.TimeStamp ?? DateTime.Now;

                writtenCount++;
                updateProgress($"Processing tower neighbor ({writtenCount:#,##0})", $"{writtenCount:#,##0} records written", writtenCount, writtenCount);
                await _towerNeighborService.WriteAsync(currentNeighbor);
            }
        }

        private async Task<TowerNeighbor> CheckTower(Tower tower, TowerNeighbor neighbor)
        {
            var existingTower = await _towerRepo.GetAsync(neighbor.NeighborSystemID, neighbor.NeighborTowerID);

            if (existingTower != null)
            {
                return _towerNeighborService.Create(tower.SystemID, tower.TowerNumber, neighbor.NeighborSystemID, neighbor.NeighborTowerID,
                    neighbor.NeighborTowerNumberHex, neighbor.NeighborTowerName, neighbor.NeighborFrequency, neighbor.NeighborChannel);
            }

            var newTower = CreateTower(neighbor.SystemID, neighbor.NeighborTowerID, neighbor.NeighborTowerName.IsNullOrWhiteSpace() ?
                $"<Unknown> (T{neighbor.NeighborTowerID})" : neighbor.NeighborTowerName);

            await _towerRepo.WriteAsync(newTower);
            await AddNewTowerFrequencyAsync(tower, newTower, neighbor);

            return _towerNeighborService.Create(tower.SystemID, tower.TowerNumber, neighbor.NeighborSystemID, neighbor.NeighborTowerID,
                neighbor.NeighborTowerNumberHex, neighbor.NeighborTowerName, neighbor.NeighborFrequency, neighbor.NeighborChannel);
        }

        private async Task AddNewTowerFrequencyAsync(Tower tower, Tower newTower, TowerNeighbor neighbor)
        {
            var newFrequency = _towerFrequencyService.CreateTowerFrequency(newTower.SystemID, newTower.TowerNumber, USAGE_CONTROL, neighbor.NeighborFrequency,
                tower.TimeStamp ?? DateTime.Now);

            await _towerFrequencyService.WriteAsync(newFrequency);
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
