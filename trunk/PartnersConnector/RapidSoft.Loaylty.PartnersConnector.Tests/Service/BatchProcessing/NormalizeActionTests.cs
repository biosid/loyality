using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using RapidSoft.Extensions;

using RapidSoft.Loaylty.PartnersConnector.Interfaces;
using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;
using RapidSoft.Loaylty.PartnersConnector.Services.BatchProcessing;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Service.BatchProcessing
{
    using RapidSoft.Loaylty.ProductCatalog.WsClients.CatalogAdminService;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class NormalizeActionTests
    {
        [TestMethod]
        public void ShouldRemoveOrderWithEqualsId()
        {
            const int QueueItemId = -50;
            const int OrderId = -5;
            var order1 = TestHelper.GetTestDTOOrder(OrderId);
            var item1 = new CommitOrderQueueItem
                            {
                                ClientId = "Вася",
                                Id = QueueItemId,
                                InsertedDate = DateTime.Now,
                                Order = order1.Serialize(Encoding.Unicode),
                                PartnerId = TestHelper.TestPartnerID,
                                ProcessStatus = Statuses.NotProcessed,
                                ProcessStatusDescription = null,
                            };

            var order2 = TestHelper.GetTestDTOOrder(OrderId - 1);
            var item2 = new CommitOrderQueueItem
                            {
                                ClientId = "Вася",
                                Id = QueueItemId,
                                InsertedDate = DateTime.Now,
                                Order = order2.Serialize(Encoding.Unicode),
                                PartnerId = TestHelper.TestPartnerID,
                                ProcessStatus = Statuses.NotProcessed,
                                ProcessStatusDescription = null,
                            };
            var items = new[] { item1, item2, item1 };

            var batch = new Batch(TestHelper.TestPartnerID, items);

            var mockCatalog = new Mock<ICatalogAdminServiceProvider>();
            var mockRepo = new Mock<ICommitOrderQueueItemRepository>();

            var action = new NormalizeAction(mockCatalog.Object, mockRepo.Object);

            var resultBatch = action.Execute(batch);

            Assert.AreEqual(resultBatch.Count, 1);
            mockCatalog.Verify(x => x.PartnerCommitOrder(TestHelper.TestPartnerID, It.IsAny<PartnerOrderCommitment[]>(), It.IsAny<string>()), Times.Once());
            mockRepo.Verify(x => x.Save(It.IsAny<IList<CommitOrderQueueItem>>()), Times.Once());
        }
    }
}
