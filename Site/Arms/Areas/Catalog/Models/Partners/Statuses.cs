using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public enum Statuses
    {
        [Description("Активный")]
        Enabled,

        [Description("Неактивный")]
        Disabled
    }

    public static class StatusesExtensions
    {
        public static PartnerStatus Map(this Statuses original)
        {
            switch (original)
            {
                case Statuses.Enabled:
                    return PartnerStatus.Enabled;
                case Statuses.Disabled:
                    return PartnerStatus.Disabled;
            }
            return PartnerStatus.Disabled;
        }

        public static Statuses Map(this PartnerStatus original)
        {
            switch (original)
            {
                case PartnerStatus.Enabled:
                    return Statuses.Enabled;
                case PartnerStatus.Disabled:
                    return Statuses.Disabled;
            }
            return Statuses.Disabled;
        }
    }
}
