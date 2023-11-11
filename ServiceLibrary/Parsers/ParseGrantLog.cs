using Microsoft.Graph;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using ServiceLibrary.Interfaces.Parsers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static ServiceLibrary.Factory;

namespace ServiceLibrary.Parsers
{
    public class ParseGrantLog : IParser
    {
        private const int COL_TIMESTAMP = 0;
        private const int COL_TYPE = 1;
        private const int COL_CHANNEL = 2;
        private const int COL_FREQUENCY = 3;
        private const int COL_TALKGROUP_ID = 4;
        private const int COL_TALKGROUP_DESCRIPTION = 5;
        private const int COL_RADIO_ID = 6;
        private const int COL_RADIO_DESCRIPTION = 7;

        private readonly int _systemID;

        public ParseGrantLog(int systemID)
        {
            _systemID = systemID;
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            var grantLogs = CreatList<GrantLog>();

            await Task.Run(async () =>
            {
                var towerNumber = Common.ParseTowerNumber(fileName);
                var year = Common.GetYear(fileName);

                using (var reader = await CreateReader(fileName, graphClient))
                {
                    var parser = CreateParser(reader);
                    var count = 0;
                    var row = parser.GetNextRow();

                    while (row != null)
                    {
                        if (row[COL_TIMESTAMP].Contains(":"))
                        {
                            grantLogs.Add(CreateGrantLog(towerNumber, year, row));
                        }

                        row = parser.GetNextRow();
                        count++;
                        updateProgress("Parsing grant log", $"{count:#,##0} records parsed", count, 0);
                    }
                }
            });

            return grantLogs;
        }

        private static GrantLog CreateGrantLog(int towerNumber, int year, IReadOnlyList<string> row)
        {
            var grantLog = Create<GrantLog>();

            grantLog.TowerNumber = towerNumber;
            grantLog.TimeStamp = Common.FixDate(row[COL_TIMESTAMP], year);
            grantLog.Type = row[COL_TYPE];
            grantLog.Channel = row[COL_CHANNEL];
            grantLog.Frequency = row[COL_FREQUENCY];
            grantLog.TalkgroupID = Common.ParseID(row[COL_TALKGROUP_ID]);
            grantLog.TalkgroupDescription = row[COL_TALKGROUP_DESCRIPTION];
            grantLog.RadioID = Common.ParseID(row[COL_RADIO_ID]);
            grantLog.RadioDescription = row[COL_RADIO_DESCRIPTION];

            return grantLog;
        }
    }
}
