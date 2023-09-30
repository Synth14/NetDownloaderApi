using System.ComponentModel.DataAnnotations;

namespace NetDownloader.Entity.Models
{
    public class Tag
    {
        /// <summary>
        /// Id => Tag ID
        /// Unique tagName
        /// TagPath => Path linked to the tag, meaning If the tag is [Movies] then it will have the path "c://downloads//movies" for example
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string TagName { get; set; }

        public string TagPath { get; set; }
    }
}