using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;

namespace Vtb24.Site.Security.OneTimePasswordService.Models
{
    internal class OtpToken
    {
        [Key]
        [StringLength(32)]
        public string Token { get; set; }

        [Required]
        [StringLength(50)]
        public string OtpType { get; set; }

        public OtpDeliveryMeans DeliveryMeans { get; set; }

        [Required]
        [StringLength(20)]
        public string Otp { get; set; }

        [StringLength(100)]
        public string To { get; set; }

        [StringLength(255)]
        public string ExternalId { get; set; }

        [Required]
        [StringLength(255)]
        public string MessageTemplate { get; set; }

        public int AttemptsToConfirm { get; set; }

        public int AttemptsToRenew { get; set; }

        public bool IsConfirmed { get; set; }

        public bool IsFake { get; set; }

        public DateTime? ConfirmedTimeUtc { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }

        public bool IsExpired(DateTime? utcNow = null)
        {
            utcNow = utcNow ?? DateTime.UtcNow;

            return ExpirationTimeUtc < utcNow;
        }

        public static OtpToken Create(int length, int timeoutSeconds)
        {
            var token = new OtpToken
            {
                Token = Guid.NewGuid().ToString("N"),
                Otp = GenerateOtp(length),
                CreationTimeUtc = DateTime.UtcNow,
                ExpirationTimeUtc = DateTime.UtcNow.AddSeconds(timeoutSeconds)
            };
            return token;
        }

        private static string GenerateOtp(int length)
        {
            var seed = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(seed);
            var rnd = new Random(BitConverter.ToInt32(seed, 0));

            var result = "";

            for (var i = 0; i < length; i++)
            {
                result += rnd.Next(0, 9);
            }

            return result;
        }
    }
}