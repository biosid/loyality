using System.Xml.Serialization;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models.Xml
{
    public class XmlResponseItem
    {
        [XmlElement(ElementName = "ordernumber")]
        public string OrderNumber { get; set; }

        [XmlElement(ElementName = "response_code")]
        public string ResponseCode { get; set; }

        [XmlElement(ElementName = "billnumber")]
        public string BillNumber { get; set; }
    }
}
