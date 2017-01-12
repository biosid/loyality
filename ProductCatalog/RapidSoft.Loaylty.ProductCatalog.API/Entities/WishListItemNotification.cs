namespace RapidSoft.Loaylty.ProductCatalog.API.Entities
{
    using System;

    using RapidSoft.Extensions;

    public class WishListItemNotification
    {
        public WishListItemNotification()
        {
        }

        public WishListItemNotification(WishListItem wishListItem, DateTime createdDate)
        {
            wishListItem.ThrowIfNull("wishListItem");

            CreatedDate = createdDate;
            this.ClientId = wishListItem.ClientId;
            ProductId = wishListItem.ProductId;

            NotificationDate = null;
            ProductsQuantity = wishListItem.ProductsQuantity;
        }

        public string ClientId
        {
            get;
            set;
        }

        public string ProductId
        {
            get;
            set;
        }

        public DateTime CreatedDate
        {
            get;
            set;
        }

        public int ProductsQuantity
        {
            get;
            set;
        }

        public DateTime? NotificationDate
        {
            get;
            set;
        }

        public string LocationKladr
        {
            get;
            set;
        }

        public decimal ClientBalance
        {
            get;
            set;
        }

        public Product Product
        {
            get;
            set;
        }

        public decimal ItemBonusCost { get; set; }

        public decimal TotalBonusCost
        {
            get;
            set;
        }

        public string FirstName
        {
            get;
            set;
        }

        public string MiddleName
        {
            get;
            set;
        }
    }
}
