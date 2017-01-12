using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vtb24.Site.Content.Advertisements.Models
{
    public class ClientAdvertisement
    {
        [Column(Order = 0), Key, ForeignKey("Advertisement")]
        public long Advertisement_Id { get; set; }

        [Column(Order = 1), Key, StringLength(255)]
        public string ClientId { get; set; }

        [Required]
        public int ShowCounter { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}