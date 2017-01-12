using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.MyOrders;
using Vtb24.Site.Services;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;

namespace Vtb24.Site.Controllers
{
    [Authorize]
    public class MyOrdersController : BaseController
    {
        private const int PAGE_SIZE = 30;

        public MyOrdersController(IGiftShop catalog, IClientService client)
        {
            _catalog = catalog;
            _client = client;
        }

        private readonly IGiftShop _catalog;
        private readonly IClientService _client;

        [HttpGet]
        public ActionResult Index(int? page = 1)
        {
            page = page ?? 1;

            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            OrderStatus[] inProcessStatuses =
            {
                OrderStatus.Processing, OrderStatus.DeliveryWaiting,
                OrderStatus.Delivery
            };

            var history = _catalog.GetOrdersHistory(inProcessStatuses, new DateTimeRange(),
                                                    PagingSettings.ByPage(page.Value, PAGE_SIZE));

            var model = new MyOrdersModel
            {
                Orders = history.Select(MyOrdersOrderModel.Map).ToList(),
                Kind = OrdersKind.InProcess,
                TotalPages = history.TotalPages,
                Page = page.Value
            };

            ViewBag.Tab = 1;

            if (model.Page > model.TotalPages || model.Page < 1)
            {
                throw new HttpException(404, "страница заказов не найдена");
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult History(int page = 1)
        {
            DumpActivationStatusToViewBag(_client); // TODO: удалить этот хак

            OrderStatus[] historyStatuses = { OrderStatus.Delivered, OrderStatus.NotDelivered, OrderStatus.Cancelled }; // OrderStatus.Cancelled не нужен: http://jira.rapidsoft.ru/browse/VTBPLK-1820 

            var history = _catalog.GetOrdersHistory(historyStatuses, new DateTimeRange(), PagingSettings.ByPage(page, PAGE_SIZE));

            var model = new MyOrdersModel
            {
                Orders = history.Select(MyOrdersOrderModel.Map).ToList(),
                Kind = OrdersKind.History,
                TotalPages = history.TotalPages,
                Page = page
            };

            ViewBag.Tab = 2;

            if (model.Page > model.TotalPages || model.Page < 1)
            {
                throw new HttpException(404, "страница заказов не найдена");
            }
            return View("Index", model);
        }

        [HttpGet]
        public ActionResult Order(int id, int page = 1)
        {
            var order = _catalog.GetOrder(id);

            if (order == null)
            {
                throw new HttpException(404, "Заказ не найден");
            }

            var model = OrderModel.Map(order);

            return View("Order", model);
        }
    }
}