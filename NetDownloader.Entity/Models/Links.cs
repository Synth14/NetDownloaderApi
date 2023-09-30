using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Asn1;

namespace NetDownloader.Entity.Models
{
    public class Links
    {

        [Key]
        public int LinksId { get; set; }
        public string Url { get; set; }
        [ForeignKey("Host")]
        public int HostId { get; set; }
        [ForeignKey("Tag")]
        public int TagId { get; set; }

        public Host Host { get; set; }
        public Tag Tag { get; set; }
    }
}