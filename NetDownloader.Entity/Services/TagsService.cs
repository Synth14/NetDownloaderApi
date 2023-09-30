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

        public async Task CreateTagsAsync(Tags tags)
        {
            _context.Add(tags);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTagsAsync(Hosts Hosts)
        {
            _context.Update(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTagsAsync(int id)
        {
            var tags = await _context.TagItems.FindAsync(id);
            _context.TagItems.Remove(tags);
            await _context.SaveChangesAsync();
        }
    }
}
