using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using OnlinePartnerStub.Models.Main;
using Vtb24.OnlineCategories.Client;
using Vtb24.OnlineCategories.Client.Exceptions;
using Vtb24.OnlineCategories.Client.Models;

namespace OnlinePartnerStub.Controllers
{
    public class MainController : Controller
    {
        [HttpGet]
        public ActionResult Index(string userTicket)
        {
            try
            {
                var gatewayClient = new BonusGatewayClient();

                var clientInfo = gatewayClient.ResolveClient(userTicket);

                var model = new MainModel
                {
                    UserTicket = userTicket,
                    FullName = clientInfo.LastName + " " + clientInfo.FirstName + " " + clientInfo.MiddleName,
                    Email = clientInfo.Email,
                    City = clientInfo.City,
                    Balance = clientInfo.Balance,
                    BonusRate = clientInfo.BonusRate,
                    BonusDelta = clientInfo.BonusDelta
                };

                return View("Index", model);
            }
            catch (BonusGatewayException ex)
            {
                return View("PartnerError", new PartnerErrorModel
                {
                    Message = "ошибка: " + ex.Message
                });
            }
            catch (Exception ex)
            {
                return View("PartnerError", new PartnerErrorModel
                {
                    Message = "неизвестная ошибка: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult Buy(string userTicket, decimal price, decimal rate, decimal delta)
        {
            var model = new BuyModel
            {
                OrderId = Guid.NewGuid().ToString("D"),
                Price = price,
                BonusPrice = Convert.ToInt32(price * rate + delta),
                UserTicket = userTicket
            };

            return View("Buy", model);
        }

        [HttpPost]
        public ActionResult Buy(BuyModel model)
        {
            try
            {
                var gatewayClient = new BonusGatewayClient();

                var request = new CreateOrderRequest
                {
                    UserTicket = model.UserTicket,
                    OrderId = model.OrderId,
                    TotalCost = model.Price,
                    ItemId = string.Format("StubItemFor{0}", model.Price),
                    ItemName = string.Format("Хороший товар за {0} рублей", model.Price),
                    ItemQuantity = 1,
                    ItemPrice = model.Price,
                    ItemBonusPrice = model.BonusPrice,
                    ItemWeight = 500
                };

                gatewayClient.CreateOrder(request);

                var paymentFormUrl = gatewayClient.CreatePaymentFormUrl(model.UserTicket, model.OrderId, model.Price);

                var confirmOrderModel = new ConfirmOrderModel
                {
                    PaymentFormUrl = paymentFormUrl,
                    Order = new OrderModel
                    {
                        OrderId = model.OrderId,
                        TotalCost = model.Price,
                        UserTicket = model.UserTicket
                    }
                };

                return View("Confirm", confirmOrderModel);
            }
            catch (BonusGatewayException ex)
            {
                return View("PartnerError", new PartnerErrorModel
                {
                    Message = "ошибка: " + ex.Message
                });
            }
            catch (Exception ex)
            {
                return View("PartnerError", new PartnerErrorModel
                {
                    Message = "неизвестная ошибка: " + ex
                });
            }
        }

        [HttpGet]
        public ActionResult Return(ReturnModel model)
        {
            return View("Return", model);
        }

        [HttpGet]
        public ActionResult NotifyOrder(OrderModel model)
        {
            return View("NotifyOrder", model);
        }

        [HttpPost]
        public ActionResult NotifyOrder(NotifyOrderStatusRequest model)
        {
            PartnerErrorModel resultModel;

            try
            {
                var gatewayClient = new BonusGatewayClient();

                gatewayClient.NotifyOrderStatus(model);

                resultModel = new PartnerErrorModel
                {
                    Message = "Сообщение успешно получено"
                };
            }
            catch (BonusGatewayException ex)
            {
                resultModel = new PartnerErrorModel
                {
                    Message = "ошибка: " + ex.Message
                };
            }
            catch (Exception ex)
            {
                resultModel = new PartnerErrorModel
                {
                    Message = "неизвестная ошибка: " + ex
                };
            }

            return View("NotifyOrderResult", resultModel);
        }

        [HttpGet]
        public ActionResult Configuration()
        {
            var model = new[]
            {
                "bonus_gateway::endpoint",
                "bonus_gateway::payment_form_url",
                "bonus_gateway::shop_id",
                "bonus_gateway::private_key",
                "bonus_gateway::public_key"
            }.ToDictionary(key => key, key => ConfigurationManager.AppSettings[key]);

            return View("Configuration", model);
        }

        [HttpGet]
        public ActionResult GetPaymentStatus(string orderId)
        {
            try
            {
                var gatewayClient = new BonusGatewayClient();

                var isPaid = gatewayClient.IsPaid(orderId);

                ViewBag.Message = isPaid
                        ? "Списание баллов по заказу было произведено"
                        : "По данному заказу списание баллов не производилось или списание было отменено";
            }
            catch (BonusGatewayException ex)
            {
                ViewBag.Message = "ошибка: " + ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.Message = "неизвестная ошибка: " + ex;
            }

            return View("GetPaymentStatusResult");
        }
    }
}
