using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Interfaces;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class HostsService : ILinksController
    {
        private readonly ApplicationDbContext _context;

        public HostsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Hosts>> GetAllHostsAsync()
        {
            return await _context.HostItems.ToListAsync();
        }

        public async Task<Hosts> GetHostsByIdAsync(int id)
        {
            return await _context.HostItems.FirstOrDefaultAsync(m => m.HostId == id);
        }

        public async Task CreateHostsAsync(Hosts Hosts)
        {
            _context.Add(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHostsAsync(Hosts Hosts)
        {
            _context.Update(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHostsAsync(int id)
        {
            var Hosts = await _context.HostItems.FindAsync(id);
            _context.HostItems.Remove(Hosts);
            await _context.SaveChangesAsync();
        }
    }
}
