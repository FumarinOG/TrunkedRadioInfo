using System;
using System.Collections.Generic;

namespace FileService.Parsers
{
    public abstract class ParseBase
    {
        protected static int GetFileVersion(IReadOnlyList<string> row)
        {
            if (row.Count != 1)
            {
                return 0;
            }

            for (var version = 1; version <= 10; version++)
            {
                if (row[0].Equals($"FileVersion:{version:0}", StringComparison.OrdinalIgnoreCase))
                {
                    return version;
                }
            }

            return 0;
        }
    }
}
