using FileService.Interfaces;
using Microsoft.Graph;
using ObjectLibrary;
using ObjectLibrary.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static FileService.Factory;

namespace FileService.Parsers
{
    public class ParsePatchLog : IParser
    {
        private const int COL_TIMESTAMP = 0;

        private readonly int _systemID;

        public ParsePatchLog(int systemID)
        {
            _systemID = systemID;
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            IPatchLogParser patchLogParser = default;
            var patches = CreatList<PatchLog>();

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
                            if (patchLogParser == null)
                            {
                                patchLogParser = GetPatchLogParser(_systemID, towerNumber, year, row);
                            }

                            patches.AddRange(patchLogParser.ParsePatchLog(row));
                        }

                        row = parser.GetNextRow();
                        count++;
                        updateProgress("Parsing patches", $"{count:#,##0} records parsed", count, count);
                    }
                }
            });

            return patches;
        }
    }
}
