using System;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class ActivateModel
    {
        public string[] Ids { get; set; }

        public ActivationStatus Status { get; set; }

        public ProductStatus MapStatus()
        {
            switch (Status)
            {
                case ActivationStatus.Activate:
                    return ProductStatus.Active;
                case ActivationStatus.Deactivate:
                    return ProductStatus.NotActive;
            }

            throw new NotSupportedException(string.Format("Статус {0} не поддерживается", Status));
        }

        public enum ActivationStatus
        {
            Activate,

            Deactivate
        }
    }
}
