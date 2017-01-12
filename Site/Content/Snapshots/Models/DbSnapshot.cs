using System;
using System.ComponentModel.DataAnnotations;

namespace Vtb24.Site.Content.Snapshots.Models
{
    internal class DbSnapshot 
    {
        [Key]
        [Required]
        [StringLength(32)]
        public string Id { get; set; }

        [Required]
        public long EntityId { get; set; }

        [Required]
        public string SerializedContent { get; set; }

        [Required]
        [StringLength(256)]
        public string Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Author { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public string ContentHash { get; set; }
    }
}
