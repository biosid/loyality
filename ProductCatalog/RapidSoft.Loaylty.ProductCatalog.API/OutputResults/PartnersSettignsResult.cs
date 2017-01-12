namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    public class PartnersSettignsResult : ResultBase
    {
        public PartnerSettings[] Settings { get; set; }

        public static PartnersSettignsResult BuildSuccess(IEnumerable<PartnerSettings> settings)
        {
            var asArray = settings.ToArray();
            return new PartnersSettignsResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Success = true,
                       Settings = asArray
                   };
        }
    }
}