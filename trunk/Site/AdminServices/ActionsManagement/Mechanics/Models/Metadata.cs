using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class Metadata
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public Attribute[] Attributes { get; set; }
    }
}
