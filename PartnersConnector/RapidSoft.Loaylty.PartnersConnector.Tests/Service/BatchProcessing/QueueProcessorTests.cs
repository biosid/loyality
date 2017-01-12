using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.PartnersConnector.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
using RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing;
using RapidSoft.Loyalty.Security;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.BatchProcessing
{
    using Common.DTO.CommitOrder;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    using CommitOrderResult = Common.DTO.CommitOrder.CommitOrderResult;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class QueueProcessorTests
    {
        [TestMethod]
        public void ShouldProcessQueue()
        {
            var partnerId = TestHelper.FirstTestPartnerId;

            const int QueueItemId = -50;
            const int OrderId = -5;
            var order = TestHelper.GetTestDTOOrder(OrderId);
            var item = new CommitOrderQueueItem
                           {
                               ClientId = "Вася",
                               Id = QueueItemId,
                               InsertedDate = DateTime.Now,
                               Order = order.Serialize(Encoding.Unicode),
                               PartnerId = partnerId,
                               ProcessStatus = Statuses.NotProcessed,
                               ProcessStatusDescription = null,
                           };

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();
            mockRepo.Setup(x => x.GetByPartnerId(partnerId, Statuses.NotProcessed))
                    .Returns(new List<CommitOrderQueueItem> { item });

            var mockCatalog = new Mock<ICatalogAdminServiceProvider>();
            var changeOrderStatusResult = new ChangeExternalOrderStatusResult
                {
                    OrderId = OrderId,
                    ExternalOrderId = OrderId.ToString(CultureInfo.InvariantCulture),
                    ResultCode = 0,
                    Success = true,
                };
            var partnerCommitOrdersResult = new PartnerCommitOrdersResult
                {
                    ResultCode = 0,
                    Success = true,
                    ChangeExternalOrderStatusResults = new[] { changeOrderStatusResult }
                };
            mockCatalog.Setup(
                x =>
                x.PartnerCommitOrder(
                    TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()))
                       .Returns(partnerCommitOrdersResult);
            mockCatalog.Setup(x => x.GetPartnerSettings(partnerId, It.IsAny<string>()))
                       .Returns(new Interfaces.Settings.PartnerSettings(partnerId, TestHelper.GetSettings()));

            var mockSender = new Mock<ITextMessageDispatcher>();
            var orderResult = new CommitOrderResult
                                  {
                                      OrderId = OrderId.ToString(CultureInfo.InvariantCulture),
                                      InternalOrderId = "Заказ № " + OrderId,
                                      Confirmed = 1
                                  };
            var ordersResult = new CommitOrdersResult { Orders = new[] { orderResult } };
            mockSender.Setup(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>())).Returns(ordersResult.Serialize(Encoding.UTF8));

            var processor = new QueueProcessor(mockCatalog.Object, mockSender.Object, mockRepo.Object);

            processor.Execute(partnerId);

            mockRepo.Verify(x => x.GetByPartnerId(partnerId, Statuses.NotProcessed), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Once());
            mockSender.Verify(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
            mockCatalog.Verify(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()), Times.Once());
        }
    }
}
