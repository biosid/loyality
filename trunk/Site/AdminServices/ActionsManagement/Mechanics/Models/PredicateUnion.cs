using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class PredicateUnion
    {
        public PredicateUnionType Type { get; set; }

        [DataMember(Name = "Type")]
        public string JsonType
        {
            get { return MappingsFromService.ToJsonPredicateUnionType(Type); }
            set { Type = MappingsToService.ToPredicateUnionType(value); }
        }

        [DataMember]
        public PredicateUnion[] Unions { get; set; }

        [DataMember]
        public PredicateOperation[] Operations { get; set; }
    }
}
