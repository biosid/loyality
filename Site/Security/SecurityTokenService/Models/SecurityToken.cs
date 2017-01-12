using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Vtb24.Site.Security.SecurityTokenService.Models
{
    internal class SecurityToken
    {
        public const int DEFAULT_TIMEOUT_MINUTES = 24*60;

        [Key]
        [StringLength(32)]
        public string Token { get; set; }

        [Required]
        [StringLength(255)]
        public string PrincipalId { get; set; }

        [StringLength(255)]
        public string ExternalId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public bool IsExpired(DateTime? utcNow = null)
        {
            utcNow = utcNow ?? DateTime.UtcNow;

            return ExpirationTimeUtc < utcNow;
        }

        public static SecurityToken Create(int timeoutMinutes = DEFAULT_TIMEOUT_MINUTES)
        {
            var token = new SecurityToken
            {
                Token = Guid.NewGuid().ToString("N"),
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddMinutes(timeoutMinutes)
            };

            return token;
        }
    }
}