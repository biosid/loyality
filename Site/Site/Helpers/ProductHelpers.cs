using System.Configuration;

namespace Vtb24.Site.Helpers
{
    public static class ProductHelpers
    {
        private static readonly int RedeemFactor;
        
        static ProductHelpers()
        {
            var redeemFactorStr = ConfigurationManager.AppSettings["can_redeem_product_factor"];

            if (string.IsNullOrEmpty(redeemFactorStr) ||
                !int.TryParse(redeemFactorStr, out RedeemFactor) ||
                RedeemFactor <= 0 || RedeemFactor > 100)
            {
                RedeemFactor = 100;
            }
        }

        public static bool CanRedeem(decimal price, decimal balance)
        {
            return balance >= price * RedeemFactor / 100;
        }

        public static decimal MaxCost(decimal balance)
        {
            return balance / RedeemFactor * 100; // RedeemFactor гарантировано больше 0
        }
    }
}
