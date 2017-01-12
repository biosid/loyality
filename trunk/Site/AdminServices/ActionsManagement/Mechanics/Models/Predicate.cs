using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class Predicate
    {
        [DataMember]
        public PredicateUnion Union { get; set; }

        [DataMember]
        public PredicateOperation Operation { get; set; }
    }
}
