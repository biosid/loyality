namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;

    public class PartnerSettingsResult : ResultBase
    {
        public Dictionary<string, string> Settings { get; set; }

        public static PartnerSettingsResult BuildSuccess(Dictionary<string, string> settings)
        {
            return new PartnerSettingsResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Success = true,
                       Settings = settings
                   };
        }
    }
}