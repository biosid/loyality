using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vtb24.Site.Security.SecurityService.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string ClientId { get; set; }

        [Required]
        [StringLength(11)]
        public string PhoneNumber { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsPasswordSet { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int OtpViaSmsCount { get; set; }
    }
}