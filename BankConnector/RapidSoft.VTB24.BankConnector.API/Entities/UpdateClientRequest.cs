namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.Serialization;

    [DataContract(Namespace = Globals.DefaultNamespace)]
    public class UpdateClientRequest
    {
        private string email;

        private Dictionary<int, string> customFields;

        [DataMember]
        public string ClientId { get; set; }

        [DataMember]
        public string Email
        {
            get
            {
                return this.Contains(UpdateProperty.Email) ? this.email : null;
            }

            set
            {
                this.email = value;
            }
        }

        [DataMember]
        public Dictionary<int, string> CustomFields
        {
            get
            {
                return this.Contains(UpdateProperty.CustomFields) ? this.customFields : null;
            }

            set
            {
                this.customFields = value;
            }
        }

        [DataMember]
        public UpdateProperty[] UpdateProperties { get; set; }

        public void Validate()
        {
            Contract.Requires(this.ClientId != null);

            // NOTE: Если не указано что обновляем, то операция обновления бессмыслена.
            Contract.Requires(this.UpdateProperties != null && this.UpdateProperties.Length > 0);

            // Email: необязательное поле
            Contract.Requires(this.Email == null || this.Email.Length <= 255);

            // Дополнительные поля
            Contract.Requires(!this.UpdateProperties.Contains(UpdateProperty.CustomFields) || this.CustomFields != null);
        }

        public bool Contains(UpdateProperty updateProperty)
        {
            return this.UpdateProperties != null && this.UpdateProperties.Contains(updateProperty);
        }
    }
}