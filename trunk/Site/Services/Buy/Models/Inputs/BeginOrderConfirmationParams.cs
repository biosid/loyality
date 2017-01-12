using System;
using Vtb24.Site.Services.GiftShop.Orders.Models.Inputs;

namespace Vtb24.Site.Services.Buy.Models.Inputs
{
    public class BeginOrderConfirmationParams
    {
        public Guid[] BasketItemIds { get; set; }

        public OrderDeliveryParameters Delivery { get; set; }

        public decimal TotalPrice { get; set; }

        public decimal TotalAdvance { get; set; }
    }
}