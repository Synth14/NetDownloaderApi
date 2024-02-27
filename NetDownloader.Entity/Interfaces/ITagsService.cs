using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public interface ITagsService
    {
        Task<bool> CreateTagsAsync(Tags tags);
        Task<bool> DeleteTagByIdAsync(int id);
        Task<IEnumerable<Tags>> GetAllTagsAsync();
        Task<Tags> GetTagsByIdAsync(int id);
        Task<bool> UpdateTagAsync(Tags tags);
    }
}