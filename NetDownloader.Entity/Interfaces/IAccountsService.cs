using NetDownloader.Entity.Models;

namespace NetDownloader.Entity.Interfaces
{
    public interface IAccountsService
    {
        Task CreateAccountsAsync(Hosts Hosts);
        Task DeleteAccountsAsync(int id);
        Task<Accounts> GetAccountsByIdAsync(int id);
        Task<IEnumerable<Accounts>> GetAllAccountsAsync();
        Task UpdateAccountsAsync(Hosts Hosts);
    }
}