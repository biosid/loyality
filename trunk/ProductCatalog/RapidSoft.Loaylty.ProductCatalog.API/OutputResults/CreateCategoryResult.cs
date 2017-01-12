using System.Runtime.Serialization;

using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    [DataContract]
    public class CreateCategoryResult : ResultBase
    {
        /// <summary>
        /// Добавленная категория
        /// </summary>
        [DataMember]
        public ProductCategory Category 
        {
            get;
            set;
        }
    }
}