using ObjectLibrary;
using System.Collections.Generic;

namespace FileService.Interfaces
{
    public interface IPatchLogParser
    {
        IEnumerable<PatchLog> ParsePatchLog(string[] row);
    }
}
