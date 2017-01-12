namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces
{
    using System;
    using System.Collections.Generic;

    using Entities;

    public interface IOrdersNotificationsRepository
    {
        int FillOrdersNotifications(string etlSessionId, int maxOrdersCount);

        OrderNotification[] GetOrdersNotifications(string etlSessionId);

        OrdersNotificationsEmail[] GetOrdersNotificationsEmails(string etlSessionId);

        void SaveEmails(IEnumerable<OrdersNotificationsEmail> emails);

        void UpdateEmailStatus(int id, OrdersNotificationsEmailStatus status);
    }
}
