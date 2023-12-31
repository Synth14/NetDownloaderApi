﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetDownloader.Entity.Models
{
    public class Tags
    {
        /// <summary>
        /// Id => Tag ID
        /// Unique tagName
        /// TagPath => Path linked to the tag, meaning If the tag is [Movies] then it will have the path "c://downloads//movies" for example
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }
        public string TagName { get; set; }

        public string TagPath { get; set; }
    }
}