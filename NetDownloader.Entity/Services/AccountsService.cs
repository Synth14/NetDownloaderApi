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
        public async Task<bool> CreateAccountAsync(Accounts account)
        {
            _context.Add(account);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<bool> DeleteAccountByIdAsync(int id)
        {
            var accounts = await _context.AccountItems.FindAsync(id);
            _context.AccountItems.Remove(accounts);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }

        public async Task<Accounts> GetAccountByIdAsync(int id)
        {
            return await _context.AccountItems.FirstOrDefaultAsync(m => m.AccountId == id);
        }

        public async Task<IEnumerable<Accounts>> GetAllAccountsAsync()
        {
            return await _context.AccountItems.ToListAsync();
        }

        public async Task<bool> UpdateAccountAsync(Accounts account)
        {
            _context.Update(account);
            var result = await _context.SaveChangesAsync();
            return result > 0;
        }
    }
}
