using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.Catalog.Models.Orders
{
    public class OrderEditModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        // ReSharper restore InconsistentNaming

        public int Id { get; set; }

        public string ExternalId { get; set; }

        public string SupplierName { get; set; }

        public string CarrierName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StatusChangeDate { get; set; }

        public OrderStatuses? OrderStatus { get; set; }

        [StringLength(256, ErrorMessage = "Превышена допустимая длина описания статуса (256 символов)")]
        public string OrderStatusDescription { get; set; }

        public SelectListItem[] OrderStatusesList { get; set; }

        public OrderPaymentStatuses? ProductPaymentStatus { get; set; }

        public OrderPaymentStatuses? DeliveryPaymentStatus { get; set; }

        public OrderItemModel[] Items { get; set; }

        public decimal ItemsPrice { get; set; }

        public decimal ItemsBonusPrice { get; set; }

        public decimal DeliveryPrice { get; set; }

        public decimal DeliveryBonusPrice { get; set; }

        public decimal DeliveryAdvance { get; set; }

        public decimal TotalAdvance { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalBonusPrice { get; set; }

        public ContactInfoModel ContactInfo { get; set; }

        public string Address { get; set; }

        public DeliveryInfoModel DeliveryInfo { get; set; }

        public StatusHistoryRecordModel[] StatusHistory { get; set; }

        public OrderEditPermissionsModel Permissions { get; set; }

        public bool HideRurPrices { get; set; }

        public bool OrderStatusFilter(OrderStatuses? status)
        {
            return status.HasValue &&
                   ((OrderStatus.HasValue && status.Value == OrderStatus.Value) ||
                    (status.Value != OrderStatuses.draft && status.Value != OrderStatuses.registration));
        }

        public OrdersQueryModel OrdersQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                           ? new OrdersQueryModel { issupplier = true }
                           : new OrdersQueryModel().MixQuery(query);
            }
        }

        public class ContactInfoModel
        {
            public string Email { get; set; }

            public string Phone { get; set; }

            public string FirstName { get; set; }

            public string MiddleName { get; set; }

            public string LastName { get; set; }

            public static ContactInfoModel Map(OrderDeliveryContact info)
            {
                if (info == null)
                {
                    return null;
                }

                return new ContactInfoModel
                {
                    Email = info.Email,
                    Phone = info.Phone,
                    FirstName = info.FirstName,
                    MiddleName = info.MiddleName,
                    LastName = info.LastName
                };
            }
        }

        public class DeliveryInfoModel
        {
            public string Date { get; set; }

            public string Time { get; set; }

            public string Comment { get; set; }

            public string AdditionalText { get; set; }

            public static DeliveryInfoModel Map(OrderDelivery info)
            {
                return info != null
                           ? new DeliveryInfoModel
                           {
                               Date = info.DeliveryDate.HasValue
                                          ? info.DeliveryDate.Value.ToShortDateString()
                                          : null,
                               Time = info.DeliveryTimeFrom.HasValue || info.DeliveryTimeTo.HasValue
                                          ? (info.DeliveryTimeFrom.HasValue
                                                 ? " с " + info.DeliveryTimeFrom.Value.ToString("h\\:mm")
                                                 : "") +
                                            (info.DeliveryTimeTo.HasValue
                                                 ? " до " + info.DeliveryTimeTo.Value.ToString("h\\:mm")
                                                 : "")
                                          : null,
                               Comment = info.Comment,
                               AdditionalText = info.AdditionalText
                           }
                           : null;
            }
        }
    }
}
