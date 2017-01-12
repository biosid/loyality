namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;

    using Extensions;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;
    using RapidSoft.Loaylty.ProductCatalog.API.InputParameters;
    using RapidSoft.Loaylty.ProductCatalog.API.OutputResults;
    using RapidSoft.Loaylty.ProductCatalog.DataSources.Interfaces;
    using RapidSoft.Loaylty.ProductCatalog.Settings;

    public class OrdersDataSource : IOrdersDataSource
    {
        static OrdersDataSource()
        {
            AutoMapper.Mapper.CreateMap<DeliveryInfo, DeliveryAddress>();
        }

        public int Insert(Order order)
        {
            order.ThrowIfNull("order");
            order.Items.ThrowIfNull("order.Items");
            if (order.Items.Length == 0)
            {
                throw new ArgumentException("order.Items.Length is zero");
            }

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();

                using (var sqlCmd = new SqlCommand("[prod].[InsertOrder]", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("ClientId", order.ClientId);
                    sqlCmd.Parameters.AddWithValue("ExternalOrderId", order.ExternalOrderId);
                    sqlCmd.Parameters.AddWithValue("PartnerId", order.PartnerId);
                    sqlCmd.Parameters.AddWithValue("Status", (int)order.Status);
                    sqlCmd.Parameters.AddWithValue("PaymentStatus", (int)order.PaymentStatus);
                    sqlCmd.Parameters.AddWithValue("DeliveryPaymentStatus", (int)order.DeliveryPaymentStatus);
                    sqlCmd.Parameters.AddWithValue("ExternalOrderStatusCode", order.ExternalOrderStatusCode);
                    sqlCmd.Parameters.AddWithValue("OrderStatusDescription", order.OrderStatusDescription);
                    sqlCmd.Parameters.AddWithValue("ExternalOrderStatusDateTime", order.ExternalOrderStatusDateTime);
                    sqlCmd.Parameters.AddWithValue("DeliveryInfo", XmlSerializer.Serialize(order.DeliveryInfo));
                    sqlCmd.Parameters.AddWithValue("UpdatedUserId", order.UpdatedUserId ?? ApiSettings.ClientSiteUserName);
                    sqlCmd.Parameters.AddWithValue("Items", XmlSerializer.Serialize(order.Items));
                    sqlCmd.Parameters.AddWithValue("TotalWeight", order.TotalWeight);
                    sqlCmd.Parameters.AddWithValue("ItemsCost", order.ItemsCost);
                    sqlCmd.Parameters.AddWithValue("BonusItemsCost", order.BonusItemsCost);
                    sqlCmd.Parameters.AddWithValue("DeliveryCost", order.DeliveryCost);
                    sqlCmd.Parameters.AddWithValue("BonusDeliveryCost", order.BonusDeliveryCost);
                    sqlCmd.Parameters.AddWithValue("DeliveryAdvance", order.DeliveryAdvance);
                    sqlCmd.Parameters.AddWithValue("ItemsAdvance", order.ItemsAdvance);
                    sqlCmd.Parameters.AddWithValue("TotalAdvance", order.TotalAdvance);
                    sqlCmd.Parameters.AddWithValue("TotalCost", order.TotalCost);
                    sqlCmd.Parameters.AddWithValue("BonusTotalCost", order.BonusTotalCost);
                    sqlCmd.Parameters.AddWithValue("CarrierId", order.CarrierId);

                    var executeScalar = sqlCmd.ExecuteScalar();
                    return int.Parse(executeScalar.ToString());
                }
            }
        }

        public ChangeOrderStatusResult[] UpdateOrdersStatuses(OrdersStatus[] orderStatuseses, string updatedUserId)
        {
            var wrongStatus =
                orderStatuseses.FirstOrDefault(s => !s.OrderId.HasValue);

            if (wrongStatus != null)
            {
                throw new InvalidOperationException("Статус заказа должен содержать OrderId");
            }

            var res = new List<ChangeOrderStatusResult>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(updatedUserId);

                foreach (var orderStatus in orderStatuseses)
                {
                    try
                    {
                        var changeOrderStatusResult = UpdateOrderStatus(orderStatus, updatedUserId, conn);
                        res.Add(changeOrderStatusResult);
                    }
                    catch (Exception ex)
                    {
                        var result = new ChangeOrderStatusResult
                        {
                            OrderId = orderStatus.OrderId,
                            ResultCode = ResultCodes.INVALID_PARAMETER_VALUE,
                            ResultDescription = ex.ToString()
                        };
                        res.Add(result);
                    }
                }
            }

            return res.ToArray();
        }

        public ChangeOrderStatusResult[] UpdateOrdersPaymentStatuses(OrdersPaymentStatus[] statuses, string updatedUserId)
        {
            var res = new List<ChangeOrderStatusResult>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(updatedUserId);

                foreach (var t in statuses)
                {
                    try
                    {
                        var changeOrderStatusResult = UpdateOrderPaymentStatus(t, conn, updatedUserId);
                        res.Add(changeOrderStatusResult);
                    }
                    catch (Exception ex)
                    {
                        var result = ChangeOrderStatusResult.BuildFail(t.OrderId, ex);
                        res.Add(result);
                    }
                }
            }

            return res.ToArray();
        }

        public ChangeOrderStatusResult[] UpdateOrdersDeliveryStatuses(OrdersDeliveryStatus[] statuses, string updatedUserId)
        {
            var res = new List<ChangeOrderStatusResult>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(updatedUserId);

                foreach (var t in statuses)
                {
                    try
                    {
                        var changeOrderStatusResult = UpdateOrderDeliveryStatus(t, conn, updatedUserId);
                        res.Add(changeOrderStatusResult);
                    }
                    catch (Exception ex)
                    {
                        var result = ChangeOrderStatusResult.BuildFail(t.OrderId, ex);
                        res.Add(result);
                    }
                }
            }

            return res.ToArray();
        }

        public ChangeExternalOrderStatusResult[] UpdateExternalOrdersStatuses(ExternalOrdersStatus[] orderStatuseses, string updatedUserId)
        {
            var wrongStatus =
                orderStatuseses.FirstOrDefault(
                    s => !s.OrderId.HasValue && (!s.PartnerId.HasValue || string.IsNullOrEmpty(s.ExternalOrderId)));

            if (wrongStatus != null)
            {
                throw new InvalidOperationException(
                    "Статус заказа должен содержать либо OrderId либо PartnerId и ExternalOrderId");
            }

            var res = new List<ChangeExternalOrderStatusResult>();

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                conn.SetUserContext(updatedUserId);

                foreach (var orderStatus in orderStatuseses)
                {
                    try
                    {
                        var changeOrderStatusResult = UpdateExternalOrderStatus(orderStatus, updatedUserId, conn);
                        if (!orderStatus.OrderId.HasValue)
                        {
                            orderStatus.OrderId = changeOrderStatusResult.OrderId;
                        }
                        res.Add(changeOrderStatusResult);
                    }
                    catch (Exception ex)
                    {
                        var result = new ChangeExternalOrderStatusResult
                                     {
                                         OrderId = orderStatus.OrderId,
                                         ExternalOrderId = orderStatus.ExternalOrderId,
                                         ResultCode = ResultCodes.UNKNOWN_ERROR,
                                         ResultDescription = ex.ToString()
                                     };
                        res.Add(result);
                    }
                }
            }

            return res.ToArray();
        }

        public Order GetOrder(int orderId, string clientId = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var sqlCmd = new SqlCommand("prod.GetOrder", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("Id", orderId);

                    if (!string.IsNullOrEmpty(clientId))
                    {
                        sqlCmd.Parameters.AddWithValue("ClientId", clientId);
                    }

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        return reader.Read() ? MapOrder(reader) : null;
                    }
                }
            }
        }

        public Order GetOrderByExternalId(string externalId, string clientId = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var sqlCmd = new SqlCommand("prod.GetOrderByExternalId", conn))
                {
                    sqlCmd.CommandType = CommandType.StoredProcedure;

                    sqlCmd.Parameters.AddWithValue("ExternalOrderId", externalId);

                    if (!string.IsNullOrEmpty(clientId))
                    {
                        sqlCmd.Parameters.AddWithValue("ClientId", clientId);
                    }

                    using (var reader = sqlCmd.ExecuteReader())
                    {
                        return reader.Read() ? MapOrder(reader) : null;
                    }
                }
            }
        }

        public Page<Order> GetOrders(
            string clientId,
            DateTime? startDate,
            DateTime? endDate,
            OrderStatuses[] statuses,
            OrderStatuses[] skipStatuses,
            int countToSkip,
            int countToTake,
            bool calcTotalCount)
        {
            if (string.IsNullOrEmpty(clientId))
            {
                return new Page<Order>(countToSkip, countToTake);
            }

            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "prod.GetOrders";

                    comm.Parameters.AddWithValue("clientId", clientId);
                    comm.Parameters.AddWithValue("dateTimeStart", startDate);
                    comm.Parameters.AddWithValue("dateTimeEnd", endDate);

                    comm.Parameters.AddWithValue("statuses", statuses != null ? string.Join(",", statuses.Select(x => (int)x)) : null);
                    comm.Parameters.AddWithValue("skipStatuses", skipStatuses != null ? string.Join(",", skipStatuses.Select(x => (int)x)) : null);

                    comm.Parameters.AddWithValue("countToTake", countToTake);
                    comm.Parameters.AddWithValue("countToSkip", countToSkip);
                    comm.Parameters.AddWithValue("calcTotalCount", calcTotalCount);

                    var totalCountParam = new SqlParameter("totalCount", SqlDbType.Int);
                    totalCountParam.Direction = ParameterDirection.Output;
                    totalCountParam.IsNullable = true;
                    comm.Parameters.Add(totalCountParam);

                    var retVal = new List<Order>();

                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = MapOrder(reader);

                            retVal.Add(entity);
                        }
                    }

                    var totalCount = totalCountParam.Value as int?;

                    return new Page<Order>(retVal, countToSkip, countToTake, totalCount);
                }
            }
        }

        public LastDeliveryAddress[] GetLastDeliveryAddresses(string clientId, bool excludeAddressesWithoutKladr, int? countToTake = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "[prod].[GetLastDeliveryAddresses]";

                    comm.Parameters.AddWithValue("clientId", clientId);
                    comm.Parameters.AddWithValue("excludeAddressesWithoutKladr", excludeAddressesWithoutKladr);
                    comm.Parameters.AddWithValue("countToTake", countToTake ?? 10);

                    using (var reader = comm.ExecuteReader())
                    {
                        var addresses = ReadDeliveryInfo(reader).Select(info => new LastDeliveryAddress
                        {
                            DeliveryVariantsLocation = info.DeliveryVariantsLocation,
                            DeliveryAddress = info.Address
                        }).ToArray();

                        return addresses;
                    }
                }
            }
        }

        public Page<Order> SearchOrders(
            DateTime startDate,
            DateTime endDate,
            OrderStatuses[] statuses,
            OrderStatuses[] skipStatuses,
            OrderPaymentStatuses[] paymentStatuses,
            OrderDeliveryPaymentStatus[] deliveryPaymentStatus,
            int[] partnerIds,
            int countToSkip,
            int countToTake,
            bool calcTotalCount,
            int[] orderIds = null,
            int[] carrierIds = null)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "prod.SearchOrders";

                    comm.Parameters.AddWithValue("dateTimeStart", startDate);
                    comm.Parameters.AddWithValue("dateTimeEnd", endDate);
                    if (statuses != null && statuses.Length > 0)
                    {
                        comm.Parameters.AddWithValue("statuses", string.Join(",", statuses.Select(x => (int)x)));
                    }

                    if (skipStatuses != null && skipStatuses.Length > 0)
                    {
                        comm.Parameters.AddWithValue("skipStatuses", string.Join(",", skipStatuses.Select(x => (int)x)));
                    }

                    if (paymentStatuses != null && paymentStatuses.Length > 0)
                    {
                        comm.Parameters.AddWithValue("paymentStatus", string.Join(",", paymentStatuses.Select(x => (int)x)));
                    }

                    if (deliveryPaymentStatus != null && deliveryPaymentStatus.Length > 0)
                    {
                        comm.Parameters.AddWithValue("deliveryPaymentStatus", string.Join(",", deliveryPaymentStatus.Select(x => (int)x)));
                    }

                    if (partnerIds != null && partnerIds.Length > 0)
                    {
                        comm.Parameters.AddWithValue("partnerIds", string.Join(",", partnerIds.Select(x => x)));
                    }

                    if (orderIds != null && orderIds.Length > 0)
                    {
                        comm.Parameters.AddWithValue("orderIds", string.Join(",", orderIds));
                    }

                    if (carrierIds != null && carrierIds.Length > 0)
                    {
                        comm.Parameters.AddWithValue("carrierIds", string.Join(",", carrierIds));    
                    }

                    comm.Parameters.AddWithValue("countToTake", countToTake);
                    comm.Parameters.AddWithValue("countToSkip", countToSkip);
                    comm.Parameters.AddWithValue("calcTotalCount", calcTotalCount);

                    var totalCountParam = new SqlParameter("totalCount", SqlDbType.Int);
                    totalCountParam.Direction = ParameterDirection.Output;
                    totalCountParam.IsNullable = true;
                    comm.Parameters.Add(totalCountParam);

                    var retVal = new List<Order>();

                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = MapOrder(reader);

                            retVal.Add(entity);
                        }
                    }

                    var totalCount = totalCountParam.Value as int?;

                    return new Page<Order>(retVal, countToSkip, countToTake, totalCount);
                }
            }
        }

        private static IEnumerable<DeliveryInfo> ReadDeliveryInfo(SqlDataReader reader)
        {
            while (reader.Read())
            {
                var deliveryInfoXml = reader["DeliveryInfo"];

                var deliveryInfo = XmlSerializer.Deserialize<DeliveryInfo>(deliveryInfoXml as string);

                yield return deliveryInfo;
            }
        }

        private static Order MapOrder(SqlDataReader reader)
        {
            var entity = new Order();

            entity.Id = reader.GetInt32("Id");
            entity.ClientId = reader.GetString("ClientId");
            entity.ExternalOrderId = reader.GetStringOrNull("ExternalOrderId");
            entity.Status = reader.GetEnum<OrderStatuses>("Status");

            entity.PaymentStatus = reader.GetEnum<OrderPaymentStatuses>("PaymentStatus");
            entity.DeliveryPaymentStatus = reader.GetEnum<OrderDeliveryPaymentStatus>("DeliveryPaymentStatus");

            entity.ExternalOrderStatusCode = reader.GetStringOrNull("ExternalOrderStatusCode");
            entity.OrderStatusDescription = reader.GetStringOrNull("OrderStatusDescription");
            entity.ExternalOrderStatusDateTime = reader.GetDateTimeOrNull("ExternalOrderStatusDateTime");

            entity.StatusChangedDate = reader.GetDateTime("StatusChangedDate");
            entity.DeliveryInfo = XmlSerializer.Deserialize<DeliveryInfo>(reader.GetStringOrNull("DeliveryInfo"));
            entity.InsertedDate = reader.GetDateTime("InsertedDate");
            entity.Items = XmlSerializer.Deserialize<OrderItem[]>(reader.GetString("Items"));
            entity.TotalWeight = reader.GetInt32("TotalWeight");
            entity.ItemsCost = reader.GetDecimal("ItemsCost");
            entity.BonusItemsCost = reader.GetDecimal("BonusItemsCost");
            entity.DeliveryCost = reader.GetDecimal("DeliveryCost");
            entity.BonusDeliveryCost = reader.GetDecimal("BonusDeliveryCost");
            entity.DeliveryAdvance = reader.GetDecimal("DeliveryAdvance");
            entity.ItemsAdvance = reader.GetDecimal("ItemsAdvance");
            entity.TotalAdvance = reader.GetDecimal("TotalAdvance");
            entity.TotalCost = reader.GetDecimal("TotalCost");
            entity.BonusTotalCost = reader.GetDecimal("BonusTotalCost");

            entity.PartnerId = reader.GetInt32("PartnerId");
            entity.CarrierId = reader.GetInt32OrNull("CarrierId");
            entity.PublicStatus = entity.GetClientOrderStatus();

            entity.DeliveryInstructions = reader.GetStringOrNull("DeliveryInstructions");

            return entity;
        }

        private ChangeExternalOrderStatusResult UpdateExternalOrderStatus(ExternalOrdersStatus orderStatus, string updatedUserId, SqlConnection conn)
        {
            using (var sqlCmd = new SqlCommand("[prod].[UpdateExternalOrderStatus]", conn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.AddParameter("OrderId", orderStatus.OrderId);
                sqlCmd.AddParameter("ExternalOrderId", orderStatus.ExternalOrderId);
                sqlCmd.AddParameter("PartnerId", orderStatus.PartnerId);
                sqlCmd.AddParameter("ClientId", orderStatus.ClientId);
                if (orderStatus.OrderStatus.HasValue)
                {
                    sqlCmd.AddParameter("Status", (int)orderStatus.OrderStatus.Value);
                }
                sqlCmd.AddParameter("OrderStatusDescription", orderStatus.OrderStatusDescription);
                sqlCmd.AddParameter("ExternalOrderStatusCode", orderStatus.ExternalOrderStatusCode);
                sqlCmd.AddParameter("ExternalOrderStatusDateTime", orderStatus.ExternalOrderStatusDateTime);
                sqlCmd.AddParameter("UpdatedUserId", updatedUserId);

                var outOrderId = sqlCmd.AddOutParameter("OutOrderId", SqlDbType.Int);
                var originalStatus = sqlCmd.AddOutParameter("OriginalStatus", SqlDbType.Int);
                var orderNotFound = sqlCmd.AddOutParameter("OrderNotFound", SqlDbType.Bit);
                var wrongWorkflow = sqlCmd.AddOutParameter("WrongWorkflow", SqlDbType.Bit);

                sqlCmd.ExecuteNonQuery();

                int resultCode;

                if (Convert.ToBoolean(orderNotFound.Value))
                {
                    resultCode = ResultCodes.NOT_FOUND;
                }
                else if (Convert.ToBoolean(wrongWorkflow.Value))
                {
                    resultCode = ResultCodes.WRONG_WORKFLOW;
                }
                else
                {
                    resultCode = ResultCodes.SUCCESS;
                }

                var outOrderIdValue = outOrderId.Value != DBNull.Value
                                          ? Convert.ToInt32(outOrderId.Value)
                                          : (int?)null;

                var originalStatusValue = originalStatus.Value != DBNull.Value
                                              ? (OrderStatuses?)Convert.ToInt32(originalStatus.Value)
                                              : null;

                return new ChangeExternalOrderStatusResult
                {
                    OrderId = outOrderIdValue,
                    OriginalStatus = originalStatusValue,
                    ExternalOrderId = orderStatus.ExternalOrderId,
                    ResultCode = resultCode
                };
            }
        }

        private ChangeOrderStatusResult UpdateOrderStatus(OrdersStatus orderStatus, string updatedUserId, SqlConnection conn)
        {
            using (var sqlCmd = new SqlCommand("[prod].[UpdateOrderStatus]", conn))
            {
                conn.SetUserContext(updatedUserId);

                sqlCmd.CommandType = CommandType.StoredProcedure;

                sqlCmd.AddParameter("OrderId", orderStatus.OrderId);
                sqlCmd.AddParameter("ClientId", orderStatus.ClientId);
                if (orderStatus.OrderStatus.HasValue)
                {
                    sqlCmd.AddParameter("Status", (int)orderStatus.OrderStatus.Value);
                }
                sqlCmd.AddParameter("OrderStatusDescription", orderStatus.OrderStatusDescription ?? string.Empty);
                sqlCmd.AddParameter("UpdatedUserId", updatedUserId);

                var originalStatus = sqlCmd.AddOutParameter("OriginalStatus", SqlDbType.Int);
                var orderNotFound = sqlCmd.AddOutParameter("OrderNotFound", SqlDbType.Bit);
                var wrongWorkflow = sqlCmd.AddOutParameter("WrongWorkflow", SqlDbType.Bit);

                sqlCmd.ExecuteNonQuery();

                int resultCode;

                if (Convert.ToBoolean(orderNotFound.Value))
                {
                    resultCode = ResultCodes.NOT_FOUND;
                }
                else if (Convert.ToBoolean(wrongWorkflow.Value))
                {
                    resultCode = ResultCodes.WRONG_WORKFLOW;
                }
                else
                {
                    resultCode = ResultCodes.SUCCESS;
                }

                var originalStatusValue = originalStatus.Value != DBNull.Value
                                              ? (OrderStatuses?)Convert.ToInt32(originalStatus.Value)
                                              : null;

                return new ChangeOrderStatusResult
                {
                    OrderId = orderStatus.OrderId,
                    OriginalStatus = originalStatusValue,
                    ResultCode = resultCode
                };
            }
        }

        private ChangeOrderStatusResult UpdateOrderPaymentStatus(OrdersPaymentStatus status, SqlConnection conn, string updatedUserId)
        {
            SqlParameter returnParam;
            using (var sqlCmd = new SqlCommand("[prod].[UpdateOrderPaymentStatus]", conn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("OrderId", status.OrderId);
                sqlCmd.Parameters.AddWithValue("PaymentStatus", (int)status.PaymentStatus);
                sqlCmd.Parameters.AddWithValue("ClientId", status.ClientId);
                sqlCmd.Parameters.AddWithValue("UpdatedUserId", updatedUserId);
                returnParam = new SqlParameter("MissingOrderId", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.Output;
                sqlCmd.Parameters.Add(returnParam);
                sqlCmd.ExecuteNonQuery();
            }

            return new ChangeOrderStatusResult
            {
                OrderId = status.OrderId,
                ResultCode = returnParam.Value == DBNull.Value ? ResultCodes.SUCCESS : ResultCodes.NOT_FOUND
            };
        }

        private ChangeOrderStatusResult UpdateOrderDeliveryStatus(OrdersDeliveryStatus status, SqlConnection conn, string updatedUserId)
        {
            SqlParameter returnParam;
            using (var sqlCmd = new SqlCommand("[prod].[UpdateOrderDeliveryStatus]", conn))
            {
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("OrderId", status.OrderId);
                sqlCmd.Parameters.AddWithValue("DeliveryStatus", (int)status.DeliveryStatus);
                sqlCmd.Parameters.AddWithValue("ClientId", status.ClientId);
                sqlCmd.Parameters.AddWithValue("UpdatedUserId", updatedUserId);
                returnParam = new SqlParameter("MissingOrderId", SqlDbType.Int);
                returnParam.Direction = ParameterDirection.Output;
                sqlCmd.Parameters.Add(returnParam);
                sqlCmd.ExecuteNonQuery();
            }

            return new ChangeOrderStatusResult
            {
                OrderId = status.OrderId,
                ResultCode = returnParam.Value == DBNull.Value ? ResultCodes.SUCCESS : ResultCodes.NOT_FOUND
            };
        }
    }
}