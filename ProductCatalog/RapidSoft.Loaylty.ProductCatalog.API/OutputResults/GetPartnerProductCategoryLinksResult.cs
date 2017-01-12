namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class GetPartnerProductCategoryLinksResult : ResultBase
    {
        public PartnerProductCategoryLink[] Links { get; set; }

        public static GetPartnerProductCategoryLinksResult BuildSuccess(IEnumerable<PartnerProductCategoryLink> links)
        {
            var arr = links.ToArray();
            return new GetPartnerProductCategoryLinksResult
                       {
                           ResultCode = ResultCodes.SUCCESS,
                           ResultDescription = null,
                           Links = arr
                       };
        } 
    }
}