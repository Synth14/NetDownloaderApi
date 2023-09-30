using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDownloader.Entity.Models
{
    public class Hosts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HostId { get; set; }
        public string Hostname { get; set; }
        [ForeignKey(nameof(Accounts.AccountId))]
        public int AccountId { get; set; }

        public Accounts Account { get; set; }
    }
}