using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public interface ILinksService
    {
        Task CreateLinksAsync(Hosts Hosts);
        Task DeleteLinksAsync(int id);
        Task<IEnumerable<Links>> GetAllLinksAsync();
        Task<Links> GetLinksByIdAsync(int id);
        Task UpdateLinksAsync(Hosts Hosts);
    }
}