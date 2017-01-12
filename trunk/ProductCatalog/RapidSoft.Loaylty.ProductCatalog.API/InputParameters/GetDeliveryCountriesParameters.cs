namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Объект-параметр операции получения списка стран, допустимых для указания адреса доставки по партнеру.
    /// </summary>
    [DataContract]
    public class GetDeliveryCountriesParameters
    {
        /// <summary>
        /// Идентификатор партнера.
        /// </summary>
        [DataMember]
        public int PartnerId { get; set; }
    }
}