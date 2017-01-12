using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Vtb24.Logging.Interaction;
using Vtb24.Site.Content.Pages;
using Vtb24.Site.Content.Pages.Models;
using Vtb24.Site.Filters;
using Vtb24.Site.Helpers;
using Vtb24.Site.Infrastructure;
using Vtb24.Site.Models.Basket;
using Vtb24.Site.Models.Buy;
using Vtb24.Site.Models.Catalog;
using Vtb24.Site.Models.Shared;
using Vtb24.Site.Security;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Security.OneTimePasswordService.Models.Exceptions;
using Vtb24.Site.Security.OneTimePasswordService.Models.Inputs;
using Vtb24.Site.Services;
using Vtb24.Site.Services.AdvancePayment.Models.Exceptions;
using Vtb24.Site.Services.BonusPayments.Models.Exceptions;
using Vtb24.Site.Services.Buy.Models.Exceptions;
using Vtb24.Site.Services.Buy.Models.Inputs;
using Vtb24.Site.Services.ClientMessageService;
using Vtb24.Site.Services.GiftShop.Basket.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Catalog.Models;
using Vtb24.Site.Services.GiftShop.Catalog.Models.Inputs;
using Vtb24.Site.Services.GiftShop.Model;
using Vtb24.Site.Services.GiftShop.Orders.Models;
using Vtb24.Site.Services.GiftShop.Orders.Models.Exceptions;
using Vtb24.Site.Services.GiftShop.Orders.Models.Inputs;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.OnlineCategory.Models.Inputs;
using ConfirmOrderParams = Vtb24.Site.Services.Buy.Models.Inputs.ConfirmOrderParams;
using IClientMessageService = Vtb24.Site.Services.IClientMessageService;
using Pluralizer = Vtb24.Site.Infrastructure.Pluralizer;

namespace Vtb24.Site.Controllers
{
    public class BuyController : BaseController
    {
        private const int THANK_YOU_PRODUCTS_COUNT = 4;

        public const string PHONE_COUNTRY_CODE = "7";
        public const string RUSSIA_COUNTRY_CODE = "RU";
        public const string RUSSIA_TITLE = "Россия";

        public const string IS_ORDER_REQUIRES_EMAIL_PARTNER_SETTING_KEY = "IsOrderRequiresEmail";

        private static readonly bool DisableProposalToSaveEmail;
        
        static BuyController()
        {
            bool value;
            DisableProposalToSaveEmail =
                bool.TryParse(ConfigurationManager.AppSettings["disable_propose_to_save_email_on_buy_form"], out value) &&
                value;
        }

        public BuyController(IGiftShop catalog,
            IClientService clientService,
            IOneTimePasswordService otp,
            IBuy buy,
            IOnlineCategoryService online,
            IGeoService geo,
            ICardRegistration card,
            IPagesManagement pages,
            IClientMessageService clientMessage,
            ClientPrincipal principal)
        {
            _catalog = catalog;
            _client = clientService;
            _otp = otp;
            _buy = buy;
            _online = online;
            _geo = geo;
            _card = card;
            _pages = pages;
            _clientMessage = clientMessage;
            _principal = principal;
        }

        private readonly IGiftShop _catalog;
        private readonly IClientService _client;
        private readonly IOneTimePasswordService _otp;
        private readonly IBuy _buy;
        private readonly IOnlineCategoryService _online;
        private readonly IGeoService _geo;
        private readonly ICardRegistration _card;
        private readonly IPagesManagement _pages;
        private readonly IClientMessageService _clientMessage;
        private readonly ClientPrincipal _principal;

