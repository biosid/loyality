namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class UpdatePartnerResult : ResultBase
    {
        /// <summary>
        /// Идентификатор клиента создающего партнера.
        /// </summary>
        [DataMember]
        public Partner Partner
        {
            get;
            set;
        }
    }
}