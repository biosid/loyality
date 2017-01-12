using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string SupplierProductId { get; set; }

        public int SupplierId { get; set; }

        public int CategoryId { get; set; }

        public string CategoryPath { get; set; }

        public ProductStatus Status { get; set; }

        public ProductModerationStatus ModerationStatus { get; set; }

        public bool IsRecommended { get; set; }

        public decimal PriceRUR { get; set; }

        public string[] Segments { get; set; }

        public string[] Pictures { get; set; }

        public string Description { get; set; }

        public string Vendor { get; set; }

        public int? Weight { get; set; }

		public bool IsDeliveredByEmail { get; set; }

        public decimal? BasePriceRUR { get; set; }

        public DateTime? BasePriceRurDate { get; set; }

        public Parameter[] Parameters { get; set; }

        public class Parameter
        {
            public string Name { get; set; }

            public string Value { get; set; }

            public string Unit { get; set; }
        }
    }
}
