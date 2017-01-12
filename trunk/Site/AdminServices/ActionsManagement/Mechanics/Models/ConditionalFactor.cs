using System.Runtime.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models
{
    [DataContract]
    public class ConditionalFactor
    {
        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public decimal Factor { get; set; }

        [DataMember]
        public Predicate Predicate { get; set; }
    }
}
