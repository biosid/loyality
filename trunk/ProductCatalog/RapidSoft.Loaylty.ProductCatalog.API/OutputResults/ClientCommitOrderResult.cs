using System.Runtime.Serialization;

namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Collections.Generic;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    [DataContract]
    public class ClientCommitOrderResult : ResultBase
    {
        [DataMember]
        public Order Order { get; set; }

        public static ClientCommitOrderResult BuildSuccess(Order order, int resultCode = ResultCodes.SUCCESS, string resultDescription = null)
        {
            return new ClientCommitOrderResult
                   {
                       Order = order,
                       ResultCode = resultCode,
                       ResultDescription = resultDescription,
                   };
        }

        public static ClientCommitOrderResult BuildFail(int code, string description)
        {
            return new ClientCommitOrderResult
                   {
                       ResultCode = code,
                       Order = null,
                       ResultDescription = description
                   };
        }
    }
}