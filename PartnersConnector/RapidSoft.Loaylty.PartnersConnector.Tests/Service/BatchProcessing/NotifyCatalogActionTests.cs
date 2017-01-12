using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.PartnersConnector.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
using RapidSoft.Loaylty.PartnersConnector.Services;
using RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.BatchProcessing
{
    using Common.Services;

    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    using CommitOrderResult = Common.DTO.CommitOrder.CommitOrderResult;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class NotifyCatalogActionTests
    {
        static NotifyCatalogActionTests()
        {
            var service = new OrderManagementService();
        }

        [TestMethod]
        public void ShouldProcessBatchWithConfirmedResults()
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
            var commitOrderResult = new CommitOrderResult
                                        {
                                            OrderId = orderId.ToString(CultureInfo.InvariantCulture),
                                            InternalOrderId = "Заказ № " + orderId,
                                            Confirmed = 1
                                        };

            var batchItem = new BatchItem(item) { CommitOrderResult = commitOrderResult };

            var batch = new Batch(TestHelper.TestPartnerID, new[] { batchItem });

            var mockCatalog = new Mock<ICatalogAdminServiceProvider>();
            var changeOrderStatusResult = new ChangeExternalOrderStatusResult
                                        {
                                            OrderId = orderId,
                                            ExternalOrderId = orderId.ToString(CultureInfo.InvariantCulture),
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
                x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()))
                       .Returns<int, PartnerOrderCommitment[], string>(
                           (partnerID, commitments, userId) =>
                               {
                                   Assert.IsTrue(
                                       commitments.All(x => x.ExternalOrderId != null),
                                       "Каждый заказ при подтверждении должен передать InternalOrderId");
                                   return partnerCommitOrdersResult;
                               });

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new NotifyCatalogAction(mockCatalog.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.IsTrue(resultBatch.Any(x => x.CommitOrderQueueItem.Id == queueItemId && x.ChangeOrderStatusResult.Success));
            mockCatalog.Verify(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Never());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldSkipItemWithoutCommitOrderResult()
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

            var batch = new Batch(TestHelper.TestPartnerID, new[] { item });

            var mockCatalog = new Mock<ICatalogAdminServiceProvider>();

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new NotifyCatalogAction(mockCatalog.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.IsTrue(!resultBatch.Any());
            mockCatalog.Verify(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()), Times.Never());
            mockRepo.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Never());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Never());
        }

        [TestMethod]
        public void ShouldRemoveItemFromBatchCauseCatalogDontSendOrderRespond()
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
            var commitOrderResult = new CommitOrderResult
            {
                OrderId = orderId.ToString(CultureInfo.InvariantCulture),
                InternalOrderId = "Заказ № " + orderId,
                Confirmed = 1
            };

            var batchItem = new BatchItem(item) { CommitOrderResult = commitOrderResult };

            var batch = new Batch(TestHelper.TestPartnerID, new[] { batchItem });

            var mockCatalog = new Mock<ICatalogAdminServiceProvider>();
            var changeOrderStatusResult = new ChangeExternalOrderStatusResult
            {
                OrderId = orderId - 150, // <== Ответ получаем, но не по нашему заказу.
                ExternalOrderId = orderId.ToString(CultureInfo.InvariantCulture),
                ResultCode = 0,
                Success = true,
            };
            var partnerCommitOrdersResult = new PartnerCommitOrdersResult
            {
                ResultCode = 0,
                Success = true,
                ChangeExternalOrderStatusResults = new[] { changeOrderStatusResult }
            };

            mockCatalog.Setup(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()))
                       .Returns(partnerCommitOrdersResult);

            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new NotifyCatalogAction(mockCatalog.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.IsTrue(!resultBatch.Any());
            mockCatalog.Verify(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<CommitOrderQueueItem>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Never());
        }

    }
}
