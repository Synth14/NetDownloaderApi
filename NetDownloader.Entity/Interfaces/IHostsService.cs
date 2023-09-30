using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Interfaces
{
    public interface IHostsService
    {
        Task CreateHostsAsync(Hosts Hosts);
        Task DeleteHostsAsync(int id);
        Task<IEnumerable<Hosts>> GetAllHostsAsync();
        Task<Hosts> GetHostsByIdAsync(int id);
        Task UpdateHostsAsync(Hosts Hosts);
    }
}