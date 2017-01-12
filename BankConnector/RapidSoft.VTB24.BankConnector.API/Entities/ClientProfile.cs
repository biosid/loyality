namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System;
    using System.Runtime.Serialization;

    using RapidSoft.VTB24.BankConnector.DataModels;

    [DataContract(Namespace = Globals.DefaultNamespace)]
    public class ClientProfile
    {
        [DataMember]
        public ClientProfileStatus Status { get; set; }

        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string MiddleName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public Gender? Gender { get; set; }

        [DataMember]
        public DateTime? BirthDate { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public string LocationKladr { get; set; }

        [DataMember]
        public string LocationName { get; set; }

        [DataMember]
        public ClientProfileCustomFieldValue[] CustomFields { get; set; }
    }
}
