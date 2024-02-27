using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Interfaces
{
    public interface IAccountsService
    {
        Task<bool> CreateAccountAsync(Accounts account);
        Task<bool> DeleteAccountByIdAsync(int id);
        Task<Accounts> GetAccountByIdAsync(int id);
        Task<IEnumerable<Accounts>> GetAllAccountsAsync();
        Task<bool> UpdateAccountAsync(Accounts account);
    }
}