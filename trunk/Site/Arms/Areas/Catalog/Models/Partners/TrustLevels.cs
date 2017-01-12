using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public enum TrustLevels
    {
        [Description("Низкая")]
        Low,

        [Description("Средняя")]
        Middle,

        [Description("Высокая")]
        High
    }

    public static class TrustLevelsExtensions
    {
        public static SupplierTrustLevel Map(this TrustLevels original)
        {
            switch (original)
            {
                case TrustLevels.Low:
                    return SupplierTrustLevel.Low;
                case TrustLevels.Middle:
                    return SupplierTrustLevel.Middle;
                case TrustLevels.High:
                    return SupplierTrustLevel.High;
            }
            return SupplierTrustLevel.Low;
        }

        public static TrustLevels Map(this SupplierTrustLevel original)
        {
            switch (original)
            {
                case SupplierTrustLevel.Low:
                    return TrustLevels.Low;
                case SupplierTrustLevel.Middle:
                    return TrustLevels.Middle;
                case SupplierTrustLevel.High:
                    return TrustLevels.High;
            }
            return TrustLevels.Low;
        }
    }
}
