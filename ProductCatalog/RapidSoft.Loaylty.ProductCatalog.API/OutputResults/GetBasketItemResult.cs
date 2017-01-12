namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// –езультат получени€ состо€ни€ корзины.
    /// </summary>
    [DataContract]
    public class GetBasketItemResult : ResultBase
    {
        /// <summary>
        /// Ёлемент корзины.
        /// </summary>
        [DataMember]
        public BasketItem Item { get; set; }
    }
}