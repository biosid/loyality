using System;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Site.Services.GiftShop;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Outputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Arms.AdminServices.ClientOrdersManagement
{
    public class ClientOrdersManagement : IClientOrdersManagement, IDisposable
    {
        public ClientOrdersManagement(IGiftShopOrders orders, IAdminSecurityService adminSecurity)
        {
            _orders = orders;
            _adminSecurity = adminSecurity;
        }

        private readonly IGiftShopOrders _orders;
        private readonly IAdminSecurityService _adminSecurity;

        #region API

        public GiftShopOrdersResult GetOrders(string clientId, OrderStatus[] statuses, DateTimeRange range, PagingSettings paging)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            return _orders.GetOrdersHistory(clientId, statuses, range, paging);
        }

        public GiftShopOrder GetOrder(string clientId, int orderId)
        {
            _adminSecurity.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                               PermissionKeys.Security_Clients);

            return _orders.GetOrder(clientId, orderId);
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            // Do nothing
        }

        #endregion
    }
}
