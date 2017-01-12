using System;
using System.ComponentModel.DataAnnotations;

namespace Vtb24.Site.Content.Advertisements.Models
{
    public class Advertisement
    {
        [Key]
        public long Id { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required, StringLength(2048)]
        public string Url { get; set; }

        [Required, StringLength(512)]
        public string PictureUrl { get; set; }

        [StringLength(256)]
        public string CssClass { get; set; }

        public int? MaxDisplayCount { get; set; }

        public DateTime? ShowUntil { get; set; }
    }
}
