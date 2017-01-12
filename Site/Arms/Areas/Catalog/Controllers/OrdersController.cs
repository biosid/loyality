using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Models.Exceptions;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models;
using Vtb24.Arms.AdminServices.GiftShopManagement.Orders.Models.Inputs;
using Vtb24.Arms.AdminServices.GiftShopManagement.Partners.Models;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;
using Vtb24.Arms.Catalog.Models.Orders.Helpers;
using Vtb24.Arms.Catalog.Models.Shared.Helpers;
using Vtb24.Arms.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Catalog.Models.Orders;
using System.Text;

namespace Vtb24.Arms.Catalog.Controllers
{
    [Authorize]
    public class OrdersController : BaseController 
    {
        private const int PAGE_SIZE = 50;

        public OrdersController(IGiftShopManagement catalog, IAdminSecurityService security)
        {
            _catalog = catalog;
            _security = security;
        }

        private readonly IGiftShopManagement _catalog;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Catalog_Login, PermissionKeys.Catalog_Orders);

            // зададим умолчательные параметры и перенаправим на список

            if (_catalog.GetUserSuppliersInfo().Any())
            {
                var query = new OrdersQueryModel { issupplier = true };
                return RedirectToAction("List", "Orders", query);
            }

            if (_catalog.GetUserCarriersInfo().Any())
            {
                var query = new OrdersQueryModel { issupplier = false };
                return RedirectToAction("List", "Orders", query);
            }

