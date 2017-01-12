namespace RapidSoft.Loaylty.ProductCatalog.API.OutputResults
{
    public static class ResultBaseExtensions
    {
        public static T BuildFailed<T>(this T resultVal, int code, string description) where T : ResultBase
        {
            resultVal.ResultCode = code;
            resultVal.ResultDescription = description;
            return resultVal;
        } 
    }
}