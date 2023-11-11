using DataLibrary.Interfaces;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ServiceCommon.Common;
using static ServiceCommon.Factory;

namespace TowerFrequencyUsageService
{
    public sealed class TowerFrequencyUsageService : ServiceBase, ITowerFrequencyUsageService
    {
        private const string EMPTY_FREQUENCY = "0.00000";

        private readonly ITowerFrequencyUsageRepository _towerFrequencyUsageRepo;

        public TowerFrequencyUsageService(ITowerFrequencyUsageRepository towerFrequencyUsageRepository) : base(null) =>
            _towerFrequencyUsageRepo = towerFrequencyUsageRepository;

        public async Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber) =>
            await _towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(systemID, towerNumber);

        public async Task<IEnumerable<TowerFrequencyUsage>> GetFrequenciesForTowerAsync(int systemID, int towerNumber, DateTime date) =>
            await _towerFrequencyUsageRepo.GetFrequenciesForTowerAsync(systemID, towerNumber, date);

        public void ProcessRecord(int systemID, int towerNumber, string frequency, string channel, int talkgroupID, int radioID,
            ICollection<TowerFrequencyUsage> towerFrequencyUsages, DateTime timeStamp, string action, Action<ICounterRecord, ActionTypes> updateCounters)
        {
            if (((talkgroupID == 0) || (talkgroupID == -1) || (radioID == 0)) && CanBypassTowerFrequencyAction(action))
            {
                return;
            }

            frequency = FixFrequency(frequency);

            var towerFrequencyUsage = towerFrequencyUsages.SingleOrDefault(tfu => tfu.Frequency.Equals(frequency, StringComparison.OrdinalIgnoreCase) &&
                tfu.Date == timeStamp.Date);

            if (towerFrequencyUsage == null)
            {
                towerFrequencyUsage = CreateTowerFrequencyUsage(systemID, towerNumber, frequency, channel, timeStamp);

                if (towerFrequencyUsage != null)
                {
                    towerFrequencyUsages.Add(towerFrequencyUsage);
                }
            }

            if (towerFrequencyUsage != null)
            {
                updateCounters(towerFrequencyUsage, action.GetEnumFromDescription<ActionTypes>());
                CheckDates(towerFrequencyUsage, timeStamp);
            }
        }

        private static bool CanBypassTowerFrequencyAction(string action)
        {
            switch (action.GetEnumFromDescription<ActionTypes>())
            {
                case ActionTypes.CWID:
                case ActionTypes.StationID:
                case ActionTypes.Data:
                case ActionTypes.PrivateData:
                case ActionTypes.GroupData:
                    return false;

                case ActionTypes.Alert:
                case ActionTypes.Group:
                case ActionTypes.GroupEmergency:
                case ActionTypes.GroupEncrypted:
                case ActionTypes.Queued:
                case ActionTypes.QueuedDataGrant:
                case ActionTypes.Affiliate:
                case ActionTypes.Unaffiliate:
                case ActionTypes.Denied:
                case ActionTypes.Forced:
                case ActionTypes.Refused:
                    return true;

                default:
                    throw new ArgumentOutOfRangeException(nameof(action));
            }
        }

        public TowerFrequencyUsage CreateTowerFrequencyUsage(int systemID, int towerNumber, string frequency, string channel, DateTime timeStamp)
        {
            if (string.IsNullOrWhiteSpace(frequency) || frequency.Equals(EMPTY_FREQUENCY, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var towerFrequency = Create<TowerFrequencyUsage>();

            towerFrequency.SystemID = systemID;
            towerFrequency.TowerID = towerNumber;
            towerFrequency.Channel = channel;
            towerFrequency.Frequency = frequency;
            towerFrequency.Date = timeStamp.Date;
            towerFrequency.HitCount = 0;
            towerFrequency.FirstSeen = timeStamp;
            towerFrequency.LastSeen = timeStamp;

            return towerFrequency;
        }
    }
}
