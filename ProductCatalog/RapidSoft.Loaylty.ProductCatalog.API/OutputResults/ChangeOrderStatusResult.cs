using RapidSoft.Loaylty.ProductCatalog.API.Entities;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeOrderStatusResult : ResultBase
    {
        /// <summary>
        /// Идентификатор заказа в системе лояльности
        /// </summary>
        [DataMember]
        public int? OrderId { get; set; }

        [DataMember]
        public OrderStatuses? OriginalStatus { get; set; }

        public static ChangeOrderStatusResult BuildFail(int orderId, Exception ex)
        {
            return new ChangeOrderStatusResult
            {
                OrderId = orderId,
                ResultCode = ResultCodes.UNKNOWN_ERROR,
                ResultDescription = ex.ToString()
            };
        }
    }
}