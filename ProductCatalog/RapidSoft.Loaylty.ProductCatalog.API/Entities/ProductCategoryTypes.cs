using System.Runtime.Serialization;

namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    public enum ProductCategoryTypes
    {
        [DataMember]
        Static = 0,
        [DataMember]
        Online = 1
    }
}