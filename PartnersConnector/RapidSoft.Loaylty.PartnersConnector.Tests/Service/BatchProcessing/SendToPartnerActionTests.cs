using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
using RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing;
using RapidSoft.Loyalty.Security;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.BatchProcessing
{
    using Common.DTO.CommitOrder;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class SendToPartnerActionTests
    {
        [TestMethod]
        public void ShouldProcessBatchWithConfirmedResult()
        {
            int queueItemId = -50;
            int orderId = -5;
            var order = TestHelper.GetTestDTOOrder(orderId);
            var item = new CommitOrderQueueItem
                           {
                               ClientId = "Вася",
                               Id = queueItemId,
                               InsertedDate = DateTime.Now,
                               Order = order.Serialize(Encoding.Unicode),
                               PartnerId = TestHelper.TestPartnerID,
                               ProcessStatus = Statuses.NotProcessed,
                               ProcessStatusDescription = null,
                           };
            var items = new[] { item };

            var batch = new Batch(TestHelper.TestPartnerID, items);

            var orderResult = new CommitOrderResult
                                  {
                                      OrderId = orderId.ToString(CultureInfo.InvariantCulture),
                                      InternalOrderId = "Заказ № " + orderId,
                                      Confirmed = 1
                                  };
            var ordersResult = new CommitOrdersResult { Orders = new[] { orderResult } };
            var serializedResult = ordersResult.Serialize(Encoding.UTF8);

            var mockSender = new Mock<ITextMessageDispatcher>();
            mockSender.Setup(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>())).Returns(serializedResult);

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new SendToPartnerAction(new Uri("https://localhost"), mockSender.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.IsTrue(resultBatch.Any(x => x.CommitOrderQueueItem.Id == queueItemId && x.CommitOrderResult.Confirmed == 1));
            mockSender.Verify(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Never());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldRemoveItemFromBatchCausePartnerDontSendOrderRespond()
        {
            int queueItemId = -50;
            int orderId = -5;
            var order = TestHelper.GetTestDTOOrder(orderId);
            var item = new CommitOrderQueueItem
            {
                ClientId = "Вася",
                Id = queueItemId,
                InsertedDate = DateTime.Now,
                Order = order.Serialize(Encoding.Unicode),
                PartnerId = TestHelper.TestPartnerID,
                ProcessStatus = Statuses.NotProcessed,
                ProcessStatusDescription = null,
            };
            var items = new[] { item };

            var batch = new Batch(TestHelper.TestPartnerID, items);

            var orderResult = new CommitOrderResult
            {
                OrderId = (orderId - 150).ToString(CultureInfo.InvariantCulture), // <== Ответ получаем, но не по нашему заказу.
                InternalOrderId = "Заказ № " + orderId,
                Confirmed = 1
            };
            var ordersResult = new CommitOrdersResult { Orders = new[] { orderResult } };
            var serializedResult = ordersResult.Serialize(Encoding.UTF8);

            var mockSender = new Mock<ITextMessageDispatcher>();
            mockSender.Setup(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>())).Returns(serializedResult);

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new SendToPartnerAction(new Uri("https://localhost"), mockSender.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.IsTrue(!resultBatch.Any());
            mockSender.Verify(x => x.Send(It.IsAny<Uri>(), It.IsAny<string>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Never());
        }
    }
}
