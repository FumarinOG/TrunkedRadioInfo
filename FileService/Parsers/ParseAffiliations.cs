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
    public class ParseAffiliations : IParser
    {
        private const int COL_TIMESTAMP = 0;

        private readonly int _systemID;

        public ParseAffiliations(int systemID)
        {
            _systemID = systemID;
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            IAffiliationParser affiliationParser = default;
            var affiliations = CreatList<Affiliation>();

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
                            if (affiliationParser == null)
                            {
                                affiliationParser = GetAffiliationParser(_systemID, towerNumber, year, row);
                            }

                            affiliations.AddRange(affiliationParser.ParseAffiliation(row));
                        }

                        row = parser.GetNextRow();
                        count++;
                        updateProgress("Parsing affiliations", $"{count:#,##0} records parsed", count, count);
                    }
                }
            });

            return affiliations;
        }
    }
}
