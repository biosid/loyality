using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public enum Moderations
    {
// ReSharper disable InconsistentNaming
        [Description("Модерация: все товары")]
        all,

        [Description("Принятые")]
        applied,

        [Description("Отклонённые")]
        canceled,

        [Description("Ожидают модерации")]
        in_moderation
// ReSharper restore InconsistentNaming
    }

    public static class ModerationsExtensions
    {
        public static ProductModerationStatus? Map(this Moderations? original)
        {
            switch (original)
            {
                case Moderations.all:
                    return null;
                case Moderations.applied:
                    return ProductModerationStatus.Applied;
                case Moderations.canceled:
                    return ProductModerationStatus.Canceled;
                case Moderations.in_moderation:
                    return ProductModerationStatus.InModeration;
            }
            return null;
        }
    }
}
