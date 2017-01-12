namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using Entities;

    [DataContract]
    public class GetPartnerResult : ResultBase
    {
        /// <summary>
        /// Идентификатор клиента создающего партнера.
        /// </summary>
        [DataMember]
        public Partner[] Partners { get; set; }

        public static GetPartnerResult BuildSuccess(IEnumerable<Partner> partners)
        {
            var asArray = partners.ToArray();
            return new GetPartnerResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true,
                           Partners = asArray
                       };
        }
    }
}