        #region Actions

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult Items(string[] id)
        {
            if (id == null || id.Length == 0)
            {
                return RedirectToAction("Index", "Basket");
            }

            // зарегистрирована ли карта?
            if (!_card.IsCardRegistered())
            {
                return View("RegisterCard");
            }

            // предыдущее состояние формы
            var postbackModel = ViewBag.PostBack as BuyModel;

            // заказываемые элементы корзины
            var basketItems = _catalog.GetBasketItems(id);
            var availableItems = basketItems.Where(i => i != null && i.ProductStatus == ProductStatus.Available);

            // получим баланс для расчёта возможности заказа
            var balance = _client.GetBalance();

            // получим максимальную возможную стоимость заказа
            var maxOrderCost = ProductHelpers.MaxCost(balance);

            // получаем группы товаров (товары сгруппированы по поставщику)
            var itemGroups = availableItems
                .GroupBy(GetReservedProductItemGroupKey)
                .Where(g => g.Sum(i => i.TotalQuantityPrice) <= maxOrderCost); // только группы, на которые хватает денег

            // выбираем ту единственную группу, которую включим в заказ (группу с наибольшим числом позиций)
            var itemsToOrder = itemGroups.OrderByDescending(g => g.Count()).FirstOrDefault().MaybeToArray();

            if (itemsToOrder == null)
            {
                TempData["error"] = id.Length > 1
                    ? "Извините, эти вознаграждения временно недоступны"
                    : "Извините, это вознаграждение временно недоступно.";
                return RedirectToAction("Index", "Basket");
            }

            // пионэр всегда готов!
            var partnerId = itemsToOrder.First().Product.PartnerId;
            var itemsIds = itemsToOrder.Select(i => i.Id).ToArray();
            var productIds = itemsToOrder.Select(i => i.Product.Id).ToArray();

            // получаем настройки партнера
            var partnerSettings = GetPartnerSettings(partnerId);

            // поддерживается ли онлайн-взаимодействие по получению вариантов доставки?
            var onlineDeliveryVariantsEnabled = partnerSettings.IsOnlineDeliveryVariansSupported;

            // обязательно ли поле E-mail?
            var isEmailRequired = partnerSettings.IsOrderRequiresEmail || itemsToOrder.Any(i => i.Product.IsDeliveredByEmail);

            // предложить сохранить E-mail?
            var proposeToSaveEmail = !DisableProposalToSaveEmail && string.IsNullOrWhiteSpace(_client.GetEmail());

            // поддержвается ли оплата картой и какова максимальная доля оплаты?
            var advancePaymentSupport = partnerSettings.AdvancePaymentSupport;
            var maxAdvanceFraction = 0;

            if (advancePaymentSupport == AdvancePaymentSupportMode.Full)
            {
                if (partnerSettings.MaxAdvanceFraction.HasValue &&
                    partnerSettings.MaxAdvanceFraction.Value > 0 &&
                    partnerSettings.MaxAdvanceFraction.Value <= 100)
                {
                    maxAdvanceFraction = partnerSettings.MaxAdvanceFraction.Value;
                }
                else
                {
                    advancePaymentSupport = AdvancePaymentSupportMode.None;
                }
            }

            // предзаполним контактные данные
            var contact = CreateContactModel();

            // получим ссылку на страницу с офертой
            var formalOfferUrl = GetFormalOfferUrl(partnerId);

            // получим ранее сохранённые адреса
            var savedAddressed = CreateSavedAddressesModel(excludeWithoutKladr: !onlineDeliveryVariantsEnabled);

            // предзаполним населённый пункт в адрессе доставки (только если варианты доставки не поддерживаются)
            var address = onlineDeliveryVariantsEnabled ? null : CreateDeliveryAddressModel();

            // получим список регионов (только если варианты доставки не поддерживаются)
            var regions = onlineDeliveryVariantsEnabled ? null : CreateRegionsModel();

            // предзаполним варианты доставки для партнёров, которые их не поддерживают
            var variants = !onlineDeliveryVariantsEnabled && postbackModel == null
                ? CreateDeliveryVariantsModel(null, address.LocationKladr, itemsIds, advancePaymentSupport != AdvancePaymentSupportMode.None)
                : null;

            var items = itemsToOrder.Select(i => BasketItemModel.Map(i, balance)).ToArray();

            var itemsPriceBonus = itemsToOrder.Sum(i => i.TotalQuantityPrice);
            var itemsPriceRur = itemsToOrder.Sum(i => i.TotalQuantityPriceRur);

            var model = new BuyModel
            {
                Balance = (int)balance,
                BasketItemIds = itemsIds,
                ProductIds = productIds,
                PartnerId = partnerId,
                Contact = contact,
                Items = items,
                ShowItemsWarning = id.Length != items.Length,
                FormalOfferUrl = formalOfferUrl,
                SavedAddresses = savedAddressed,
                Address = address,
                Regions = regions,
                Variants = variants,
                OnlineDeliveryVariantsEnabled = onlineDeliveryVariantsEnabled,
                IsEmailRequired = isEmailRequired || proposeToSaveEmail,
                ProposeToSaveEmail = proposeToSaveEmail,
                AdvancePaymentSupport = advancePaymentSupport,
                MaxAdvanceFraction = maxAdvanceFraction,
                DisableButton = !items.All(i => i.CanBuy),
                ItemsPriceBonus = itemsPriceBonus,
                ItemsPriceRur = itemsPriceRur
            };

            // если это постбэк, восстанавливаем состояние до отправки формы
            if (postbackModel != null)
            {
                // восстанавливаем варианты доставки
                var postCode = postbackModel.Address.PostCode;
                var kladrCode = postbackModel.Address.LocationKladr;
                model.Variants = CreateDeliveryVariantsModel(postCode, kladrCode, postbackModel.BasketItemIds, advancePaymentSupport != AdvancePaymentSupportMode.None);

                // востанавлиаем выбранный вариант доставки
                if (model.Variants != null)
                {
                    model.Variants.DeliveryVariantId = postbackModel.DeliveryVariantId;
                    model.Variants.PickupPointId = postbackModel.PickupPointId;
                }

                // восстанавливаем выбранный размер доплаты рублями
                model.AdvancePayment = postbackModel.AdvancePayment;

                // восстанавливаем состояние кнопки
                if (postbackModel.DisableButton)
                {
                    model.DisableButton = true;
                }
            }

            return View("Buy", model);
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Items(BuyModel model)
        {
            var isMultiItem = model.BasketItemIds.Length > 1;

            var partnerSettings = GetPartnerSettings(model.PartnerId);

            var onlineDeliveryVariantsEnabled = partnerSettings.IsOnlineDeliveryVariansSupported;

            var selectedDeliveryType = (DeliveryType)model.DeliveryType;

            // удаляем кладр, если варианты доставки поддерживаются
            if (onlineDeliveryVariantsEnabled)
            {
                ModelState.RemoveErrors(k => k == "Address.LocationKladr");
            }

            // не валидируем адрес, если не выбрана доставка курьером
            if (selectedDeliveryType != DeliveryType.Delivery)
            {
                ModelState.RemoveErrors(k => k.StartsWith("Address."));
            }

            // убираем ошибки валидации Email, в зависимости от обязательности
            if (model.IsEmailRequired)
            {
                ModelState.RemoveErrors("Contact.Email");
            }
            else
            {
                ModelState.RemoveErrors("Contact.RequiredEmail");
                ModelState.RemoveErrors("Contact.ConfirmRequiredEmail");
            }

            // предварительная проверка на валидность
            if (!ModelState.IsValid)
            {
                ViewBag.PostBack = model;
                return Items(model.ProductIds);
            }

            try
            {
                var delivery = ParseSelectedDeliveryType(model);

                var orderId = _buy.BeginOrderConfirmation(new BeginOrderConfirmationParams
                {
                    BasketItemIds = model.BasketItemIds.Select(Guid.Parse).ToArray(),
                    Delivery = delivery,
                    TotalPrice = model.BonusPayment,
                    TotalAdvance = model.AdvancePayment
                });

                if (model.Contact.SaveEmail)
                {
                    SaveEmail(model.Contact.RequiredEmail);
                }

                return Confirm(orderId);
            }
            // изменение цены
            catch (OrderPriceChangedException e)
            {
                var refPrice = e.TotalPrice.ToString("N0");
                ModelState["BonusPayment"].Value = new ValueProviderResult(refPrice, refPrice, null);
                var text = string.Format(
                    "При оформлении заказа произошло изменение стоимости. Уточненная стоимость составляет {0}. Продолжить?",
                    Pluralizer.Pluralize(e.TotalPrice, "{1:N0} бонус", "{2:N0} бонуса", "{5:N0} бонусов")
                );
                ModelState.AddModelError("", text);
                ViewBag.PostBack = model;
                return Items(model.ProductIds);
            }
            // ошибка создания заказа
            catch (Exception e)
            {
                model.DisableButton = true;

                if (e is OrderCannotBeDeliveredException)
                {
                    ModelState.AddModelError("", isMultiItem
                        ? "Вознаграждения недоступны для заказа по данному адресу. Приносим свои извинения."
                        : "Вознаграждение недоступно для заказа по данному адресу. Приносим свои извинения.");
                }
                else if (e is OrderItemNotAvailableException || e is OrderCancelledByPartnerException)
                {
                    ModelState.AddModelError("", isMultiItem
                        ? "Заказ этих вознаграждений в данный момент невозможен. Приносим свои извинения."
                        : "Заказ этого вознаграждения в данный момент невозможен. Приносим свои извинения."
                    );
                }
                else if (e is OrderNotFoundException)
                {
                    ModelState.AddModelError("", isMultiItem
                        ? "Часть вознаграждений были удалены из корзины. Приносим свои извинения."
                        : "Вознаграждение было удалено из корзины. Приносим свои извинения."
                    );
                }
                else
                {
                    ModelState.AddModelError("", "Во время выполнения заказа произошла ошибка. Приносим свои извинения.");
                    LogError(e);
                }
            }

            // возвращаем страницу с ошибками
            ViewBag.PostBack = model;
            return Items(model.ProductIds);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DeliveryVariants(string postCode, string kladrCode, string[] basketItems, bool showRurPrice)
        {
            try
            {
                var model = CreateDeliveryVariantsModel(postCode, kladrCode, basketItems, showRurPrice);
                if (model == null || model.Groups == null || !model.Groups.Any())
                {
                    return View("_NoVariants");
                }
                return View("_DeliveryVariants", model);
            }
            catch (BasketItemNotFoundException)
            {
                return View("_ErrorVariants");
            }
            catch (Exception e)
            {
                LogError(e);
                return View("_ErrorVariants");
            }
        }

        [ChildActionOnly]
        public ActionResult Confirm(int orderId)
        {
            var order = _catalog.GetOrder(orderId);

            if (order == null || order.Status != OrderStatus.Registration)
            {
                throw new HttpException(404, "Заказ не найден");
            }

            var model = new ConfirmOtpModel
            {
                OrderDraftId = orderId
            };


            try
            {
                var otp = _buy.SendOrderConfirmationOtp(orderId);
                model.OtpToken = otp.OtpToken;
                model.ExpirationTimeUtc = otp.ExpirationTimeUtc;
                return View("Confirm", model);
            }
            catch (TooFrequentOtpSendException ex)
            {
                ModelState.AddModelError("", ex.Format("подтвердить заказ"));
                model.DisableButton = true;
                return View("Confirm", model);
            }
            catch (TooManyOtpSendAttemptsException ex)
            {
                model.DisableButton = true;
                ModelState.AddModelError("", ex.Format("подтвердить заказ"));
                return View("Confirm", model);
            }
        }

        [Authorize]
        [ClientActivated]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmOtp(ConfirmOtpModel otpModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Confirm", otpModel);
            }

            try
            {
                var optParams = new ConfirmOtpParameters
                {
                    OtpToken = otpModel.OtpToken,
                    Otp = otpModel.Otp
                };
                if (!_otp.Confirm(optParams))
                {
                    ModelState.AddModelError("Otp", "Неверный код подтверждения");
                    return View("Confirm", otpModel);
                }
            }
            catch (OneTimePasswordServiceException e)
            {
                if (OtpController.HandleOtpException(e, ModelState, otpModel))
                {
                    return View("Confirm", otpModel);
                }
                throw;
            }

            if (!_buy.IsAdvancePaymentRequired(otpModel.OrderDraftId))
            {
                var confirmModel = new ConfirmOrderModel
                {
                    OrderDraftId = otpModel.OrderDraftId,
                    OtpToken = otpModel.OtpToken,
                    UserTicket = otpModel.UserTicket
                };

                return RedirectToAction("ConfirmOrder", "Buy", confirmModel);
            }

            if (otpModel.IsIframe)
            {
                LogError("оплата заказа картой для онлайн-поставщиков не поддерживается");
                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }

            var payModel = new PayAdvanceModel
            {
                OrderId = otpModel.OrderDraftId,
                OtpToken = otpModel.OtpToken
            };

            return RedirectToAction("PayAdvance", "Buy", payModel);
        }

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult PayAdvance(PayAdvanceModel model)
        {
            return View("PayAdvance", model);
        }

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult ConfirmOrder(ConfirmOrderModel model)
        {
            try
            {
                // подтверждение заказа
                var confirmParams = new ConfirmOrderParams
                {
                    OrderId = model.OrderDraftId,
                    OtpToken = model.OtpToken
                };

                var order = _buy.ConfirmOrder(confirmParams);

                // отправка сообщения об успешном оформлении заказа
                SendOrderSuccessNotification(_principal.ClientId, order.Id, order.PartnerId);

                if (!model.IsIframe)
                    return RedirectToAction("ThankYou", "Buy");

                var urlParams = GetReturnUrlParameters(model.UserTicket, order);
                var url = _online.GetOrderSuccessUrl(urlParams);
                return Redirect(url);
            }
            catch (OrdersServiceException e)
            {
                if (e is OrderCancelledByPartnerException)
                {
                    LogInfo(e);
                }
                else
                {
                    LogError(e);
                }

                if (!model.IsIframe)
                {
                    return RedirectToAction("Failed", "Buy");
                }

                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }
            catch (BonusPointsException e)
            {
                LogError(e);

                if (!model.IsIframe)
                {
                    return RedirectToAction("Failed", "Buy");
                }

                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }
            catch (PaymentNotAuthorizedException e)
            {
                LogError(e);

                if (!model.IsIframe)
                {
                    return RedirectToAction("Failed", "Buy");
                }

                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }
            catch (AdvancePaymentServiceException e)
            {
                LogError(e);

                if (!model.IsIframe)
                {
                    return RedirectToAction("Failed", "Buy");
                }

                return View("PostCommand", PostCommandModel.ShowBuyFailed());
            }
        }

        // отображается в iframe
        [HttpGet]
        [Interaction("Partner", "PaymentForm")]
        public ActionResult OnlineOrder(OnlineOrderModel model, bool? refreshDisabled)
        {
            var logEntry = (IInteractionLogEntry) RouteData.Values["logEntry"];

            logEntry.Info["PartnerId"] = model.ShopId;
            logEntry.Info["Request"] = Request.Url != null ? Request.Url.Query : null;

            if (!_principal.IsAuthenticated)
            {
                if (refreshDisabled.HasValue && refreshDisabled.Value)
                {
                    var urlParams = GetReturnUrlParameters(model);
                    var url = _online.GetOrderErrorUrl(urlParams, null);

                    logEntry.Info["Response"] = "redirect to " + url;
                    logEntry.NotSucceeded();

                    return Redirect(url);
                }

                return View("PostCommand", PostCommandModel.ReloadOnlineOrderFrame(model));
            }

            if (!ClientActivatedAttribute.IsClientActivated())
            {
                logEntry.Info["Response"] = "client is not activated";
                logEntry.NotSucceeded();

                return View("PostCommand", PostCommandModel.ShowActivationRequired());
            }

            if (!_card.IsCardRegistered())
            {
                logEntry.Info["Response"] = "redirect to card registration";
                logEntry.Succeeded();

                return RedirectToAction("Register", "Card",
                                        new { returnUrl = Url.Action("OnlineOrder", "Buy", model) });
            }

            var options = new BeginOnlineOrderConfirmationParams
            {
                ExternalOrderId = model.OrderId,
                PartnerId = model.ShopId
            };

            try
            {
                var otp = _buy.SendOnlineOrderConfirmationOtp(options);

                var confirmModel = new ConfirmOtpModel
                {
                    OtpToken = otp.OtpToken,
                    ExpirationTimeUtc = otp.ExpirationTimeUtc,
                    OrderDraftId = otp.OrderId,
                    UserTicket = model.UserTicket
                };

                logEntry.Info["Response"] = "form";
                logEntry.Succeeded();

                return View("Confirm", confirmModel);
            }
            catch (TooFrequentOtpSendException ex)
            {
                var urlParams = GetReturnUrlParameters(model);
                var url = _online.GetOrderErrorUrl(urlParams, ex.Format("подтвердить заказ"));

                logEntry.Info["Response"] = "redirect to " + url;
                logEntry.NotSucceeded();

                return Redirect(url);
            }
            catch (TooManyOtpSendAttemptsException ex)
            {
                var urlParams = GetReturnUrlParameters(model);
                var url = _online.GetOrderErrorUrl(urlParams, ex.Format("подтвердить заказ"));

                logEntry.Info["Response"] = "redirect to " + url;
                logEntry.NotSucceeded();

                return Redirect(url);
            }
            catch (NotEnoughPointsException)
            {
                var urlParams = GetReturnUrlParameters(model);
                var url = _online.GetNotEnoughPointsUrl(urlParams);

                logEntry.Info["Response"] = "redirect to " + url;
                logEntry.NotSucceeded();

                return Redirect(url);
            }
        }

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult BankProduct(string bankProductId)
        {
            if (string.IsNullOrEmpty(bankProductId))
            {
                return RedirectToAction("BankProducts", "Catalog");
            }

            // зарегистрирована ли карта?
            if (!_card.IsCardRegistered())
            {
                return View("RegisterCard");
            }

            var orderId = _buy.BeginBankProductOrderConfirmation(bankProductId);

            return ConfirmBankProduct(orderId);
        }

        [ChildActionOnly]
        public ActionResult ConfirmBankProduct(int orderId)
        {
            var order = _catalog.GetOrder(orderId);

            if (order == null || order.Status != OrderStatus.Registration)
            {
                throw new HttpException(404, "Заказ не найден");
            }

            var model = new ConfirmBankProductOtpModel
            {
                OrderDraftId = orderId
            };

            try
            {
                var otp = _buy.SendOrderConfirmationOtp(orderId);
                model.OtpToken = otp.OtpToken;
                model.ExpirationTimeUtc = otp.ExpirationTimeUtc;
                return View("ConfirmBankProduct", model);
            }
            catch (TooFrequentOtpSendException ex)
            {
                ModelState.AddModelError("", ex.Format("подтвердить заказ"));
                model.DisableButton = true;
                return View("ConfirmBankProduct", model);
            }
            catch (TooManyOtpSendAttemptsException ex)
            {
                model.DisableButton = true;
                ModelState.AddModelError("", ex.Format("подтвердить заказ"));
                return View("ConfirmBankProduct", model);
            }
        }

        [Authorize]
        [ClientActivated]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmBankProduct(ConfirmBankProductOtpModel model)
        {
            try
            {
                var optParams = new ConfirmOtpParameters
                {
                    OtpToken = model.OtpToken,
                    Otp = model.Otp
                };

                if (!_otp.Confirm(optParams))
                {
                    ModelState.AddModelError("Otp", "Неверный код подтверждения");
                    return View("ConfirmBankProduct", model);
                }

                // подтверждение заказа
                var confirmParams = new ConfirmOrderParams
                {
                    OrderId = model.OrderDraftId,
                    OtpToken = model.OtpToken
                };

                var order = _buy.ConfirmBankProductOrder(confirmParams);

                // отправка сообщения об успешном оформлении заказа
                SendOrderSuccessNotification(_principal.ClientId, order.Id, order.PartnerId);

                return RedirectToAction("ThankYou", "Buy");
            }
            catch (OneTimePasswordServiceException e)
            {
                if (OtpController.HandleOtpException(e, ModelState, model))
                {
                    return View("ConfirmBankProduct", model);
                }

                LogError(e);
                return RedirectToAction("Failed", "Buy");
            }
            catch (Exception e)
            {
                LogError(e);
                return RedirectToAction("Failed", "Buy");
            }
        }

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult Failed()
        {
            return View("Failed");
        }

        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult ThankYou()
        {
            try
            {
                var mostOrderedTask = Async(() => _catalog.GetPopularProducts(ProductPopularityType.MostOrdered, THANK_YOU_PRODUCTS_COUNT));
                var balanceTask = Async(() => (decimal?)_client.GetBalance());

                Task.WaitAll(mostOrderedTask, balanceTask);

                var mostOrdered = mostOrderedTask.Result;
                var balance = balanceTask.Result;

                var model = new ThankYouModel
                {
                    PopularProductsByOrder = mostOrdered.Select(p => ListProductModel.Map(p, balance)).ToArray()
                };

                return View("ThankYou", model);
            }
            catch (Exception e)
            {
                LogError(e);
                return View("ThankYou", null);
            }
        }


        [Authorize]
        [ClientActivated]
        [HttpGet]
        public ActionResult Edit(int orderDraftId)
        {
            var order = _catalog.GetOrder(orderDraftId);

            if (order == null || order.Status != OrderStatus.Registration)
            {
                throw new HttpException(404, "Заказ не найден");
            }

            return Redirect(Url.Action("Items", "Buy") + "?" + string.Join("&", order.Items.Select(x => "id=" + x.ProductId)));
        }

        #endregion

        private static OrderDeliveryParameters ParseSelectedDeliveryType(BuyModel model)
        {
            var delivery = new OrderDeliveryParameters
            {
                ExternalDeliveryVariantId = model.DeliveryVariantId,
                DeliveryVariantLocation = new DeliveryLocationInfo
                {
                    KladrCode = model.Address.LocationKladr,
                    PostCode = model.Address.PostCode
                },
                Contact = new DeliveryContact
                {
                    Email = model.IsEmailRequired ? model.Contact.RequiredEmail : model.Contact.Email,
                    Phone = PHONE_COUNTRY_CODE + SanitizePhoneNumber(model.Contact.PhoneNumber).Substring(1),
                    FirstName = model.Contact.FirstName,
                    LastName = model.Contact.LastName,
                    MiddleName = model.Contact.MiddleName,
                },
                Comment = !string.IsNullOrWhiteSpace(model.Comment) ? model.Comment : null,
            };

            switch ((DeliveryType)model.DeliveryType)
            {
                case DeliveryType.Delivery:
                    {
                        delivery.Type = DeliveryType.Delivery;
                        delivery.Address = new DeliveryAddress
                        {
                            PostCode = model.Address.PostCode,
                            Region = model.Address.Region,
                            City = model.Address.Location,
                            Street = model.Address.Street,
                            House = model.Address.House,
                            Flat = model.Address.Flat
                        };
                    }
                    break;
                case DeliveryType.Pickup:
                    {
                        delivery.Type = DeliveryType.Pickup;
                        delivery.ExternalPickupPointId = model.PickupPointId;
                    }
                    break;
                case DeliveryType.Email:
                    {
                        delivery.Type = DeliveryType.Email;
                    }
                    break;
                default:
                    {
                        throw new InvalidOperationException("Не удалось определить выбранный тип доставки");
                    }
            }

            return delivery;
        }

        private static string GetReservedProductItemGroupKey(ReservedProductItem reservedProductItem)
        {
            if (reservedProductItem.ReservedProductGroupId == null)
            {
                // для партнёров, не поддерживающих мультипозицию, генерируем уникальные группы для каждой позиции
                return Guid.NewGuid().ToString();
            }

            if (reservedProductItem.Product.IsDeliveredByEmail)
            {
                // для продуктов с доставкой по e-mail, группируем дополнительно по e-mail
                return reservedProductItem.ReservedProductGroupId + "_email";
            }

            return reservedProductItem.ReservedProductGroupId.ToString();
        }

        private CatalogPartnerSettings GetPartnerSettings(int partnerId)
        {
            return _catalog.GetPartners().First(p => p.Id == partnerId).Settings;
        }

        private ContactModel CreateContactModel()
        {
            var profile = _client.GetProfile();
            var phone = profile.Phones.FirstOrDefault();

            return new ContactModel
            {
                Email = profile.Email,
                RequiredEmail = profile.Email,
                FirstName = profile.FirstName,
                MiddleName = profile.MiddleName,
                LastName = profile.LastName,
                PhoneNumber = phone != null ? phone.Number.Substring(1) : string.Empty,
                SaveEmail = true
            };
        }

        private DeliveryAddressModel CreateDeliveryAddressModel()
        {
            var location = _client.GetUserLocation();
            var resolvedLocation = _geo.GetLocationByKladr(location.KladrCode);

            var region = resolvedLocation.GetFullRegionName();
            var regionKladr = resolvedLocation.KladrCode.Substring(0, 2).PadRight(13, '0');
            var locality = resolvedLocation.GetFullName();
            var localityKladr = resolvedLocation.KladrCode;

            return new DeliveryAddressModel
            {
                Region = region,
                RegionKladr = regionKladr,
                Location = locality,
                LocationKladr = localityKladr
            };
        }

        private string GetFormalOfferUrl(int partnerId)
        {
            var offer = _pages.GetOfferPageByPartnerId(partnerId, false);
            if (offer != null && offer.Status == PageStatus.Active)
            {
                return offer.CurrentVersion.Data.Url;
            }
            return null;
        }

        private DeliveryAddressModel[] CreateSavedAddressesModel(bool excludeWithoutKladr)
        {
            return _catalog.GetDeliveryAddresses(excludeWithoutKladr)
                           .Select(a => DeliveryAddressModel.Map(a.Address, a.Location.KladrCode))
                           .ToArray();
        }

        private SelectListItem[] CreateRegionsModel()
        {
            return
                _geo.ListRegions()
                    .Select(r => new SelectListItem { Text = r.GetFullRegionName(), Value = r.KladrCode })
                    .ToArray();
        }

        private DeliveryVariantsModel CreateDeliveryVariantsModel(string postCode, string kladrCode, IEnumerable<string> basketItems, bool showRurPrice)
        {
            var variants = _buy.GetDeliveryVariants(basketItems.Select(Guid.Parse).ToArray(), new DeliveryLocationInfo
            {
                KladrCode = kladrCode,
                PostCode = postCode
            });

            var model = CreateDeliveryVariantsModel(variants);

            model.ShowRurPrice = showRurPrice;

            return model;
        }

        private DeliveryVariantsModel CreateDeliveryVariantsModel(DeliveryVariants variants)
        {
            var groups = variants.Groups.MaybeSelect(DeliveryGroupModel.Map).MaybeToArray();

            var model = new DeliveryVariantsModel
            {
                LocationName = variants.LocationName,
                Groups = groups,
            };

            // если доступен только один вариант доставки -- курьерский -- автоматически его выбираем
            if (groups != null && groups.SelectMany(g => g.Variants).Count() == 1)
            {
                var variant = groups.SelectMany(g => g.Variants).First();
                if (!variant.IsPickup)
                {
                    model.DeliveryVariantId = variant.Id;
                }
            }

            return model;
        }

        private void SendOrderSuccessNotification(string clientId, int orderId, int partnerId)
        {
            const string MESSAGE_TITLE = "Большое спасибо за Ваш заказ!";
            const string MESSAGE_TEMPLATE = @"Уважаемый клиент!

Спасибо за Ваш заказ на сайте Программы «Коллекция». Ваш заказ успешно зарегистрирован. Наш {0} свяжется с Вами в ближайшее время для уточнения деталей доставки. О текущем статусе обработки заказа Вы можете узнать здесь: {1}.

С наилучшими пожеланиями,
Клиентский сервис Программы «Коллекция».";

            try
            {
                var siteUrl = ConfigurationManager.AppSettings["mymessages_site_url"];
                var isEnabledStr = ConfigurationManager.AppSettings["mymessages_send_on_order_success"];
                bool isEnabled;

                if (string.IsNullOrWhiteSpace(siteUrl) ||
                    !bool.TryParse(isEnabledStr, out isEnabled) ||
                    !isEnabled)
                {
                    return;
                }

                var partner = _catalog.GetPartners().FirstOrDefault(p => p.Id == partnerId);
                var partnerName = partner != null
                                      ? "партнер «" + partner.Name + "»"
                                      : "партнер";

                var orderUrl = siteUrl + new UrlHelper(ControllerContext.RequestContext)
                                             .Action("Order", "MyOrders", new { id = orderId });

                var parameters = new NotifyClientsParameters
                {
                    Notifications = new[]
                    {
                        new Notification
                        {
                            ClientId = clientId,
                            Title = MESSAGE_TITLE,
                            Text = string.Format(MESSAGE_TEMPLATE, partnerName, orderUrl)
                        }
                    }
                };

                _clientMessage.Notify(parameters);
            }
            catch (Exception ex)
            {
                LogWarn(ex, new { clientId, orderId });
            }
        }

        private void SaveEmail(string email)
        {
            try
            {
                _client.SetEmail(email);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        private static string SanitizePhoneNumber(string phone)
        {
            return Regex.Replace(phone, "[^\\d]", string.Empty);
        }

        private static ReturnUrlParameters GetReturnUrlParameters(OnlineOrderModel model)
        {
            return new ReturnUrlParameters
            {
                ExternalOrderId = model.OrderId,
                Total = model.MaxDiscount,
                UserTicket = model.UserTicket
            };
        }

        private static ReturnUrlParameters GetReturnUrlParameters(string userTicket, GiftShopOrder order)
        {
            return new ReturnUrlParameters
            {
                ExternalOrderId = order.ExternalId,
                Total = order.TotalPriceRur,
                UserTicket = userTicket
            };
        }
    }
}
