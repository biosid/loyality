using System;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Outputs
{
    public class ValidateSecurityTokenResult
    {
        public bool IsValid { get; set; }

        public string PrincipalId { get; set; }

        public string ExternalId { get; set; }

        public int? CategoryId { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }
    }
}