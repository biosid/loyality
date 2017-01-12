namespace RapidSoft.Loaylty.ProductCatalog.DataSources.Repositories
{
    using System.Data;
    using System.Data.SqlClient;

    using API.Entities;

    using Extensions;

    using Interfaces;

    internal class OrdersHistoryRepository : BaseRepository, IOrdersHistoryRepository
    {
        public OrdersHistoryRepository()
            : base(DataSourceConfig.ConnectionString)
        {
        }

        public OrderHistoryPage GetOrderHistory(
            int orderId,
            int countToSkip,
            int countToTake,
            bool calcTotalCount)
        {
            using (var conn = new SqlConnection(DataSourceConfig.ConnectionString))
            {
                conn.Open();
                using (var comm = conn.CreateCommand())
                {
                    comm.CommandType = CommandType.StoredProcedure;
                    comm.CommandText = "prod.GetOrderHistory";

                    comm.Parameters.AddWithValue("Id", orderId);
                    comm.Parameters.AddWithValue("countToTake", countToTake);
                    comm.Parameters.AddWithValue("countToSkip", countToSkip);
                    comm.Parameters.AddWithValue("calcTotalCount", calcTotalCount);

                    var totalCountParam = new SqlParameter("totalCount", SqlDbType.Int)
                                              {
                                                  Direction = ParameterDirection.Output,
                                                  IsNullable = true
                                              };
                    comm.Parameters.Add(totalCountParam);

                    var retVal = new OrderHistoryPage();
                    using (var reader = comm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var entity = new OrderHistory();
                            entity.Id = reader.GetInt32("Id");

                            entity.UpdatedUserId = reader.GetString("UpdatedUserId");
                            entity.UpdatedDate = reader.GetDateTime("TriggerDate");

                            var newStatusInt = reader.GetInt32OrNull("NewStatus");
                            entity.NewStatus = newStatusInt.HasValue ? (OrderStatuses?)newStatusInt : null;
                            var oldStatusInt = reader.GetInt32OrNull("OldStatus");
                            entity.OldStatus = oldStatusInt.HasValue ? (OrderStatuses?)oldStatusInt : null;

                            entity.NewOrderStatusDescription = reader.GetStringOrNull("NewOrderStatusDescription");
                            entity.OldOrderStatusDescription = reader.GetStringOrNull("OldOrderStatusDescription");

                            var newPaymentStatusInt = reader.GetInt32OrNull("NewPaymentStatus");
                            entity.NewOrderPaymentStatus = newPaymentStatusInt.HasValue ? (OrderPaymentStatuses?)newPaymentStatusInt : null;
                            var oldPaymentStatusInt = reader.GetInt32OrNull("OldPaymentStatus");
                            entity.OldOrderPaymentStatus = oldPaymentStatusInt.HasValue ? (OrderPaymentStatuses?)oldPaymentStatusInt : null;

                            var newDeliveryStatusInt = reader.GetInt32OrNull("NewDeliveryStatus");
                            entity.NewDeliveryPaymentStatus = newDeliveryStatusInt.HasValue ? (OrderDeliveryPaymentStatus?)newDeliveryStatusInt : null;
                            var oldDeliveryStatusInt = reader.GetInt32OrNull("OldDeliveryStatus");
                            entity.OldDeliveryPaymentStatus = oldDeliveryStatusInt.HasValue ? (OrderDeliveryPaymentStatus?)oldDeliveryStatusInt : null;

                            retVal.Add(entity);
                        }
                    }

                    retVal.TotalCount = totalCountParam.Value as int?;

                    return retVal;
                }
            }
        }
    }
}