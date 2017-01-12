namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public class PartnerCommitOrdersResult : ResultBase
    {
        public ChangeExternalOrderStatusResult[] ChangeExternalOrderStatusResults { get; set; }

        public static PartnerCommitOrdersResult BuildSuccess(ChangeExternalOrderStatusResult[] changeExternalOrderStatusResults)
        {
            return new PartnerCommitOrdersResult
                   {
                       ResultCode = ResultCodes.SUCCESS,
                       Success = true,
                       ChangeExternalOrderStatusResults = changeExternalOrderStatusResults,
                       ResultDescription = null
                   };
        }

        public new static PartnerCommitOrdersResult BuildFail(int code, string message)
        {
            return new PartnerCommitOrdersResult
                   {
                       ResultCode = code,
                       Success = false,
                       ResultDescription = message,
                       ChangeExternalOrderStatusResults = null
                   };
        }
    }
}