using System;
using System.ComponentModel.DataAnnotations;

namespace Vtb24.Site.Content.News.Models
{
    [Serializable]
    public class NewsMessage
    {
        [Key]
        [Required]
        public long Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(512)]
        public string Description { get; set; }

        [Required]
        [StringLength(2048)]
        public string Url { get; set; }

        [Required]
        public int Priority { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Required]
        public bool IsPublished { get; set; }

        [Required]
        [StringLength(512)]
        public string Picture { get; set; }

        [Required]
        [StringLength(256)]
        public string Author { get; set; }

        [StringLength(128)]
        public string Segment { get; set; }
    }
}
