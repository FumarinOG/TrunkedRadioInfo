using System.Threading.Tasks;

namespace SystemDetailService
{
    public interface ISystemDetailService
    {
        Task<SystemDetailViewModel> GetAsync(string systemID);
    }
}
