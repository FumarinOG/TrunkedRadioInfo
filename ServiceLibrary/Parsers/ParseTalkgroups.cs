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
    public class ParseTalkgroups : ParseBase, IParser
    {
        private CSVReader _csvReader;
        private ITalkgroupParser _talkgroupParser;
        private Action<string, string, int, int> _updateProgress;

        private readonly int _systemID;

        public ParseTalkgroups(int systemID)
        {
            _systemID = systemID;
        }

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            var talkgroups = CreatList<Talkgroup>();

            using (var reader = await CreateReader(fileName, graphClient))
            {
                _csvReader = CreateParser(reader);
                _updateProgress = updateProgress;

                var row = _csvReader.GetNextRow();

                _talkgroupParser = GetTalkgroupParser(GetFileVersion(row), _systemID);
                await ParseTalkgroupsAsync(talkgroups);
            }

            return talkgroups;
        }

        private async Task ParseTalkgroupsAsync(ICollection<Talkgroup> talkgroups)
        {
            await Task.Run(() =>
            {
                var row = _csvReader.GetNextRow();
                var count = 0;

                while (row != null)
                {
                    if (!row[0].Contains("#"))
                    {
                        talkgroups.Add(_talkgroupParser.ParseTalkgroup(row));
                        count++;
                        _updateProgress("Parsing talkgroups", $"{count:#,##0} records parsed", count, count);
                    }

                    row = _csvReader.GetNextRow();
                }
            });
        }
    }
}
