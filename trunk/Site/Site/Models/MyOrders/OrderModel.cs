using System;
using System.Collections.Generic;
using System.Linq;
using Vtb24.Site.Models.Buy;
using Vtb24.Site.Models.MyOrders.Helpers;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;

namespace Vtb24.Site.Models.MyOrders
{
    public class OrderModel
    {
        public string PageTitle { get; set; }

        public string ExternalOrderId { get; set; }

        public MyOrderItemModel[] Items { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalItemsPrice { get; set; }

        public string Status { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public string StatusDescription { get; set; }

        public string RecipientName { get; set; }

        public string RecipientPhone { get; set; }

        public string RecipientEmail { get; set; }

        public string DeliveryAddress { get; set; }

        public string DeliveryPostCode { get; set; }

        public string DeliveryCity { get; set; }

        public string Comment { get; set; }

        public bool HasDelivery { get; set; }

        public bool HasAdvance { get; set; }

        public string DeliveryVariantName { get; set; }

        public string DeliveryVariantDescription { get; set; }

        public string DeliveryInstructions { get; set; }

        public bool IsPickup { get; set; }

        public bool IsEmail { get; set; }

        public PickupPointModel PickupPoint { get; set; }
        
        public static OrderModel Map(GiftShopOrder original)
        {
            var order = new OrderModel
            {
                PageTitle = string.Format("Заказ №{0} от {1:dd.MM.yyyy}", original.Id, original.CreateDate),

                ExternalOrderId = original.ExternalId,

                Items = original.Items.Select(MyOrderItemModel.Map).ToArray(),

                TotalPrice = original.TotalPrice,
                TotalItemsPrice = original.ItemsPrice,

                Status = OrderStatusTextHelper.GetStatusText(original.Status, original.PartnerId),
                StatusChangeDate = original.StatusChangeDate,
                StatusDescription = GetStatusDescription(original),

                HasDelivery = original.Delivery != null,
                HasAdvance = original.TotalAdvance != 0,

                DeliveryInstructions = original.Status == OrderStatus.Delivered ? original.DeliveryInstructions : null
            };

            if (original.Delivery != null)
            {
                var name = string.Format("{0} {1}", original.Delivery.Contact.FirstName, original.Delivery.Contact.LastName);

                order.RecipientEmail = original.Delivery.Contact.Email;
                order.RecipientPhone = PhoneFormatter.FormatPhoneNumber(original.Delivery.Contact.Phone);
                order.RecipientName = name;
                order.DeliveryVariantName = original.Delivery.DeliveryVariantName;
                order.DeliveryVariantDescription = original.Delivery.DeliveryVariantDescription;
                order.IsPickup = original.Delivery.Type == DeliveryType.Pickup;
                order.IsEmail = original.Delivery.Type == DeliveryType.Email;

                switch (original.Delivery.Type)
                {
                    case DeliveryType.Pickup:
                        {
                            order.PickupPoint = PickupPointModel.Map(original.Delivery.PickupPoint, 0, 0);
                        }
                        break;
                    case DeliveryType.Delivery:
                        {
                            order.DeliveryPostCode = original.Delivery.Address.PostCode;
                            order.DeliveryCity = new[]
                                {
                                    original.Delivery.Address.Town,
                                    original.Delivery.Address.City,
                                    original.Delivery.Address.Region
                                }
                                .FirstOrDefault(t => !string.IsNullOrEmpty(t));
                            order.DeliveryAddress = GetAddressString(original.Delivery.Address);
                        }
                        break;
                }

                order.Comment = original.Delivery.Comment;
            }

            return order;
        }

        private static string GetAddressString(DeliveryAddress address)
        {
            var data = new List<string>();

            if (!string.IsNullOrEmpty(address.Street))
            {
                data.Add(address.Street);
            }

            if (!string.IsNullOrEmpty(address.House))
            {
                data.Add(address.House);
            }

            if (!string.IsNullOrEmpty(address.Flat))
            {
                data.Add(string.Format("кв. {0}", address.Flat));
            }

            return string.Join(", ", data);
        }

        private static string GetStatusDescription(GiftShopOrder order)
        {
            return order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.NotDelivered
                       ? order.StatusDescription
                       : null;
        }
    }
}