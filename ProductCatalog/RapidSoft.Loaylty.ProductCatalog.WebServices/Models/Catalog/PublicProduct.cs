namespace RapidSoft.Loaylty.ProductCatalog.WebServices.Models.Catalog
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class PublicProduct
    {
        [DataMember]
        public string ProductId { get; set; }

        [DataMember]
        public int PartnerId { get; set; }

        [DataMember]
        public int CategoryId { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string[] Pictures { get; set; }

        [DataMember]
        public string Vendor { get; set; }

        [DataMember]
        public ProductParameterValue[] Parameters { get; set; }

        [DataMember]
        public bool IsActionPrice { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public DateTime InsertedDate { get; set; }
    }
}
