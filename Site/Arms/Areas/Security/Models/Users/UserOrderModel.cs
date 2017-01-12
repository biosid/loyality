using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.Security.Models.Users.Helpers;
using Vtb24.Site.Services.GiftShop.Orders.Models;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserOrderModel
    {
        public int Id { get; set; }

        public DateTime OrderTime { get; set; }

        public string Status { get; set; }

        public DateTime StatusChangeTime { get; set; }

        public string StatusDescription { get; set; }

        public UserOrderItemModel[] Items { get; set; }

        public decimal TotalPrice { get; set; }

        public string DeliveryString { get; set; }

        public bool HasDelivery { get; set; }

        public bool HasAdvance { get; set; }

        public string DeliveryVariantName { get; set; }

        public string DeliveryVariantDescription { get; set; }

        public bool IsPickup { get; set; }

        public bool IsEmail { get; set; }

        public string DeliveryAddress { get; set; }

        public DeliveryPickupPoint PickupPoint { get; set; }

        public string RecipientEmail { get; set; }

        public string RecipientPhone { get; set; }

        public string RecipientName { get; set; }

        public string Comment { get; set; }

        public static UserOrderModel Map(GiftShopOrder original)
        {
            var hasDelivery = original.Delivery != null;

            var model = new UserOrderModel
            {
                Id = original.Id,
                OrderTime = original.CreateDate,
                Status = original.Status.Map(),
                StatusChangeTime = original.StatusChangeDate,
                StatusDescription = original.StatusDescription,
                Items = original.Items.Select(UserOrderItemModel.Map).ToArray(),
                TotalPrice = original.TotalPrice,
                HasDelivery = hasDelivery,
                HasAdvance = original.TotalAdvance > 0,
                IsPickup = hasDelivery && original.Delivery.Type == DeliveryType.Pickup,
                IsEmail = hasDelivery && original.Delivery.Type == DeliveryType.Email,
                DeliveryString = UserDeliveryFormatter.Map(original.Delivery),
                DeliveryVariantName = hasDelivery ? original.Delivery.DeliveryVariantName : null,
                DeliveryVariantDescription = hasDelivery ? original.Delivery.DeliveryVariantDescription : null,
                DeliveryAddress = hasDelivery ? UserDeliveryFormatter.MapAddress(original.Delivery.Address) : null,
                PickupPoint = hasDelivery ? original.Delivery.PickupPoint : null
            };

            if (original.Delivery != null && original.Delivery.Contact != null)
            {
                model.RecipientEmail = original.Delivery.Contact.Email;
                model.RecipientPhone = original.Delivery.Contact.Phone;
                model.RecipientName = string.Join(" ", GetRecipientNameParts(original.Delivery.Contact));
                model.Comment = original.Delivery.Comment;
            }

            return model;
        }

        private static IEnumerable<string> GetRecipientNameParts(DeliveryContact original)
        {
            if (!string.IsNullOrEmpty(original.LastName))
                yield return original.LastName;

            if (!string.IsNullOrEmpty(original.FirstName))
                yield return original.FirstName;

            if (!string.IsNullOrEmpty(original.MiddleName))
                yield return original.MiddleName;
        }
    }
}
