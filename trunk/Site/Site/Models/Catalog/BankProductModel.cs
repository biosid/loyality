using System;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Services.BankProducts.Models;

namespace Vtb24.Site.Models.Catalog
{
    public class BankProductModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Thumbnail { get; set; }

        public DateTime ExpirationDate { get; set; }

        public BreadCrumbModel[] BreadCrumbs { get; set; }

        public decimal Price { get; set; }

        public decimal? PointsDeficit { get; set; }

        public decimal PointsProgress { get; set; }

        public bool CanRedeem { get; set; }

        public bool IsUserActivated { get; set; }

        public static BankProductModel Map(BankProduct original)
        {
            return new BankProductModel
            {
                Id = original.Id,
                Title = original.Description,
                ExpirationDate = original.ExpirationDate,
                Price = original.Cost
            };
        }
    }
}
