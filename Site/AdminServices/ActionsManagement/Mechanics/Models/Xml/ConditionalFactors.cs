using System;
using System.Xml.Serialization;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Xml
{

    [Serializable]
    [XmlRoot(ElementName = "ArrayOfConditionalFactor", Namespace = "", IsNullable = false)]
    public class ConditionalFactors
    {
        [XmlElement("ConditionalFactor")]
        public ConditionalFactor[] Factors { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class ConditionalFactor
    {
        [XmlElement("Priority")]
        public Priority Priority { get; set; }

        [XmlElement("Predicate")]
        public filter Predicate { get; set; }

        [XmlElement("Factor")]
        public Factor Factor { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Priority
    {
        [XmlText]
        public int Value { get; set; }
    }

    [Serializable]
    [XmlType(AnonymousType = true)]
    public class Factor
    {
        [XmlText]
        public decimal Value { get; set; }
    }
}
