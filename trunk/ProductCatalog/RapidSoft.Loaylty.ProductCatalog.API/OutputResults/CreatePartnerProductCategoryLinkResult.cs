namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using Entities;

    public class CreatePartnerProductCategoryLinkResult : ResultBase
    {
        /// <summary>
        /// Созданная привязка категории рубрикатора к категории партнёра
        /// </summary>
        public PartnerProductCategoryLink Link { get; set; }

        public static new CreatePartnerProductCategoryLinkResult BuildFail(int resultCode, string errorMessage)
        {
            return new CreatePartnerProductCategoryLinkResult
                   {
                       Link = null,
                       ResultCode = resultCode,
                       ResultDescription = errorMessage
                   };
        }
    }
}