using System;
using Vtb24.Arms.AdminServices.GiftShopManagement.Products.Models;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class ModerateModel
    {
        public string[] Ids { get; set; }

        public ModerationStatus Status { get; set; }

        public ProductModerationStatus MapStatus()
        {
            switch (Status)
            {
                case ModerationStatus.Deny:
                    return ProductModerationStatus.Canceled;
                case ModerationStatus.Approve:
                    return  ProductModerationStatus.Applied;
            }

            throw new NotSupportedException(string.Format("Статус {0} не поддерживается", Status));
        }

        public enum ModerationStatus
        {
            Approve,
            Deny
        }
    }
}