            return View("NoPartners");
        }

        [HttpGet]
        public ActionResult List(OrdersQueryModel query)
        {
            var suppliers = _catalog.GetUserSuppliersInfo();
            var carriers = _catalog.GetUserCarriersInfo();

            int[] supplierIds;
            int[] carrierIds;

            if (!MapPartnersIds(query.partner, query.issupplier, suppliers, carriers, out supplierIds, out carrierIds))
            {
                return HttpNotFound();
            }

            var options = new OrdersSearchCriteria
            {
                SupplierIds = supplierIds,
                CarrierIds = carrierIds,
                OrderId = query.id,
                From = query.from.HasValue ? query.from.Value.Date : (DateTime?) null,
                To = query.to.HasValue ? query.to.Value.Date.AddTicks(TimeSpan.TicksPerDay - 1) : (DateTime?) null,
                Statuses = query.status.MaybeSelect(OrderStatusesExtensions.Map).MaybeToArray(),
                SkipStatuses = new[] { OrderStatus.Draft, OrderStatus.Registration },
                ProductPaymentStatuses = query.productpayment.MaybeSelect(OrderPaymentStatusExtensions.Map).MaybeToArray(),
                DeliveryPaymentStatuses = query.deliverypayment.MaybeSelect(OrderPaymentStatusExtensions.Map).MaybeToArray()
            };

            var orders = _catalog.SearchOrders(options, PagingSettings.ByPage( query.page ?? 1, PAGE_SIZE));

            var model = new OrdersModel
            {
                Query = query,
                Suppliers = MapSuppliersDropDown(suppliers, query.partner, query.issupplier).ToArray(),
                Carriers = MapCarriersDropDown(carriers, query.partner, query.issupplier).ToArray(),
                Orders = orders.Select(OrderModel.Map).ToArray(),
                TotalPages = orders.TotalPages
            };

            return View("List", model);
        }

        [HttpGet]
        public ActionResult StartExport(OrdersQueryModel query)
        {
            var suppliers = _catalog.GetUserSuppliersInfo();
            var carriers = _catalog.GetUserCarriersInfo();

            int[] supplierIds;
            int[] carrierIds;

            if (!MapPartnersIds(query.partner, query.issupplier, suppliers, carriers, out supplierIds, out carrierIds))
            {
                return HttpNotFound();
            }

            var criteria = new OrdersSearchCriteria
            {
                SupplierIds = supplierIds,
                CarrierIds = carrierIds,
                OrderId = query.id,
                From = query.from.HasValue ? query.from.Value.Date : (DateTime?) null,
                To = query.to.HasValue ? query.to.Value.Date.AddTicks(TimeSpan.TicksPerDay - 1) : (DateTime?) null,
                Statuses = query.status.MaybeSelect(OrderStatusesExtensions.Map).MaybeToArray(),
                SkipStatuses = new[] { OrderStatus.Draft, OrderStatus.Registration },
                ProductPaymentStatuses =
                    query.productpayment.MaybeSelect(OrderPaymentStatusExtensions.Map).MaybeToArray(),
                DeliveryPaymentStatuses =
                    query.deliverypayment.MaybeSelect(OrderPaymentStatusExtensions.Map).MaybeToArray()
            };

            var criteriaStr = JsonConvert.SerializeObject(criteria);

            var fileName = String.Format("orders-{0:yyyyMMdd-HHmmss}.csv", DateTime.Now);

            return new JsonResult
            {
                Data = new
                {
                    criteria = criteriaStr,
                    fileName
                },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ExportBatch(string criteria, string fileName, int page)
        {
            if (page <= 0)
            {
                return HttpNotFound();
            }

            var dirPath = ConfigurationManager.AppSettings["order_export_path"];
            var filePath = Path.Combine(dirPath, fileName);

            if ((page == 1 && System.IO.File.Exists(filePath)) ||
                (page != 1 && !System.IO.File.Exists(filePath)))
            {
                return new JsonResult
                {
                    Data = new { status = "failed" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }

            if (page == 1 && !Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            var searchCriteria = JsonConvert.DeserializeObject<OrdersSearchCriteria>(criteria);

            var options = new OrdersExportOptions
            {
                SearchCriteria = searchCriteria,
                MapOrderStatus = MapOrderStatus,
                MapOrderPaymentStatus = MapOrderPaymentStatus,
                MapOrderDelivery = DeliveryFormatter.Map
            };

            int totalPages;

            using (var writer = new StreamWriter(filePath, true, Encoding.UTF8))
            {
                _catalog.ExportOrdersHistoryPage(options, writer, page, out totalPages);
            }

            return new JsonResult
            {
                Data = new { status = "ok", totalPages },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult ExportResults(string fileName)
        {
            var filePath = Path.Combine(ConfigurationManager.AppSettings["order_export_path"], fileName);
            
            if (!System.IO.File.Exists(filePath))
            {
                return HttpNotFound();
            }

            Response.AddHeader("content-disposition", "attachment;filename=" + fileName);

            return new FilePathResult(filePath, "text/csv");
        }

        [HttpGet]
        public ActionResult Edit(int id, string query)
        {
            var model = new OrderEditModel
            {
                Id = id,
                query = query
            };

            try
            {
                Hydrate(model);
            }
            catch (EntityNotFoundException e)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "заказ не найден", e);
            }

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(OrderEditModel model)
        {
            if (ModelState.IsValid)
            {
                var options = new ChangeOrderStatusOptions
                {
                    Id = model.Id,
                    Status = model.OrderStatus.Map(),
                    StatusDescription = model.OrderStatusDescription
                };

                _catalog.ChangeOrderStatus(options);

                return RedirectToAction("List", "Orders", model.OrdersQueryModel);
            }

            try
            {
                Hydrate(model);
            }
            catch (EntityNotFoundException e)
            {
                throw new HttpException((int)HttpStatusCode.NotFound, "заказ не найден", e);
            }

            return View("Edit", model);
        }

        private void Hydrate(OrderEditModel model)
        {
            var order = _catalog.GetOrder(model.Id);
            if (order.Items == null || order.Items.Length == 0)
                throw new InvalidOperationException(string.Format("Заказ не содержит продукт: {0}", model.Id));

            var statusHistory = _catalog.GetOrderStatusHistory(model.Id);

            var permissions = OrderEditPermissionsModel.Map(_security);

            model.ExternalId = order.ExternalId;
            model.SupplierName = GetSupplierName(order.SupplierId);
            model.CarrierName = GetCarrierName(order.CarrierId);
            model.CreateDate = order.CreateDate;
            model.StatusChangeDate = order.StatusChangeDate;
            model.OrderStatus = order.Status.Map();
            if (string.IsNullOrEmpty(model.OrderStatusDescription))
                model.OrderStatusDescription = order.StatusDescription;
            model.OrderStatusesList = model.OrderStatus.HasValue
                                          ? MapStatusesDropDown(model.OrderStatus.Value, order.NextStatuses.Map()).ToArray()
                                          : null;
            model.ProductPaymentStatus = order.ProductPaymentStatus.Map();
            model.DeliveryPaymentStatus = order.DeliveryPaymentStatus.Map();
            model.Items = order.Items.Select(OrderItemModel.Map).ToArray();
            model.ItemsPrice = order.ItemsPrice;
            model.ItemsBonusPrice = order.ItemsBonusPrice;
            model.DeliveryPrice = order.DeliveryPrice;
            model.DeliveryBonusPrice = order.DeliveryBonusPrice;
            model.DeliveryAdvance = order.DeliveryAdvance;
            model.TotalAdvance = order.TotalAdvance;
            model.TotalPrice = order.TotalPrice;
            model.TotalBonusPrice = order.TotalBonusPrice;
            model.ContactInfo = order.Delivery != null
                                    ? OrderEditModel.ContactInfoModel.Map(order.Delivery.Contact)
                                    : null;
            model.Address = order.Delivery != null
                                ? DeliveryFormatter.Map(order.Delivery)
                                : null;
            model.DeliveryInfo = OrderEditModel.DeliveryInfoModel.Map(order.Delivery);
            model.StatusHistory = statusHistory.Select(StatusHistoryRecordModel.Map).ToArray();
            model.Permissions = permissions;
            model.HideRurPrices = order.IsBankProductOrder();
        }

        private string GetSupplierName(int id)
        {
            try
            {
                var supplier = _catalog.GetUserSupplierInfoById(id);
                if (supplier != null)
                    return supplier.MapName();
            }
            catch (CatalogManagementServiceException)
            {
            }

            return string.Format("Неизвестный поставщик ({0})", id);
        }

        private string GetCarrierName(int? id)
        {
            if (!id.HasValue)
                return "- нет -";

            try
            {
                var carrier = _catalog.GetUserCarrierInfoById(id.Value);
                if (carrier != null)
                    return carrier.MapName();
            }
            catch (CatalogManagementServiceException)
            {
            }

            return string.Format("Неизвестный курьер ({0})", id.Value);
        }

        private bool MapPartnersIds(int? partnerId, bool isSupplier, SupplierInfo[] suppliers, CarrierInfo[] carriers, out int[] supplierIds, out int[] carrierIds)
        {
            supplierIds = null;
            carrierIds = null;

            if (!partnerId.HasValue)
            {
                if (isSupplier)
                {
                    if (suppliers.Length == 0)
                        throw new SecurityException("Нет доступных поставщиков");

                    supplierIds = suppliers.Select(s => s.Id).ToArray();
                }
                else
                {
                    if (carriers.Length == 0)
                        throw new SecurityException("Нет доступных курьеров");

                    carrierIds = carriers.Select(c => c.Id).ToArray();
                }
            }
            else
            {
                if (isSupplier)
                {
                    var supplier = suppliers.FirstOrDefault(s => s.Id == partnerId.Value);
                    if (supplier == null)
                        return false;

                    supplierIds = new[] { supplier.Id };
                }
                else
                {
                    var carrier = carriers.FirstOrDefault(c => c.Id == partnerId.Value);
                    if (carrier == null)
                        return false;

                    carrierIds = new[] { carrier.Id };
                }
            }

            return true;
        }

        private IEnumerable<SelectListItem> MapSuppliersDropDown(SupplierInfo[] suppliers, int? selectedId, bool isSupplier)
        {
            if (suppliers.Length == 0)
                yield break;

            yield return new SelectListItem
            {
                Text = "- все поставщики -",
                Selected = isSupplier && !selectedId.HasValue,
                Value = Url.Action("List", "Orders", new OrdersQueryModel { issupplier = true })
            };

            foreach (var supplier in suppliers.OrderBy(s => s.Name))
                yield return new SelectListItem
                {
                    Selected = selectedId.HasValue && supplier.Id == selectedId.Value,
                    Text = supplier.MapName(),
                    Value = Url.Action("List", "Orders", new OrdersQueryModel { partner = supplier.Id, issupplier = true })
                };
        }

        private IEnumerable<SelectListItem> MapCarriersDropDown(CarrierInfo[] carriers, int? selectedId, bool isSupplier)
        {
            if (carriers.Length == 0)
                yield break;

            yield return new SelectListItem
            {
                Text = "- все курьеры -",
                Selected = !isSupplier && !selectedId.HasValue,
                Value = Url.Action("List", "Orders", new OrdersQueryModel { issupplier = false })
            };

            foreach (var carrier in carriers.OrderBy(c => c.Name))
                yield return new SelectListItem
                {
                    Selected = selectedId.HasValue && carrier.Id == selectedId.Value,
                    Text = carrier.MapName(),
                    Value = Url.Action("List", "Orders", new OrdersQueryModel { partner = carrier.Id, issupplier = false })
                };
        }

        private static IEnumerable<SelectListItem> MapStatusesDropDown(OrderStatuses currentStatus, IEnumerable<OrderStatuses> nextStatuses)
        {
            yield return new SelectListItem
            {
                Text = currentStatus.EnumDescription() + " (текущий)",
                Value = currentStatus.ToString(),
                Selected = true
            };

            foreach (var status in nextStatuses.Where(status => status != currentStatus))
            {
                yield return new SelectListItem
                {
                    Text = status.EnumDescription(),
                    Value = status.ToString(),
                    Selected = false
                };
            }
        }

        private static string MapOrderStatus(OrderStatus original)
        {
            var status = original.Map();

            return status.HasValue ? status.Value.EnumDescription() : "Неизвестен";
        }

        private static string MapOrderPaymentStatus(OrderPaymentStatus original)
        {
            var status = original.Map();

            return status.HasValue ? status.Value.EnumDescription() : "Неизвестно";
        }
    }
}
