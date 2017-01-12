using System.Collections.Generic;
using System.Net.Http;
using RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Models;

namespace RapidSoft.VTB24.BankConnector.Acquiring.Uniteller.Infrastructure
{
    internal static class MappingsToService
    {
        public static FormUrlEncodedContent ToFormUrlEncodedContent(GetAuthorizationResultsRequest original)
        {
            return new FormUrlEncodedContent(ToKeyValuePairs(original));
        }

        public static FormUrlEncodedContent ToFormUrlEncodedContent(ConfirmPaymentRequest original)
        {
            return new FormUrlEncodedContent(ToKeyValuePairs(original));
        }

        public static FormUrlEncodedContent ToFormUrlEncodedContent(CancelPaymentRequest original)
        {
            return new FormUrlEncodedContent(ToKeyValuePairs(original));
        }

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(GetAuthorizationResultsRequest original)
        {
            yield return new KeyValuePair<string, string>("Shop_ID", original.ShopId);
            yield return new KeyValuePair<string, string>("Login", original.Login);
            yield return new KeyValuePair<string, string>("Password", original.Password);
            yield return new KeyValuePair<string, string>("Format", ((int)original.Format).ToString("D"));
            yield return new KeyValuePair<string, string>("ShopOrderNumber", original.ShopOrderNumber);
            yield return new KeyValuePair<string, string>("S_FIELDS", "OrderNumber;Response_Code;BillNumber");
        }

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(ConfirmPaymentRequest original)
        {
            yield return new KeyValuePair<string, string>("Billnumber", original.BillNumber);
            yield return new KeyValuePair<string, string>("Shop_ID", original.ShopId);
            yield return new KeyValuePair<string, string>("Login", original.Login);
            yield return new KeyValuePair<string, string>("Password", original.Password);
            yield return new KeyValuePair<string, string>("Format", ((int)original.Format).ToString("D"));

        }

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(CancelPaymentRequest original)
        {
            yield return new KeyValuePair<string, string>("Billnumber", original.BillNumber);
            yield return new KeyValuePair<string, string>("Shop_ID", original.ShopId);
            yield return new KeyValuePair<string, string>("Login", original.Login);
            yield return new KeyValuePair<string, string>("Password", original.Password);
            yield return new KeyValuePair<string, string>("Format", ((int)original.Format).ToString("D"));

        }
    }
}
