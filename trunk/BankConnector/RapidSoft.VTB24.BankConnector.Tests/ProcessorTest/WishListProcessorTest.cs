namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService;

    using RapidSoft.VTB24.BankConnector.Processors;

    using Rapidsoft.Loyalty.NotificationSystem.WsClients.ClientMessageService;

    using Notification = RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService.Notification;
    using ResultBase = RapidSoft.Loaylty.ProductCatalog.WsClients.WishListService.ResultBase;

    [TestClass]
    public class WishListProcessorTest
    {
        [TestMethod]
        public void ShouldCallClientInboxService()
        {
            var wishListMock = new Mock<IWishListService>();

            var notifications = new[]
                                    {
                                        new Notification
                                            {
                                                ClientId = "Geo",
                                                ProductId = "Me3",
                                                ProductName = "Элементарный треугольник",
                                                ProductQuantity = 100500,
                                                FirstName = "Эйншт",
                                                ItemBonusCost = 3.141592654m,
                                                TotalBonusCost = 2.718281828m
                                            },
                                        new Notification
                                            {
                                                ClientId = "A",
                                                ProductId = "str",
                                                ProductName = "Квант темной материи",
                                                ProductQuantity = 0,
                                                FirstName = "Целисий",
                                                MiddleName = "Кельвинович",
                                                ItemBonusCost = 299792458,
                                                TotalBonusCost = 6.67384m
                                            },
                                    };
            int wishListCounter = 0;

            Func<GetWishListNotificationsResult> selector = () =>
            {
                return new GetWishListNotificationsResult
                {
                    ResultCode = 0,
                    Success = true,
                    Notifications =
                        wishListCounter < notifications.Count()
                            ? new[] { notifications[wishListCounter++], }
                            : new Notification[0]
                };
            };

            wishListMock.Setup(s => s.MakeWishListNotifications())
                        .Returns(new ResultBase { Success = true, ResultCode = 0, ResultDescription = "Mock" });
            wishListMock.Setup(s => s.GetWishListNotifications(It.IsAny<GetWishListNotificationsParameters>()))
                        .Returns(selector);

            var clientMessageService = new Mock<IClientMessageService>();

            var sendResult = new List<string>();

            clientMessageService.Setup(s => s.Notify(It.IsAny<NotifyClientsParameters>())).Returns<NotifyClientsParameters>(
                x =>
                {
                    sendResult.AddRange(x.Notifications.Select(y => y.Text));
                    var result = new NotifyClientsResult();
                    result.Success = true;
                    result.ResultCode = 0;
                    result.Threads = x.Notifications.Select(y => new Thread() { ClientId = y.ClientId }).ToArray();
                    return result;
                });

            var wishListProcessor = new WishListProcessor(clientMessageService.Object, wishListMock.Object);

            wishListProcessor.SendWishListNotifications();

            Assert.IsTrue(
                sendResult.Any(
                    x => x.Contains("Эйншт") && x.Contains("Элементарный треугольник")));
            Assert.IsTrue(
                sendResult.Any(
                    x => x.Contains("Целисий") && x.Contains("Кельвинович") && x.Contains("Квант темной материи")));
        }

        [TestMethod]
        public void ShouldSplitItemsOfDifferentNotificationsBatches()
        {
            var wishListMock = new Mock<IWishListService>();

            var notifications = new[]
                                    {
                                        new Notification
                                            {
                                                ClientId = "A",
                                                ProductId = "str",
                                                ProductName = "Квант темной материи",
                                                ProductQuantity = 0,
                                                FirstName = "Целисий",
                                                MiddleName = "Кельвинович",
                                                ItemBonusCost = 299792458,
                                                TotalBonusCost = 6.67384m
                                            },
                                        new Notification
                                            {
                                                ClientId = "A",
                                                ProductId = "temp",
                                                ProductName = "Градус движения",
                                                ProductQuantity = 100,
                                                FirstName = "Целисий",
                                                MiddleName = "Кельвинович",
                                                ItemBonusCost = 32,
                                                TotalBonusCost = 273.16m
                                            }
                                    };

            int wishListCounter = 0;

            Func<GetWishListNotificationsResult> selector = () =>
            {
                return new GetWishListNotificationsResult
                {
                    ResultCode = 0,
                    Success = true,
                    Notifications =
                        wishListCounter < notifications.Count()
                            ? new[] { notifications[wishListCounter++], }
                            : new Notification[0]
                };
            };

            wishListMock.Setup(s => s.MakeWishListNotifications())
                        .Returns(new ResultBase { Success = true, ResultCode = 0, ResultDescription = "Mock" });
            wishListMock.Setup(s => s.GetWishListNotifications(It.IsAny<GetWishListNotificationsParameters>()))
                        .Returns(selector);

            var clientMessageService = new Mock<IClientMessageService>();

            var sendResult = new List<string>();

            clientMessageService.Setup(s => s.Notify(It.IsAny<NotifyClientsParameters>())).Returns<NotifyClientsParameters>(
                x =>
                {
                    sendResult.AddRange(x.Notifications.Select(y => y.Text));
                    var result = new NotifyClientsResult();
                    result.Success = true;
                    result.ResultCode = 0;
                    result.Threads = x.Notifications.Select(y => new Thread() { ClientId = y.ClientId }).ToArray();
                    return result;
                });

            var wishListProcessor = new WishListProcessor(clientMessageService.Object, wishListMock.Object);

            wishListProcessor.SendWishListNotifications();

            Assert.IsTrue(
                sendResult.Any(
                    x =>
                    x.Contains("Целисий") && x.Contains("Кельвинович") && x.Contains("Квант темной материи")
                    && x.Contains("Градус движения") && x.Contains(32.ToString("N0")) && x.Contains(299792458.ToString("N0"))));
        }
    }
}
