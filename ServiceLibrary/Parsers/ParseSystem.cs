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
    public class ParseSystem : IParser
    {
        private const string SYSTEM_HEADER = "[System]";
        private const string COL_SYSTEM_ID = "SystemID";
        private const string COL_SYSTEM_ID_HEX = "SystemIDHEX";
        private const string COL_WACN = "WACN";
        private const string COL_DESCRIPTION = "Description";
        private const string TOWERS_HEADER = "[Towers]";

        public async Task<IEnumerable<IRecord>> ParseFileAsync(string fileName, Action<string, string, int, int> updateProgress, GraphServiceClient graphClient = null)
        {
            var systemInfo = Create<SystemInfo>();
            var foundTowers = false;

            systemInfo.FirstSeen = DateTime.MinValue;
            systemInfo.LastSeen = DateTime.MinValue;

            await Task.Run(async () =>
            {
                using (var reader = await CreateReader(fileName, graphClient))
                {
                    var parser = CreateParser(reader);
                    var row = parser.GetNextRow();

                    while (row != null)
                    {
                        if (row.Length == 1)
                        {
                            var rowData = row[0];

                            if ((!rowData.Equals(SYSTEM_HEADER, StringComparison.CurrentCultureIgnoreCase)) && 
                                (!rowData.Equals(TOWERS_HEADER, StringComparison.CurrentCultureIgnoreCase)))
                            {
                                if ((rowData.Length > COL_SYSTEM_ID.Length) && (rowData.Substring(0, rowData.IndexOf("=", StringComparison.Ordinal))
                                    .Equals(COL_SYSTEM_ID, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    systemInfo.SystemIDDecimal =
                                        Convert.ToInt32(rowData.Substring(COL_SYSTEM_ID.Length + 1));
                                }
                                else if ((rowData.Length > COL_SYSTEM_ID_HEX.Length) && (rowData.Substring(0, rowData.IndexOf("=", StringComparison.Ordinal))
                                    .Equals(COL_SYSTEM_ID_HEX, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    systemInfo.SystemID = rowData.Substring(COL_SYSTEM_ID_HEX.Length + 1);
                                }
                                else if ((rowData.Length > COL_WACN.Length) && (rowData.Substring(0, rowData.IndexOf("=", StringComparison.Ordinal))
                                    .Equals(COL_WACN, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    systemInfo.WACN = rowData.Substring(COL_WACN.Length + 1);
                                }
                                else if ((rowData.Length > COL_DESCRIPTION.Length) && (rowData.Substring(0, rowData.IndexOf("=", StringComparison.Ordinal))
                                    .Equals(COL_DESCRIPTION, StringComparison.CurrentCultureIgnoreCase)))
                                {
                                    systemInfo.Description = rowData.Substring(COL_DESCRIPTION.Length + 1);
                                }
                                else
                                {
                                    if (foundTowers)
                                    {
                                        systemInfo.AddTower(systemInfo.ID, Common.FixTowerNumber(rowData.Substring(0, rowData.IndexOf("=", StringComparison.Ordinal))),
                                            rowData.Substring(rowData.IndexOf("=", StringComparison.Ordinal) + 1));
                                    }
                                }
                            }
                            else if (rowData.Equals(TOWERS_HEADER, StringComparison.CurrentCultureIgnoreCase))
                            {
                                foundTowers = true;
                            }
                        }

                        row = parser.GetNextRow();
                    }
                }
            });

            return new List<SystemInfo> { systemInfo };
        }
    }
}
