using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Interfaces
{
    public interface ILinksController
    {
        Task CreateHostsAsync(Hosts Hosts);
        Task DeleteHostsAsync(int id);
        Task<IEnumerable<Hosts>> GetAllHostsAsync();
        Task<Hosts> GetHostsByIdAsync(int id);
        Task UpdateHostsAsync(Hosts Hosts);
    }
}