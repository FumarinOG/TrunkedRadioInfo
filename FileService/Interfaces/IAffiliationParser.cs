using ObjectLibrary;
using System.Collections.Generic;

namespace FileService.Interfaces
{
    public interface IAffiliationParser
    {
        IEnumerable<Affiliation> ParseAffiliation(string[] row);
    }
}
