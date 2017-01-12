using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    [DataContract(Namespace = Globals.DefaultNamespace)]
    public class UpdateClientEmailRequest
    {
        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string UserId { get; set; }

        public void Validate()
        {
            Contract.Requires(this.ClientId != null);
            Contract.Requires(this.UserId != null);

            Contract.Requires(this.Email == null || this.Email.Length <= 255);
        }
    }
}
