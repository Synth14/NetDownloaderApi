using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public interface ITagsService
    {
        Task CreateTagsAsync(Tags tags);
        Task DeleteTagsAsync(int id);
        Task<IEnumerable<Tags>> GetAllTagsAsync();
        Task<Tags> GetTagsByIdAsync(int id);
        Task UpdateTagsAsync(Hosts Hosts);
    }
}