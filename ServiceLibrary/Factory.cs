using DataLibrary.Interfaces;
using Microsoft.Graph;
using ObjectLibrary.Abstracts;
using ObjectLibrary.Interfaces;
using ServiceLibrary.Interfaces.Parsers;
using ServiceLibrary.Parsers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLibrary
{
    public static class Factory
    {
        #region Constants
        private const int AFFILIATIONS_V1_ROW_LENGTH = 5;
        private const int AFFILIATIONS_V2_ROW_LENGTH = 6;
        private const int PATCH_LOG_ROW_LENGTH = 2;
        private const int RADIOS_VERSION = 4;
        private const int TALKGROUPS_VERSION_5 = 5;
        private const int TALKGROUPS_VERSION_6 = 6;
        private const int TALKGROUPS_VERSION_7 = 7;
        private const int TOWER_FREQUENCY_VERSION_4 = 4;
        private const int TOWER_FREQUENCY_VERSION_5 = 5;
        private const int TOWER_FREQUENCY_VERSION_6 = 6;
        private const int TOWER_FREQUENCY_VERSION_7 = 7;
        private const int TOWER_FREQUENCY_VERSION_8 = 8;
        private const int TOWER_TABLE_VERSION_4 = 4;
        private const int TOWER_TABLE_VERSION_5 = 5;
        private const int TOWER_TABLE_VERSION_6 = 6;
        private const int TOWER_TABLE_VERSION_7 = 7;
        private const int TOWER_TABLE_VERSION_8 = 8;
        private const int TOWER_NEIGHBOR_VERSION_4 = 4;
        private const int TOWER_NEIGHBOR_VERSION_5 = 5;
        private const int TOWER_NEIGHBOR_VERSION_6 = 6;
        private const int TOWER_NEIGHBOR_VERSION_7 = 7;
        private const int TOWER_NEIGHBOR_VERSION_8 = 8;
        #endregion

        public static T Create<T>() where T : AuditableBase, new()
        {
            return new T
            {
                IsNew = true
            };
        }


        public static List<T> CreatList<T>() where T : AuditableBase, new()
        {
            return new List<T>();
        }

        public static List<T1> CreateTempList<T1, T2>() where T1 : ITempRecord<T2>, new() where T2 : IRecord
        {
            return new List<T1>();
        }

        public static T1 CreateTemp<T1, T2>() where T1 : ITempRecord<T2>, new() where T2 : IRecord
        {
            return new T1();
        }

        public static CSVReader CreateParser(StreamReader reader)
        {
            return new CSVReader(reader);
        }

        public static async Task<StreamReader> CreateReader(string fileName, GraphServiceClient graphClient = null)
        {
            if (graphClient != null)
            {
                var stream = await graphClient.Drive.Root.ItemWithPath(fileName).Content.Request().GetAsync();

                return new StreamReader(stream, Encoding.ASCII);
            }

            return new StreamReader(fileName, Encoding.ASCII);
        }

        public static IAffiliationParser GetAffiliationParser(int systemID, int towerNumber, int year, string[] row)
        {
            switch (row.Length)
            {
                case AFFILIATIONS_V1_ROW_LENGTH:
                    return new ParseAffiliationsV1(systemID, towerNumber, year);

                case AFFILIATIONS_V2_ROW_LENGTH:
                    return new ParseAffiliationsV2(systemID, towerNumber, year);

                default:                                            // There was a period of time where the affiliation
                                                                    // logs didn't have CRLFs  at the end of the lines 
                    if (row.Length > AFFILIATIONS_V2_ROW_LENGTH)    // so you would end up with one long string for all
                    {                                               // of the affiliations.
                        return new ParseAffiliationsLong(systemID, towerNumber, year);
                    }

                    return null;
            }
        }

        public static IPatchLogParser GetPatchLogParser(int systemID, int towerNumber, int year, string[] row)
        {
            switch (row.Length)
            {
                case PATCH_LOG_ROW_LENGTH:
                    return new ParsePatchLogV1(systemID, towerNumber, year);

                default:                                    // There was a period of time where the patch logs didn't
                                                            // have CRLFs at the end of the lines so you would end up
                    if (row.Length > PATCH_LOG_ROW_LENGTH)  // with one long string for all of the patch logs 
                    {
                        return new ParsePatchLogLong(systemID, towerNumber, year);
                    }

                    return null;
            }
        }

        public static IRadioParser GetRadioParser(int fileVersion, int systemID)
        {
            switch (fileVersion)
            {
                case RADIOS_VERSION:
                    return new ParseRadiosV4(systemID);

                default:
                    return null;
            }
        }

        public static ITalkgroupParser GetTalkgroupParser(int fileVersion, int systemID)
        {
            switch (fileVersion)
            {
                case TALKGROUPS_VERSION_5:
                    return new ParseTalkgroupsV5(systemID);

                case TALKGROUPS_VERSION_6:
                    return new ParseTalkgroupsV6(systemID);

                case TALKGROUPS_VERSION_7:
                    return new ParseTalkgroupsV7(systemID);

                default:
                    return null;
            }
        }

        public static ITowerFrequencyParser GetTowerFrequencyParser(int systemID, int fileVersion)
        {
            switch (fileVersion)
            {
                case TOWER_FREQUENCY_VERSION_4:
                    return new ParseTowerFrequencyV4(systemID);

                case TOWER_FREQUENCY_VERSION_5:
                    return new ParseTowerFrequencyV5(systemID);

                case TOWER_FREQUENCY_VERSION_6:
                    return new ParseTowerFrequencyV6(systemID);

                case TOWER_FREQUENCY_VERSION_7:
                    return new ParseTowerFrequencyV7(systemID);

                case TOWER_FREQUENCY_VERSION_8:
                    return new ParseTowerFrequencyV8(systemID);

                default:
                    return null;
            }
        }

        public static ITowerTableParser GetTowerTableParser(int systemID, int fileVersion)
        {
            switch (fileVersion)
            {
                case TOWER_TABLE_VERSION_4:
                    return new ParseTowerTableV4(systemID);

                case TOWER_TABLE_VERSION_5:
                    return new ParseTowerTableV5(systemID);

                case TOWER_TABLE_VERSION_6:
                    return new ParseTowerTableV6(systemID);

                case TOWER_TABLE_VERSION_7:
                    return new ParseTowerTableV7(systemID);

                case TOWER_TABLE_VERSION_8:
                    return new ParseTowerTableV8(systemID);

                default:
                    return null;
            }
        }

        public static ITowerNeighborParser GetTowerNeighborParser(int systemID, int fileVersion)
        {
            switch (fileVersion)
            {
                case TOWER_NEIGHBOR_VERSION_4:
                    return new ParseTowerNeighborV4(systemID);

                case TOWER_NEIGHBOR_VERSION_5:
                    return new ParseTowerNeighborV5(systemID);

                case TOWER_NEIGHBOR_VERSION_6:
                    return new ParseTowerNeighborV6(systemID);

                case TOWER_NEIGHBOR_VERSION_7:
                    return new ParseTowerNeighborV7(systemID);

                case TOWER_NEIGHBOR_VERSION_8:
                    return new ParseTowerNeighborV8(systemID);

                default:
                    return null;
            }
        }
    }
}
