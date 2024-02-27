using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class TagsService : ITagsService
    {
        private readonly ApplicationDbContext _context;

        public TagsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tags>> GetAllTagsAsync()
        {
            return await _context.TagItems.ToListAsync();
        }

        public async Task<Tags> GetTagsByIdAsync(int id)
        {
            return await _context.TagItems.FirstOrDefaultAsync(m => m.TagId == id);
        }

        public async Task<bool> CreateTagsAsync(Tags tags)
        {
            _context.TagItems.Add(tags);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateTagAsync(Tags tags)
        {
            _context.Update(tags);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteTagByIdAsync(int id)
        {
            var tags = await _context.TagItems.FindAsync(id);
            _context.TagItems.Remove(tags);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
