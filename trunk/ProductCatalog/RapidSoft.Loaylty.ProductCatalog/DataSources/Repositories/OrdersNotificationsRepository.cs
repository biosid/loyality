namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;

    using Entities;
    using Interfaces;

    internal class OrdersNotificationsRepository : BaseRepository, IOrdersNotificationsRepository
    {
        public int FillOrdersNotifications(string etlSessionId, int maxOrdersCount)
        {
            using (var context = DbNewContext())
            {
                var results = context.Database.SqlQuery<int>(
                    "exec prod.FillOrdersNotifications @etlSessionId, @maxOrdersCount",
                    new SqlParameter("@etlSessionId", etlSessionId),
                    new SqlParameter("@maxOrdersCount", maxOrdersCount));

                return results.Single();
            }
        }

        public OrderNotification[] GetOrdersNotifications(string etlSessionId)
        {
            using (var context = DbNewContext())
            {
                var items =
                    context.Database
                           .SqlQuery<OrderNotificationItem>(
                               "exec prod.GetItemsForOrdersNotifications @etlSessionId",
                               new SqlParameter("@etlSessionId", etlSessionId))
                           .ToArray();

                var notifications =
                    context.Database
                           .SqlQuery<OrderNotification>(
                               "exec prod.GetOrdersNotifications @etlSessionId",
                               new SqlParameter("@etlSessionId", etlSessionId))
                           .ToArray();

                var results = notifications.GroupJoin(
                    items,
                    n => n.OrderId,
                    i => i.OrderId,
                    SetOrderNotificationItems);

                return results.ToArray();
            }
        }

        public OrdersNotificationsEmail[] GetOrdersNotificationsEmails(string etlSessionId)
        {
            using (var context = DbNewContext())
            {
                var query = context.OrdersNotificationsEmails
                                   .AsNoTracking()
                                   .Where(e => e.EtlSessionId == etlSessionId);

                return query.ToArray();
            }
        }

        public void SaveEmails(IEnumerable<OrdersNotificationsEmail> emails)
        {
            using (var context = DbNewContext())
            {
                foreach (var email in emails)
                {
                    context.OrdersNotificationsEmails.Add(email);
                }

                context.SaveChanges();
            }
        }

        public void UpdateEmailStatus(int id, OrdersNotificationsEmailStatus status)
        {
            using (var context = DbNewContext())
            {
                var email = context.OrdersNotificationsEmails.FirstOrDefault(m => m.Id == id);
                if (email == null || email.Status == status)
                {
                    return;
                }

                email.Status = status;
                context.SaveChanges();
            }
        }

        private OrderNotification SetOrderNotificationItems(OrderNotification notification, IEnumerable<OrderNotificationItem> items)
        {
            notification.Items = items.ToArray();
            return notification;
        }
    }
}
