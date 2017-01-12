using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminSecurity.Models.Shared;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Helpers
{
    public static class PermissionsHelpers
    {
        public static void Hydrate(this PermissionsEditModel model, IGiftShopManagement catalog)
        {
            model.Lists = new Dictionary<PermissionKeys, ListItemModel[]>();

            var partners = catalog.GetPartnersInfo()
                                  .OrderBy(partner => partner.Name)
                                  .Select(partner => new ListItemModel
                                  {
                                      Id = partner.Id.ToString("D"),
                                      Name = partner.Name
                                  })
                                  .ToArray();

            model.Lists[PermissionKeys.Catalog_PartnerIds] = partners;
        }
    }
}