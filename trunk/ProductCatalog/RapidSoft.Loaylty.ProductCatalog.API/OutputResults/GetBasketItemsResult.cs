namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// ��������� ��������� ��������� �������.
    /// </summary>
    [DataContract]
    public class GetBasketItemsResult : ResultBase
    {
        /// <summary>
        /// �������� �������.
        /// </summary>
        [DataMember]
        public BasketItem[] Items { get; set; }
    }
}