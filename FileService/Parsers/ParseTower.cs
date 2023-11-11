using FileService.Interfaces;
using Microsoft.Graph;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SystemInfoService;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParseTower : ParseBase, IParser
    {
        private const string TOWER_INFO = "-TowerInfo";
        private const string SYSTEM_ID = "System ID";
        private const string SYSTEM_NAME = "System Name";
        private const string WACN = "WACN";
        private const string TOWER_NUMBER_DECIMAL = "Tower Number (Decimal)";
        private const string TOWER_NUMBER_HEX = "Tower Number (Hex)";
        private const string TOWER_DESCRIPTION = "Tower Description";
        private const string CONTROL_CAPABILITIES = "Control Capabilities";
        private const string FLAVOR = "Flavor";
        private const string CALL_SIGNS = "Call Sign(s)";
        private const string TIMESTAMP = "Timestamp";
        private const string TABLES = "-Tables";
        private const string FREQUENCIES = "-Frequencies";
        private const string NEIGHBORS = "-Neighbors";

        private ITowerFrequencyParser _towerFrequencyParser;
        private ITowerTableParser _towerTableParser;
        private ITowerNeighborParser _towerNeighborParser;

        private readonly Dictionary<string, int> _systems = new Dictionary<string, int>();

        private readonly ISystemInfoService _systemInfoService;

        private readonly int _systemID;

        public ParseTower(int systemID, ISystemInfoService systemInfoService)
        {
            _systemID = systemID;
            _systemInfoService = systemInfoService;
        }

        private async Task FillSystemListAsync()
        {
            foreach (var system in await _systemInfoService.GetListAsync())
            {
                _systems.Add(system.SystemID.ToUpperInvariant(), system.ID);
            }
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            var tower = Create<Tower>();

            using (var reader = await CreateReader(fileName, graphClient))
            {
                var parser = CreateParser(reader);
                var row = parser.GetNextRow();
                var fileVersion = GetFileVersion(row);

                _towerFrequencyParser = GetTowerFrequencyParser(_systemID, fileVersion);
                _towerTableParser = GetTowerTableParser(_systemID, fileVersion);
                _towerNeighborParser = GetTowerNeighborParser(_systemID, fileVersion);

                if (row != null)
                {
                    await ParseTowerAsync(fileName, tower, parser, updateProgress);
                }
            }

            return new List<Tower> { tower };
        }

        private async Task ParseTowerAsync(string fileName, Tower tower, CSVReader parser, Action<string, string, int, int> updateProgress)
        {
            await FillSystemListAsync();

            await Task.Run(() =>
            {
                var row = parser.GetNextRow();

                while (row != null)
                {
                    if (row[0].Length != 0)
                    {
                        if (row.Length == 1)
                        {
                            if (row[0].Equals(TABLES, StringComparison.OrdinalIgnoreCase))
                            {
                                updateProgress("Parsing tower table", "Parsing tower table", 1, 1);
                                tower.AddTables(ParseData(parser, tower, (rw, tw) => _towerTableParser.ParseTowerTable(rw, tw)));
                            }
                            else if (row[0].Equals(FREQUENCIES, StringComparison.OrdinalIgnoreCase))
                            {
                                updateProgress("Parsing tower frequencies", "Parsing tower frequencies", 1, 1);
                                tower.AddFrequencies(ParseData(parser, tower, (rw, tw) => _towerFrequencyParser.ParseTowerFrequency(rw, tw)));
                            }
                            else if (row[0].Equals(NEIGHBORS, StringComparison.OrdinalIgnoreCase))
                            {
                                updateProgress("Parsing tower neighbors", "Parsing tower neighbors", 1, 1);
                                tower.AddNeighbors(ParseData(parser, tower, (rw, tw, sys) => _towerNeighborParser.ParseTowerNeighbor(rw, tw, sys)));
                            }
                            else
                            {
                                updateProgress("Parsing header section", "Parsing header section", 1, 1);

                                while (ProcessHeaderSection(row, tower, GetTowerNumber(fileName)))
                                {
                                    row = parser.GetNextRow();
                                }

                                tower.FirstSeen = tower.TimeStamp ?? DateTime.Now;
                                tower.LastSeen = tower.TimeStamp ?? DateTime.Now;
                            }
                        }
                    }

                    row = parser.GetNextRow();
                }
            });
        }

        private bool ProcessHeaderSection(IReadOnlyList<string> row, Tower tower, int towerNumber)
        {
            if ((row[0] == TOWER_INFO) || row[0].Contains("#"))
            {
                return true;
            }

            if (CheckItem(row[0], SYSTEM_ID, si => tower.SystemID = _systems[si]))
            {
                return true;
            }

            if (CheckItem(row[0], SYSTEM_NAME, sn => tower.Description = sn))
            {
                return true;
            }

            if (CheckItem(row[0], WACN, w => tower.WACN = w))
            {
                return true;
            }

            if (CheckItem(row[0], TOWER_NUMBER_DECIMAL, () => tower.TowerNumber = towerNumber))
            {
                return true;
            }

            if (CheckItem(row[0], TOWER_NUMBER_HEX, tn => tower.TowerNumberHex = tn))
            {
                return true;
            }

            if (CheckItem(row[0], TOWER_DESCRIPTION, td => tower.Description = td))
            {
                return true;
            }

            if (CheckItem(row[0], CONTROL_CAPABILITIES, cc => tower.ControlCapabilities = cc))
            {
                return true;
            }

            if (CheckItem(row[0], FLAVOR, f => tower.Flavor = f))
            {
                return true;
            }

            if (CheckItem(row[0], CALL_SIGNS, cs => tower.CallSigns = cs))
            {
                return true;
            }

            if (CheckItem(row[0], TIMESTAMP, t => tower.TimeStamp = t))
            {
                return true;
            }

            return false;
        }

        private static bool CheckItem(string item, string text, Action assignValue)
        {
            if (text.Length > item.Length)
            {
                return false;
            }

            if (item.Substring(0, text.Length).Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                assignValue();
                return true;
            }

            return false;
        }

        private static bool CheckItem(string item, string text, Action<string> assignValue)
        {
            if (text.Length > item.Length)
            {
                return false;
            }

            if (item.Substring(0, text.Length).Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                assignValue(item.Substring(item.LastIndexOf(":", StringComparison.Ordinal) + 1).Trim());
                return true;
            }

            return false;
        }

        private static bool CheckItem(string item, string text, Action<DateTime> assignValue)
        {
            if (text.Length > item.Length)
            {
                return false;
            }

            if (!item.Substring(0, text.Length).Equals(text, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var timeStamp = item.Substring(item.IndexOf(":", StringComparison.OrdinalIgnoreCase) + 1).Trim();
            var timeStampParts = timeStamp.Split(' ');
            string month;
            string day;
            string year;
            string time;

            if (timeStampParts.Length == 6)
            {
                month = timeStampParts[1];
                day = timeStampParts[3];
                year = timeStampParts[5];
                time = timeStampParts[4];
            }
            else
            {
                month = timeStampParts[1];
                day = timeStampParts[2];
                year = timeStampParts[4];
                time = timeStampParts[3];
            }

            assignValue(DateTime.Parse($"{month}-{day}-{year} {time}"));
            return true;
        }

        private static int GetTowerNumber(string fileName)
        {
            var shortFileName = fileName.Substring(fileName.LastIndexOf("\\", StringComparison.OrdinalIgnoreCase) + 1);
            var towerNumberText = shortFileName.Substring(5);
            int towerNumber;

            towerNumberText = towerNumberText.Substring(0, towerNumberText.IndexOf(".", StringComparison.OrdinalIgnoreCase));

            if (towerNumberText.Length == 4)
            {
                towerNumber = int.Parse(towerNumberText.Substring(0, 2) + "0" + towerNumberText.Substring(2));
            }
            else
            {
                towerNumber = int.Parse(towerNumberText);
            }

            return towerNumber;
        }

        private static IEnumerable<T> ParseData<T>(CSVReader parser, Tower tower, Func<string[], Tower, T> parseData) where T : IAuditable
        {
            var data = new List<T>();
            var row = parser.GetNextRow();

            while (row != null)
            {
                if (row.Length == 1)
                {
                    if (!row[0].Contains("#"))
                    {
                        break;
                    }

                    row = parser.GetNextRow();
                }
                else
                {
                    if (!row[0].Contains("#"))
                    {
                        data.Add(parseData(row, tower));
                    }

                    row = parser.GetNextRow();
                }
            }

            return data;
        }

        private IEnumerable<T> ParseData<T>(CSVReader parser, Tower tower, Func<string[], Tower, Dictionary<string, int>, T> parseData) where T : IAuditable
        {
            var data = new List<T>();
            var row = parser.GetNextRow();

            while (row != null)
            {
                if (row.Length == 1)
                {
                    if (!row[0].Contains("#"))
                    {
                        break;
                    }

                    row = parser.GetNextRow();
                }
                else
                {
                    if (!row[0].Contains("#"))
                    {
                        data.Add(parseData(row, tower, _systems));
                    }

                    row = parser.GetNextRow();
                }
            }

            return data;
        }
    }
}
