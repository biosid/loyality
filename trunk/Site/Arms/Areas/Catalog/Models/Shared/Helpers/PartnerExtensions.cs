using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Shared.Helpers
{
    public static class PartnerExtensions
    {
        public static string MapName(this PartnerInfo partner)
        {
            return MapName(partner.Name, partner.Status);
        }

        public static string MapName(this Partner partner)
        {
            return MapName(partner.Name, partner.Status);
        }

        private static string MapName(string name, PartnerStatus status)
        {
            return status == PartnerStatus.Enabled ? name : name + " (Неактивный)";
        }
    }
}