namespace RapidSoft.VTB24.BankConnector.API.Entities
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = Globals.DefaultNamespace)]
    public class ClientProfileCustomField
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
