namespace Vtb24.Site.SecurityWebServices.Security.Models.Inputs
{
    public class CreateUserOptions
    {
        public string ClientId { get; set; }

        public string PhoneNumber { get; set; }

        public RegistrationType RegistrationType { get; set; }
    }
}