using System;

namespace Vtb24.Site.SecurityWebServices.PublicProfile.Outputs
{
    public class GetPublicProfileResult
    {
        public byte Status { get; set; }

        public string Email { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string NameLang { get; set; }

        public string City { get; set; }

        public decimal Balance { get; set; }

        public decimal Rate { get; set; }

        public decimal Delta { get; set; }

        public DateTime UtcDateTime { get; set; }
    }
}