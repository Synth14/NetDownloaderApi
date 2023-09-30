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
        [ForeignKey(nameof(Hosts.HostId))]
        public int HostId { get; set; }
        [ForeignKey(nameof(Tags.TagId))]

        public int TagId { get; set; }

        public Hosts Host { get; set; }
        public Tags Tag { get; set; }
    }
}