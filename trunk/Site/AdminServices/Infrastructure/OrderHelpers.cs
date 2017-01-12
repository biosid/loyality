using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;

namespace Vtb24.Arms.AdminServices.Infrastructure
{
    public static class OrderHelpers
    {
        public const decimal BANK_PRODUCTS_PRICE_RATE = 0.3m;

        private static readonly int BankProductsPartnerId = AppSettingsHelper.Int("bank_orders_partner_id", -1);

        public static bool IsBankProductOrder(this Order order)
        {
            return order.SupplierId == BankProductsPartnerId;
        }
    }
}
