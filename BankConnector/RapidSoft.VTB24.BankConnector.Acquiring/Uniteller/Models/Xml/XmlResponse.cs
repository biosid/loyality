using System.Xml.Serialization;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Xml
{
    [XmlRoot(ElementName = "unitellerresult")]
    public class XmlResponse
    {
        [XmlAttribute(AttributeName = "firstcode")]
        public string FirstCode { get; set; }

        [XmlAttribute(AttributeName = "secondcode")]
        public string SecondCode { get; set; }

        [XmlArray(ElementName = "orders")]
        [XmlArrayItem(ElementName = "order")]
        public XmlResponseItem[] Items { get; set; }
    }
}
