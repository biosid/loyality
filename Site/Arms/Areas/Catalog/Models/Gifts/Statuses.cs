using System;
using System.ComponentModel;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public enum Statuses
    {
// ReSharper disable InconsistentNaming

        [Description("Неизвестно")]
        unknown,

        [Description("Неактивное")]
        not_active,

        [Description("Активное")]
        active
// ReSharper restore InconsistentNaming
    }

    public static class StatusesExtensions
    {
        public static ProductStatus Map(this Statuses original)
        {
            switch (original)
            {
                case Statuses.not_active:
                    return ProductStatus.NotActive;
                case Statuses.active:
                    return ProductStatus.Active;
            }

            throw new NotSupportedException(string.Format("Статус {0} не поддерживается", original));
        }

        public static Statuses Map(this ProductStatus original)
        {
            switch (original)
            {
                case ProductStatus.NotActive:
                    return Statuses.not_active;
                case ProductStatus.Active:
                    return Statuses.active;
            }

            return Statuses.unknown;
        }
    }
}