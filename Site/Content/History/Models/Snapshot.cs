using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vtb24.Site.Content.History.Models
{
    public class Snapshot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(256)]
        public string Author { get; set; }

        [Required]
        public DateTime When { get; set; }
    }
}
