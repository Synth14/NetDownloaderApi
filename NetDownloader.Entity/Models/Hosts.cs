using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDownloader.Entity.Models
{
    public class Hosts
    {
        [Key]
        public int HostId { get; set; }
        public string Hostname { get; set; }
        [ForeignKey(nameof(Accounts.AccountId))]
        public string AccountId { get; set; }

        public Accounts Account { get; set; }
    }
}