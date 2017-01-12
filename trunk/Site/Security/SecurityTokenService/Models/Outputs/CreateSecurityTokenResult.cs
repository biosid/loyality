using System;

namespace Vtb24.Site.Security.SecurityTokenService.Models.Outputs
{
    public class CreateSecurityTokenResult
    {
        public string SecurityToken { get; set; }

        public string ExternalId { get; set; }

        public DateTime CreationTimeUtc { get; set; }

        public DateTime ExpirationTimeUtc { get; set; }
    }
}