using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Interfaces;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class HostsService : IHostsService
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

        public async Task<Hosts> GetHostByIdAsync(int id)
        {
            return await _context.HostItems.FirstOrDefaultAsync(m => m.HostId == id);
        }

        public async Task<bool> CreateHostAsync(Hosts Hosts)
        {
            _context.Add(Hosts);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> UpdateHostAsync(Hosts Hosts)
        {
            _context.Update(Hosts);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteHostByIdAsync(int id)
        {
            var Hosts = await _context.HostItems.FindAsync(id);
            _context.HostItems.Remove(Hosts);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
