namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    [DataContract]
    public class CreateProductParameters : ProductParameters
    {
        private string partnerProductId;
        
        /// <summary>
        ///  Артикул
        /// </summary>
        [DataMember]
        [StringLength(256)]
        public string PartnerProductId
        {
            get
            {
                return this.partnerProductId;
            }

            set
            {
                this.partnerProductId = value.Trim();
            }
        }
    }
}
