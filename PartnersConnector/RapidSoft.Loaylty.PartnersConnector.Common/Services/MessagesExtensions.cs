namespace RapidSoft.Loaylty.PartnersConnector.Common.DTO.Offline
{
    using System.Globalization;

    public static class MessagesExtensions
    {
        public static string GetStringFromPrice(this decimal? decimalVal)
        {
            if (!decimalVal.HasValue)
            {
                return null;
            }
            else
            {
                return decimalVal.Value.ToString("F2", CultureInfo.InvariantCulture);
            }
        }

        public static decimal? GetPriceFromString(this string value)
        {
            if (value == null)
            {
                return null;
            }

            decimal price;
            decimal.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out price);
            return price;            
        }
    }
}