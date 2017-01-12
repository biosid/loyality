using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;

namespace Vtb24.Arms.Catalog.Models.Partners
{
    public enum Types
    {
        [Description("Директ-партнёр")]
        Direct,

        [Description("Оффлайн-партнёр")]
        Offline,

        [Description("Онлайн-партнёр")]
        Online
    }

    public static class TypesExtensions
    {
        public static SupplierType Map(this Types original)
        {
            switch (original)
            {
                case Types.Direct:
                    return SupplierType.Direct;
                case Types.Offline:
                    return SupplierType.Offline;
                case Types.Online:
                    return SupplierType.Online;
            }
            return SupplierType.Direct;
        }

        public static Types Map(this SupplierType original)
        {
            switch (original)
            {
                case SupplierType.Direct:
                    return Types.Direct;
                case SupplierType.Offline:
                    return Types.Offline;
                case SupplierType.Online:
                    return Types.Online;
            }
            return Types.Direct;
        }
    }
}
