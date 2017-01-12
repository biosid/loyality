namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    /// <summary>
    /// ��������� ��������� ��������� �������.
    /// </summary>
    [DataContract]
    public class GetBasketItemResult : ResultBase
    {
        /// <summary>
        /// ������� �������.
        /// </summary>
        [DataMember]
        public BasketItem Item { get; set; }
    }
}