using System;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Services.MyInfoService.Models
{
    public class MyInfo
    {
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public ClientProfileGender Gender { get; set; }

        public ClientStatus Status { get; set; }

        public string LocationKladr { get; set; }

        public string LocationTitle { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public CustomField[] CustomFields { get; set; }
    }
}