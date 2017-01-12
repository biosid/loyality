using System;

namespace Vtb24.Site.Services.VtbBankConnector.Models.Inputs
{
    public class RegisterClientParams
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
