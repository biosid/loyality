namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetPartnerInfoByIdResult : ResultBase
    {
        public PartnerInfo PartnerInfo { get; set; }

        public static GetPartnerInfoByIdResult BuildSuccess(PartnerInfo partnerInfo)
        {
            return new GetPartnerInfoByIdResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Success = true,
                           PartnerInfo = partnerInfo
                       };
        }
    }
}