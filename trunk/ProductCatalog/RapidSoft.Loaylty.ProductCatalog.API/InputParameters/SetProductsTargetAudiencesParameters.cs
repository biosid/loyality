namespace RapidSoft.Loaylty.ProductCatalog.API.InputParameters
{
    public class SetProductsTargetAudiencesParameters
    {
        public string UserId
        {
            get;
            set;
        }

        public string[] ProductIds
        {
            get;
            set;
        }

        public string[] TargetAudienceIds
        {
            get;
            set;
        }
    }
}
