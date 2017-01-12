namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class GetOrderResult : ResultBase
    {
        [DataMember]
        public Order Order { get; set; }

        [DataMember]
        public IList<OrderStatuses> NextOrderStatuses { get; set; }

        public static GetOrderResult BuildSuccess(Order order, IList<OrderStatuses> flow)
        {
            return new GetOrderResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ResultDescription = null,
                       Success = true,
                       Order = order,
                       NextOrderStatuses = flow
                   };
        }

        public static new GetOrderResult BuildFail(int code, string description)
        {
            return new GetOrderResult
            {
                ResultCode = code,
                ResultDescription = description,
            };
        }
    }
}