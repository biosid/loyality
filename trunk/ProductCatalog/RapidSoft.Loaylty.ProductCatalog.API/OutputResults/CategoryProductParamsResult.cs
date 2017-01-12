namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class CategoryProductParamsResult : ResultBase
    {
        [DataMember]
        public ProductParamResult[] ProductParamResult
        {
            get;
            set;
        }
    }
}
