using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class LinksService
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

        public async Task<Links> GetLinksByIdAsync(int id)
        {
            return await _context.LinkItems.FirstOrDefaultAsync(m => m.LinksId== id);
        }

        public async Task CreateLinksAsync(Hosts Hosts)
        {
            _context.Add(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateLinksAsync(Hosts Hosts)
        {
            _context.Update(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLinksAsync(int id)
        {
            var Links = await _context.AccountItems.FindAsync(id);
            _context.AccountItems.Remove(Links);
            await _context.SaveChangesAsync();
        }
    }
}
