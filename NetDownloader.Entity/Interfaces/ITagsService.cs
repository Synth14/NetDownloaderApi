using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public interface ITagsService
    {
        Task CreateTagsAsync(Hosts Hosts);
        Task DeleteTagsAsync(int id);
        Task<IEnumerable<Tags>> GetAllTagsAsync();
        Task<Tags> GetTagsByIdAsync(int id);
        Task UpdateTagsAsync(Hosts Hosts);
    }
}