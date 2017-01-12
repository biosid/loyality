namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.Text.RegularExpressions;

    [DataContract(Namespace = Globals.DefaultNamespace)]
    public class UpdateClientPhoneNumberRequest
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string UserId { get; set; }

        public void Validate()
        {
            Contract.Requires(this.ClientId != null);
            Contract.Requires(this.UserId != null);

            Contract.Requires(this.PhoneNumber == null || this.PhoneNumber.Length <= 20);
            Contract.Requires(this.PhoneNumber == null || Regex.IsMatch(this.PhoneNumber, "^\\+?[0-9]+$"));
        }
    }
}
