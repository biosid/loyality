namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    using System.Runtime.Serialization;

    [DataContract]
    public class ChangeOrdersStatusesResult : ResultBase
    {
        [DataMember]
        public ChangeOrderStatusResult[] ChangeOrderStatusResults
        {
            get;
            set;
        }

        public static ChangeOrdersStatusesResult BuildSuccess(ChangeOrderStatusResult[] changeOrderStatusResults)
        {
            return new ChangeOrdersStatusesResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       ChangeOrderStatusResults = changeOrderStatusResults
                   };
        }
    }
}