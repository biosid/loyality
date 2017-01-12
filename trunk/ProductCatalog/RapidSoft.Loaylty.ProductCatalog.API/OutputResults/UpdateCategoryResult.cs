using System.Runtime.Serialization;

using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{

    [DataContract]
    public class UpdateCategoryResult : ResultBase
    {
        /// <summary>
        /// Обновлённая категория
        /// </summary>
        [DataMember]
        public ProductCategory Category 
        {
            get;
            set;
        }
    }
}