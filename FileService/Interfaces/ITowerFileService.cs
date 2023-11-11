using ObjectLibrary;
using System.Threading.Tasks;

namespace FileService.Interfaces
{
    public interface ITowerFileService : IService
    {
        Task<Tower> GetAsync(int id);
    }
}
