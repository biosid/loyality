using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.AdminVtbBankConnector.Models;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.Security.Models.Users;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Models;
using Vtb24.Site.Services.Processing;
using Vtb24.Site.Services.Processing.Models.Inputs;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;

namespace Vtb24.Arms.Security.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        public UsersController(ISecurityManagement securityManager, IClientMessageService clientMessage, IClientOrdersManagement orders, IAdminSecurityService security, IAdminVtbBankConnector bankConnector, IProcessing processing)
        {
            _securityManager = securityManager;
            _clientMessage = clientMessage;
            _orders = orders;
            _security = security;
            _bankConnector = bankConnector;
            _processing = processing;
        }

        private readonly ISecurityManagement _securityManager;
        private readonly IClientMessageService _clientMessage;
        private readonly IClientOrdersManagement _orders;
        private readonly IAdminSecurityService _security;
        private readonly IAdminVtbBankConnector _bankConnector;
        private readonly IProcessing _processing;

        [HttpGet]
        public ActionResult Index(bool notFound = false, string login = "")
        {
            _security.CurrentPermissions.AssertAllGranted(PermissionKeys.Security_Login,
                                                          PermissionKeys.Security_Clients);

            var model = new UsersModel
            {
                NoResults = notFound,
                Login = login
            };

            return View("Index", model);
        }


        [HttpGet]
        public ActionResult EditByClientId(string clientId)
        {
            var login = _securityManager.GetLoginByClientId(clientId);

            if (string.IsNullOrWhiteSpace(login))
            {
                throw new HttpException(404, string.Format("Клиент с id {0} не найден", clientId));
            }

            return RedirectToAction("Edit", new {login});
        }


        [HttpGet]
        public ActionResult Edit(string login, int page = 1)
        {
            var normalizedPhone = PhoneFormatter.StripPhoneNumber(login);

            var client = _securityManager.SearchClient(normalizedPhone);

            if (client == null)
                return RedirectToAction("Index", new { notFound = true, login = normalizedPhone });

            var model = UserEditModel.Map(client);

            // история изменений телефона
            const int TAKE = 100;
            var skip = TAKE * (page - 1);
            var loginChangeHistory = _securityManager.GetLoginChangeHistory(client.SecurityUserId, new SecurityPagingSettings(TAKE, skip));

            model.PhoneHistoryPage = loginChangeHistory.SecurityPagingSettings.Skip / loginChangeHistory.SecurityPagingSettings.Take + 1;
            model.TotalPhoneChangeHistoryItems = loginChangeHistory.TotalCount;
            model.PhoneChangeHistory = loginChangeHistory.History
                                                         .Select(PhoneNumberHistoryItemModel.Map)
                                                         .ToArray();

            // статус отключения от программы
            model.IsUserOnBlocking = _securityManager.IsUserOnBlocking(normalizedPhone);

            model.Permissions = UserEditPermissionsModel.Map(_security);

            return View("Edit", model);
        }

        [HttpGet]
        public ActionResult Messages(string login, int page = 1)
        {
            const int PAGE_SIZE = 30;

            var clientId = _securityManager.GetClientIdByLogin(PhoneFormatter.StripPhoneNumber(login));

            if (clientId == null)
            {
                return HttpNotFound();
            }

            var paging = PagingSettings.ByPage(page, PAGE_SIZE);

            var threads = _clientMessage.GetThreads(new ClientGetThreadsParameters
            {
                ClientId = clientId,
                Filter = ReadFilters.UnreadFirst,
                CountToSkip = paging.Skip,
                CountToTake = paging.Take
            });

            var model = new UserMessagesModel
            {
                Login = login,
                Messages = threads.Threads.Select(UserMessageModel.Map).ToArray(),
                Page = page,
                TotalPages = paging.GetTotalPages(threads.TotalCount)
            };

            return View("Messages", model);
        }

        [HttpGet]
        public ActionResult Orders(UserOrdersQueryModel query)
        {
            const int PAGE_SIZE = 30;

            var clientId = _securityManager.GetClientIdByLogin(PhoneFormatter.StripPhoneNumber(query.login));

            var statuses = query.kind.Map();

            if (clientId == null || statuses == null)
            {
                return HttpNotFound();
            }

            var from = query.from.HasValue
                           ? query.from.Value.Date
                           : (DateTime?) null;
            var to = query.to.HasValue
                         ? query.to.Value.Date.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerSecond)
                         : (DateTime?) null;

            var orders = _orders.GetOrders(clientId, statuses, new DateTimeRange(from, to),
                                           PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE));

            var model = new UserOrdersModel
            {
                Query = query,
                Orders = orders.Select(UserOrderModel.Map).ToArray(),
                TotalPages = orders.TotalPages
            };

            return View("Orders", model);
        }

        [HttpGet]
        public ActionResult Order(string login, int orderId, string query)
        {
            var clientId = _securityManager.GetClientIdByLogin(PhoneFormatter.StripPhoneNumber(login));

            if (clientId == null)
            {
                return HttpNotFound();
            }

            try
            {
                var order = _orders.GetOrder(clientId, orderId);

                var model = new UserOrderDetailsModel
                {
                    query = query,
                    Login = login,
                    Order = UserOrderModel.Map(order)
                };

                return View("Order", model);
            }
            catch (OrderNotFoundException)
            {
                return HttpNotFound();
            }
        }

        [HttpGet]
        public ActionResult Points(UserPointsQueryModel query)
        {
            const int PAGE_SIZE = 30;

            var clientId = _securityManager.GetClientIdByLogin(PhoneFormatter.StripPhoneNumber(query.login));

            if (clientId == null)
            {
                return HttpNotFound();
            }

            query.to = query.to.HasValue
                           ? query.to.Value
                           : (query.from.HasValue
                                  ? new DateTime(Math.Min(query.from.Value.AddMonths(1).Ticks, DateTime.Now.Ticks))
                                  : DateTime.Now);

            query.to = query.to.Value.Date.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerSecond);

            query.from = query.from.HasValue && query.from.Value.Date < query.to.Value
                             ? query.from.Value.Date
                             : query.to.Value.AddMonths(-1).Date;

            var parameters = new GetOperationHistoryParameters
            {
                ClientId = clientId,
                From = query.from.Value,
                To = query.to.Value
            };

            var operations = _processing.GetOperationsHistory(parameters, PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE));

            var balance = _processing.GetBalance(new GetBalanceParameters { ClientId = clientId });

            var model = new UserPointsModel
            {
                Query = query,
                Operations = operations.Select(UserPointsOperationModel.Map).ToArray(),
                Balance = balance,
                Total = operations.TotalIncome - operations.TotalOutcome,
                TotalPages = operations.TotalPages
            };

            return View("Points", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableUser(string login, bool disable)
        {
            _securityManager.DisableUser(PhoneFormatter.StripPhoneNumber(login), disable);

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DisableProfile(string login)
        {
            _securityManager.DisableProfile(PhoneFormatter.StripPhoneNumber(login));

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string login)
        {
            _securityManager.ResetPassword(PhoneFormatter.StripPhoneNumber(login));

            return JsonSuccess();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePhone(ChangePhoneNumberModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            var normalizedPhone = PhoneFormatter.StripPhoneNumber(model.Login);

            var clientId = _securityManager.GetClientIdByLogin(normalizedPhone);

            if (clientId == null)
            {
                return HttpNotFound();
            }

            try
            {
                _bankConnector.ChangeClientPhoneNumber(clientId, PhoneFormatter.StripPhoneNumber(model.PhoneNumber));
                return JsonSuccess(new { newUrl = Url.Action("Edit", "Users", new { login = model.PhoneNumber }) });
            }
            catch (AdminVtbBankConnectorPhoneNumberAlreadyUsed)
            {
                ModelState.AddModelError("PhoneNumber", "Данный номер телефона уже используется");
                return JsonErrors();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return JsonErrors();
            }

            var normalizedPhone = PhoneFormatter.StripPhoneNumber(model.Login);

            var clientId = _securityManager.GetClientIdByLogin(normalizedPhone);

            if (clientId == null)
            {
                return HttpNotFound();
            }

            _bankConnector.ChangeClientEmail(clientId, model.Email);

            return JsonSuccess();
        }
    }
}
