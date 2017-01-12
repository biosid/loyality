namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    using Entities;

    public class CreatePartnerProductCateroryLinkParameters
    {
        /// <summary>
        /// Идентификатор пользователя, выполняющего изменение.
        /// </summary>
        public string UserId { get; set; }

        public PartnerProductCategoryLink Link
        {
            get;
            set;
        }
    }
}