namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    using API.Entities;
    using Configuration;
    using Extensions;
    using Interfaces;

    internal class OrdersRepository : BaseRepository, IOrdersRepository
    {
        public OrdersRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public OrdersRepository(string connectionString)
            : base(connectionString)
        {
        }

        public bool Exists(int? orderId, string externalOrderId = null)
        {
            using (var ctx = DbNewContext())
            {
                return orderId.HasValue
                           ? ctx.Orders.Any(o => o.Id == orderId.Value)
                           : !string.IsNullOrEmpty(externalOrderId) &&
                             ctx.Orders.Any(o => o.ExternalOrderId == externalOrderId);
            }
        }

        public Order Get(int partnerId, string externalOrderId)
        {
            using (var ctx = DbNewContext())
            {
                var retVal = ctx.Orders.SingleOrDefault(x => x.PartnerId == partnerId && x.ExternalOrderId == externalOrderId);
                return retVal;
            }
        }

        public IList<int> GetIds(OrderStatuses orderStatus, OrderPaymentStatuses orderPaymentStatus, PartnerType partnerType, int[] skipPartnerIds)
        {
            using (var ctx = DbNewContext())
            {
                var partners = ctx.Partners.Where(x => x.Type == partnerType);
                if (skipPartnerIds != null && skipPartnerIds.Length > 0)
                {
                    partners = partners.Where(p => !skipPartnerIds.Contains(p.Id));
                }

                var orders = ctx.Orders.Where(x => x.Status == orderStatus && x.PaymentStatus == orderPaymentStatus);
                var retVal =
                    orders.Join(partners, order => order.PartnerId, partner => partner.Id, (order, partner) => order.Id)
                          .ToList();
                return retVal;
            }
        }

        public void Delete(Order order)
        {
            using (var ctx = DbNewContext())
            {
                ctx.Entry(order).State = EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public Page<Order> GetForPayment(int countToSkip, int? countToTake = null)
        {
	        var dataSource = new OrdersDataSource();

            var statuses = new[]
                           {
                               OrderStatuses.DeliveryWaiting, OrderStatuses.Delivery, OrderStatuses.Delivered,
                               OrderStatuses.DeliveredWithDelay, OrderStatuses.NotDelivered
                           };

            using (var ctx = DbNewContext())
            {
                var query =
                    ctx.Orders
                       .Where(
                           t =>
                           (t.PaymentStatus == OrderPaymentStatuses.No || t.PaymentStatus == OrderPaymentStatuses.BankCancelled) &&
                           (statuses.Contains(t.Status) || (t.PartnerId == ConfigHelper.BankProductsPartnerId && t.Status == OrderStatuses.Processing)));

                query = query.OrderBy(t => t.Id).Skip(countToSkip);

                if (countToTake.HasValue)
                {
                    query = query.Take(countToTake.Value);
                }

                var tempIds = query.Select(x => x.Id).ToList();

	            var orders = tempIds.Select(x => dataSource.GetOrder(x)).ToList();

                var total = query.Count();
                var page = new Page<Order>(orders, countToSkip, countToTake, total);
                return page;
            }
        }

        public IList<OrderStatuses> GetNextOrderStatuses(OrderStatuses orderStatus)
        {
            using (var ctx = DbNewContext())
            {
                var retVal =
                    ctx.OrderStatusWorkFlowItems.Where(x => x.FromStatus == orderStatus)
                       .Select(x => x.ToStatus)
                       .ToList();

                return retVal;
            }
        }

        public IList<OrderStatusWorkFlowItem> GetOrderStatusWorkFlow(OrderStatuses fromOrderStatus)
        {
            var asArray = new[]
                          {
                              fromOrderStatus
                          };

            return GetOrderStatusWorkFlow(asArray);
        }

        public IList<OrderStatusWorkFlowItem> GetOrderStatusWorkFlow(IEnumerable<OrderStatuses> fromOrderStatuses)
        {
            using (var ctx = DbNewContext())
            {
                var retVal = ctx.OrderStatusWorkFlowItems.Where(x => fromOrderStatuses.Contains(x.FromStatus)).ToList();

                return retVal;
            }
        }

        public bool HasNonterminatedOrders(string clientId)
        {
            var nonterminatedStatus = new[]
                                      {
                                          OrderStatuses.Processing,
                                          OrderStatuses.DeliveryWaiting, 
                                          OrderStatuses.Delivery
                                      };

            using (var ctx = DbNewContext())
            {
                var retVal =
                    ctx.Orders.Where(x => x.ClientId == clientId)
                       .Any(x => nonterminatedStatus.Contains(x.Status));

                return retVal;
            }
        }

        public void UpdateOrderInternal(Order order)
        {
            using (var ctx = DbNewContext())
            {
                FillXmlFields(order);

                ctx.Entry(order).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public void UpdateOrderStatusDescription(int orderId, string statusDescription, string updatedUserId)
        {
            using (var ctx = DbNewContext())
            {
                var order = ctx.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.OrderStatusDescription = statusDescription;
                order.UpdatedDate = DateTime.Now;
                order.UpdatedUserId = updatedUserId;
                
                ctx.Entry(order).State = EntityState.Modified;
                ctx.SaveChanges();    
            }
        }

        public void UpdateDeliveryInstructions(int orderId, string deliveryInstructions, string updatedUserId)
        {
            using (var ctx = DbNewContext())
            {
                var order = ctx.Orders.FirstOrDefault(o => o.Id == orderId);

                if (order == null)
                {
                    throw new Exception("Order not found");
                }

                order.DeliveryInstructions = deliveryInstructions;
                order.UpdatedDate = DateTime.Now;
                order.UpdatedUserId = updatedUserId;

                ctx.Entry(order).State = EntityState.Modified;
                ctx.SaveChanges();
            }
        }

        public IList<Order> GetOrders(int[] ids)
        {
            using (var ctx = DbNewContext())
            {
                var query = ctx.Orders.Where(x => ids.Contains(x.Id)).Distinct();
                return query.ToList();
            }
        }

        public int[] GetOrdersWithAdvancePayments(int[] ids)
        {
            ids = ids.Distinct().ToArray();

            using (var ctx = DbNewContext())
            {
                var query = ctx.Orders.Where(o => ids.Contains(o.Id) && o.TotalAdvance != 0).Select(o => o.Id);

                return query.ToArray();
            }
        }

        public Order GetById(int id)
        {
            using (var ctx = DbNewContext())
            {
                return ctx.Orders.FirstOrDefault(o => o.Id == id);
            }
        }

        private void FillXmlFields(Order order)
        {
            if (order.Items == null)
            {
                order.Items = new OrderItem[]
                    {
                    };
            }

            order.ItemsXml = XmlSerializer.Serialize(order.Items);
            order.DeliveryInfoXml = XmlSerializer.Serialize(order.DeliveryInfo);
        }
    }
}