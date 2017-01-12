using System;

namespace RapidSoft.Loaylty.ProductCatalog.Entities
{
    [Obsolete("Делалось для прототипа, в настоящий момент не используется")]
    public class WishList
    {
        public Guid[] ProductIds
        {
            get;
            set;
        }

        public Guid[] OnlineProductIds
        {
            get;
            set;
        }

    }
}