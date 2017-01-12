namespace RapidSoft.Loaylty.ProductCatalog.DataSources
{
    using System;
    using System.Globalization;

    public class LoyaltyDBSpecification
    {
        public static string GetDateKey()
        {
            return GetDateKey(DateTime.Now);
        }

        public static string GetDateKey(DateTime dateTime)
        {
            return dateTime.ToString("yyyyMMdd_HHmmss");
        }

        public static string GetImportKey(int partnerId, DateTime dateTime)
        {
            return GetImportKey(partnerId, GetDateKey(dateTime));
        }

        public static string GetImportKey(int partnerId, string dateKey)
        {
            var retVal = string.Format("{0}_{1}", partnerId, dateKey);
            return retVal;
        }

        public static string GetProductId(string partnerId, string offerId)
        {
            var retVal = partnerId + "_" + offerId;
            return retVal;
        }

        public static string GetProductId(int partnerId, string offerId)
        {
            var retVal = GetProductId(partnerId.ToString(CultureInfo.InvariantCulture), offerId);
            return retVal;
        }
    }
}