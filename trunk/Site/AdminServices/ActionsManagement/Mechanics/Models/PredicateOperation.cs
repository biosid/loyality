using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class PredicateOperation
    {
        public PredicateOperator Operator { get; set; }

        [DataMember(Name = "Operator")]
        public string JsonOperator
        {
            get { return MappingsFromService.ToJsonOperator(Operator); }
            set
            {
                Operator = MappingsToService.ToPredicateOperator(value);
            }
        }

        public AttributeType Type { get; set; }

        [DataMember(Name = "Type")]
        public string JsonType
        {
            get { return MappingsFromService.ToJsonAttributeType(Type); }
            set
            {
                Type = MappingsToService.ToAttributeType(value);
            }
        }

        [DataMember]
        public string Attribute { get; set; }

        [DataMember]
        public string[] Values { get; set; }
    }
}
