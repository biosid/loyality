namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    [DataContract]
    public class Location
    {
        [DataMember]
        public string KladrCode
        {
            get;
            set;
        }
        
        [DataMember]
        public string PostCode
        {
            get;
            set;
        }
    }
}