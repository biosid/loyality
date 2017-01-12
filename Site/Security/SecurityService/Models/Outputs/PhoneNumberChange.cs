using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vtb24.Site.Security.SecurityService.Models.Outputs
{
    public class PhoneNumberChange
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        internal User User { get; set; }

        [Required]
        public DateTime ChangeTime { get; set; }

        [Required]
        [StringLength(11)]
        public string OldPhoneNumber { get; set; }

        [Required]
        [StringLength(11)]
        public string NewPhoneNumber { get; set; }

        [StringLength(256)]
        public string ChangedBy { get; set; }
    }
}