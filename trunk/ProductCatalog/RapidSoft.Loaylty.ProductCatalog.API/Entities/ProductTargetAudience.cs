namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;

    public class ProductTargetAudience
    {
        public string ProductId
        {
            get;
            set;
        }

        public string TargetAudienceId
        {
            get;
            set;
        }

        public string InsertedUserId
        {
            get;
            set;
        }

        public DateTime InsertedDate
        {
            get;
            set;
        }
    }
}
