using System.ComponentModel.DataAnnotations;

namespace NetDownloader.Entity.Models
{
    public class Accounts
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public string ApiKey { get; set; }

    }
}