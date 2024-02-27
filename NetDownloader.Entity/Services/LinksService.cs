using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class LinksService : ILinksService
    {
        private readonly ApplicationDbContext _context;

        public LinksService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Links>> GetAllLinksAsync()
        {
            return await _context.LinkItems.ToListAsync();
        }

        public async Task<Links> GetLinkByIdAsync(int id)
        {
            return await _context.LinkItems.FirstOrDefaultAsync(m => m.LinksId == id);
        }

        public async Task<bool> CreateLinkAsync(Links link)
        {
            _context.Add(link);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateLinkAsync(Links link)
        {
            _context.Update(link);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteLinkAsync(int id)
        {
            var Links = await _context.AccountItems.FindAsync(id);
            _context.AccountItems.Remove(Links);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
