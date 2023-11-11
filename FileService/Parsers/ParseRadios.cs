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
    public class ParseRadios : ParseBase, IParser
    {
        private CSVReader _csvReader;
        private IRadioParser _radioParser;
        private Action<string, string, int, int> _updateProgress;

        private readonly int _systemID;

        public ParseRadios(int systemID)
        {
            _systemID = systemID;
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            var radios = CreatList<Radio>();

            using (var reader = await CreateReader(fileName, graphClient))
            {
                _csvReader = CreateParser(reader);
                _updateProgress = updateProgress;

                var row = _csvReader.GetNextRow();

                _radioParser = GetRadioParser(GetFileVersion(row), _systemID);
                await ParseRadiosAsync(radios);
            }

            return radios;
        }

        private async Task ParseRadiosAsync(ICollection<Radio> radios)
        {
            await Task.Run(() =>
            {
                var row = _csvReader.GetNextRow();
                var count = 0;

                while (row != null)
                {
                    if (!row[0].Contains("#"))
                    {
                        radios.Add(_radioParser.ParseRadio(row));
                        count++;
                        _updateProgress("Parsing radios", $"{count:#,##0} records parsed", count, count);
                    }

                    row = _csvReader.GetNextRow();
                }
            });
        }
    }
}
