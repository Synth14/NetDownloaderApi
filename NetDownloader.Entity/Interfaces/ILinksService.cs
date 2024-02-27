using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public interface ILinksService
    {
        Task<bool> CreateLinkAsync(Links link);
        Task<bool> DeleteLinkAsync(int id);
        Task<IEnumerable<Links>> GetAllLinksAsync();
        Task<Links> GetLinkByIdAsync(int id);
        Task<bool> UpdateLinkAsync(Links link);
    }
}