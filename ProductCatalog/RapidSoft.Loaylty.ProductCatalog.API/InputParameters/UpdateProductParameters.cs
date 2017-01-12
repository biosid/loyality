namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class UpdateProductParameters : ProductParameters
    {
        [DataMember]
        [StringLength(256)]
        public string ProductId
        {
            get;
            set;
        }
    }
}
