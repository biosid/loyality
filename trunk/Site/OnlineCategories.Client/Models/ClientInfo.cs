using System;

namespace Vtb24.OnlineCategories.Client.Models
{
    public class ClientInfo
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string Email { get; set; }

        public string NameLanguageCode { get; set; }

        public string City { get; set; }

        public decimal? Balance { get; set; }

        public decimal BonusRate { get; set; }

        public decimal BonusDelta { get; set; }

        public DateTime UtcWhen { get; set; }
    }
}
