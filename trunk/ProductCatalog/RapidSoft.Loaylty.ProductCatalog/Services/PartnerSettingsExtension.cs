namespace RapidSoft.Loaylty.ProductCatalog.Services
{
    using System;
    using System.Collections.Generic;

    public static class PartnerSettingsExtension
    {
        public const string ReportRecipientsKey = "ReportRecipients";
        public const string CheckOrderUrlKey = "Check";
        public const string FixPriceUrlKey = "FixBasketItemPrice";
        public const string MultiPositionOrdersKey = "HasMultiPositionOrders";
        public const string GetDeliveryVariantsKey = "GetDeliveryVariants";
        public const string DisableAutoOrderStatusChangeKey = "DisableAutoOrderStatusChange";

        public const string ImportDeliveryRatesEtlPackageId = "ImportDeliveryRatesEtlPackageId";

        /// <summary>
        /// Идентификатор etl-пакета который должен использоваться для импорта список тарифов доставки по умолчанию.
        /// </summary>
        public static Guid DefaultImportDeliveryRatesEtlPackageId
        {
            get
            {
                return new Guid("77A3E3C6-C00B-41FF-8376-DCEF0DF79A00");
            }
        }

        public static string GetImportDeliveryRatesEtlPackageId(this Dictionary<string, string> settings)
        {
            var value = settings.GetValue(ImportDeliveryRatesEtlPackageId);
            return value ?? DefaultImportDeliveryRatesEtlPackageId.ToString();
        }

        public static bool GetCheckOrderSupported(this Dictionary<string, string> settings)
        {
            if (settings == null)
            {
                return false;
            }

            var value = settings.GetValue(CheckOrderUrlKey);

            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool GetDeliveryVariantsSupported(this Dictionary<string, string> settings)
        {
            if (settings == null)
            {
                return false;
            }

            var value = settings.GetValue(GetDeliveryVariantsKey);

            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool GetFixPriceSupported(this Dictionary<string, string> settings)
        {
            if (settings == null)
            {
                return false;
            }

            var value = settings.GetValue(FixPriceUrlKey);

            return !string.IsNullOrWhiteSpace(value);
        }

        public static bool GetMultiPositionOrdersSupported(this Dictionary<string, string> settings)
        {
            if (settings == null)
            {
                return false;
            }

            var value = settings.GetValue(MultiPositionOrdersKey);

            return !string.IsNullOrWhiteSpace(value);
        }

        public static string GetReportRecipients(this Dictionary<string, string> settings, string defaultValue)
        {
            var value = settings.GetValue(ReportRecipientsKey);
            return value ?? defaultValue;
        }

        public static string GetValue(this Dictionary<string, string> settings, string key)
        {
            if (settings == null)
            {
                return null;
            }

            string retVal;
            return settings.TryGetValue(key, out retVal) ? retVal : null;
        }
    }
}