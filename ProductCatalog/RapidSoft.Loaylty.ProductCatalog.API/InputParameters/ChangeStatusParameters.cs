namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class ChangeStatusParameters
    {
        [DataMember]
        public string UserId
        {
            get;
            set;
        }

        [DataMember]
        public string[] ProductIds
        {
            get;
            set;
        }

        [DataMember]
        public ProductStatuses ProductStatus
        {
            get;
            set;
        }

    }
}
