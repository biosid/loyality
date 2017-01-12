namespace RapidSoft.Loaylty.ProductCatalog.OrdersNotifications.Templates
{
    using System;
    using System.Collections.Generic;
    using RapidSoft.Loaylty.ProductCatalog.Entities;

    /// <summary>
    /// Параметры для шаблона письма с нотификациями по заказам
    /// </summary>
    public partial class OrdersNotificationsEmailBody
    {
        public string PartnerName { get; set; }

        public IEnumerable<OrderNotification> OrdersNotifications { get; set; }
    }
}
