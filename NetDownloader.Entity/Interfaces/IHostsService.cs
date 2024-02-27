using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Interfaces
{
    public interface IHostsService
    {
        Task<bool> CreateHostAsync(Hosts Hosts);
        Task<bool> DeleteHostByIdAsync(int id);
        Task<IEnumerable<Hosts>> GetAllHostsAsync();
        Task<Hosts> GetHostByIdAsync(int id);
        Task<bool> UpdateHostAsync(Hosts Hosts);
    }
}