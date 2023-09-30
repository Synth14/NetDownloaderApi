using Microsoft.EntityFrameworkCore;
using NetDownloader.Entity.Context;
using NetDownloader.Entity.Interfaces;
using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly ApplicationDbContext _context;

        public AccountsService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task CreateAccountsAsync(Hosts Hosts)
        {
            _context.Add(Hosts);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAccountsAsync(int id)
        {
            var accounts = await _context.AccountItems.FindAsync(id);
            _context.AccountItems.Remove(accounts);
            await _context.SaveChangesAsync();
        }

        public async Task<Accounts> GetAccountsByIdAsync(int id)
        {
            return await _context.AccountItems.FirstOrDefaultAsync(m => m.AccountId == id);
        }

        public async Task<IEnumerable<Accounts>> GetAllAccountsAsync()
        {
            return await _context.AccountItems.ToListAsync();
        }

        public async Task UpdateAccountsAsync(Hosts Hosts)
        {
            _context.Update(Hosts);
            await _context.SaveChangesAsync();
        }
    }
}
