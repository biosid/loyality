namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetPartnersInfoResult : ResultBase
    {
        public PartnerInfo[] PartnersInfo { get; set; }

        public static GetPartnersInfoResult BuildSuccess(IEnumerable<PartnerInfo> partnersInfo)
        {
            var asArray = partnersInfo.ToArray();
            return new GetPartnersInfoResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true,
                           PartnersInfo = asArray
                       };
        }
    }
}