namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Статусы категорий.
    /// </summary>
    [DataContract]
    public enum ProductCategoryStatuses
    {
        [EnumMember]
        NotActive = 0,

        [EnumMember]
        Active = 1,
    }
}