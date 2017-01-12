namespace RapidSoft.Loaylty.ProductCatalog.Import
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Loaylty.ProductCatalog.API.Entities;

    internal static class ModerationStatusCalculatorFactory
    {
        public static IModerationStatusCalculator Build(Partner partner, IList<Product> existsProduct)
        {
            var config = ConfigurationManager.AppSettings["TestPartnerIds"];
            if (config != null)
            {
                var separ = new[] { ',' };
                var ids = config.Split(separ).Select(x => x.Trim());

                var partnerId = partner.Id.ToString(CultureInfo.InvariantCulture);

                if (ids.Any(x => x == partnerId))
                {
                    return new TestPartnerCalculator();
                }
            }

            return Build(partner.ThrustLevel, existsProduct);
        }

        private static IModerationStatusCalculator Build(PartnerThrustLevel partnerThrustLevel, IList<Product> existsProduct)
        {
            switch (partnerThrustLevel)
            {
                case PartnerThrustLevel.Low:
                    return new LowPartnerTrustCalculator(existsProduct);
                case PartnerThrustLevel.Middle:
                    return new MiddlePartnerTrustCalculator(existsProduct);
                case PartnerThrustLevel.High:
                    return new HighPartnerTrustCalculator(existsProduct);
                default:
                    throw new NotSupportedException(string.Format("Уровень доверия партнеру {0} не поддерживается", partnerThrustLevel));
            }
        }
    }
}