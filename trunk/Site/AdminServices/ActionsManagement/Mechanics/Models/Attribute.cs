using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class Attribute
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string FullDisplayName { get; set; }

        [DataMember]
        public string DictionaryId { get; set; }

        public AttributeType Type { get; set; }

        [DataMember(Name = "Type")]
        public string JsonType
        {
            get { return MappingsFromService.ToJsonAttributeType(Type); }
            set { Type = MappingsToService.ToAttributeType(value); }
        }

    }
}
