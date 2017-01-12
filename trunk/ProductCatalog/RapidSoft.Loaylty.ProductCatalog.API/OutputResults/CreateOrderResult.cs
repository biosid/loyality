namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class CreateOrderResult : ResultBase
    {
        /// <summary>
        /// Сформированный заказ
        /// </summary>
        [DataMember]
        public Order Order { get; set; }

        public static CreateOrderResult BuildSuccess(Order order)
        {
            return new CreateOrderResult
                   {
                       Order = order,
                       ResultCode = ResultCodes.SUCCESS,
                       Success = true,
                       ResultDescription = null,
                   };
        }

        public static CreateOrderResult BuildFail(Order order, int resultCode, string resultDescription)
        {
            return new CreateOrderResult
            {
                Order = order,
                ResultCode = resultCode,
                ResultDescription = resultDescription,
            };
        }
    }
}