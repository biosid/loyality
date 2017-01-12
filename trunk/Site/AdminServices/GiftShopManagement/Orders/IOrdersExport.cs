using System.IO;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Orders
{
    public interface IOrdersExport
    {
        void ExportOrdersHistoryPage(OrdersExportOptions options, TextWriter writer, int page, out int totalPages);
    }
}
