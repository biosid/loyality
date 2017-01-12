using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Extensions;

using RapidSoft.Loaylty.PartnersConnector.Queue.Entities;
using RapidSoft.Loaylty.PartnersConnector.Queue.Repository;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Queue.Repository
{
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class CommitOrderQueueItemRepositoryTests
    {
        private const int PartnerId = -555;

        private const string OrderXml = @"<?xml version=""1.0"" encoding=""utf-16""?>
<Order xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
  <OrderId xmlns=""http://tempuri.org/XMLSchema.xsd"">45</OrderId>
  <TotalWeight xmlns=""http://tempuri.org/XMLSchema.xsd"">0</TotalWeight>
  <ItemsCost xmlns=""http://tempuri.org/XMLSchema.xsd"">2997.0000</ItemsCost>
  <DeliveryCost xmlns=""http://tempuri.org/XMLSchema.xsd"">500.0000</DeliveryCost>
  <TotalCost xmlns=""http://tempuri.org/XMLSchema.xsd"">3497.0000</TotalCost>
  <Items xmlns=""http://tempuri.org/XMLSchema.xsd"">
    <Item>
      <OfferId>5146031</OfferId>
      <OfferName>Prince of Persia: Забытые пески (DVD-BOX)</OfferName>
      <Price>599.4000</Price>
      <Weight>0</Weight>
      <Amount>5</Amount>
    </Item>
  </Items>
  <DeliveryInfo xmlns=""http://tempuri.org/XMLSchema.xsd"">
    <CountryCode>RU</CountryCode>
    <CountryName>Россия</CountryName>
    <PostCode>123123</PostCode>
    <Address>г. Москва, ул. Островетянова, 22б</Address>
    <DeliveryDate>2013-03-21</DeliveryDate>
    <DeliveryTimeFrom>00:00:00.0000000+04:00</DeliveryTimeFrom>
    <DeliveryTimeTo>00:00:00.0000000+04:00</DeliveryTimeTo>
    <Comment>без опозданий</Comment>
    <Contacts>
      <Contact>
        <FirstName>Иван</FirstName>
        <MiddleName>Иванович</MiddleName>
        <LastName>Иванов</LastName>
        <PhoneNumber />
        <Email>a@a.a</Email>
      </Contact>
    </Contacts>
  </DeliveryInfo>
</Order>";

        [TestMethod]
        public void ShouldCreateQueueItem()
        {
            var item = BuildTestItem(PartnerId);

            var repo = new CommitOrderQueueItemRepository();

            var saved = repo.Save(item);

            Assert.IsNotNull(saved);
            Assert.AreNotEqual(saved.Id, default(long));
        }

        [TestMethod]
        public void ShouldReturnListByPartnerId()
        {
            var item = BuildTestItem(PartnerId);

            var repo = new CommitOrderQueueItemRepository();

            var saved = repo.Save(item);

            var list = repo.GetByPartnerId(PartnerId);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any(x => x.Id == saved.Id));

            list = repo.GetByPartnerId(PartnerId, Statuses.NotProcessed);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any(x => x.Id == saved.Id));
        }

        [TestMethod]
        public void ShouldUpdateQueueItem()
        {
            var item = BuildTestItem(PartnerId);

            var repo = new CommitOrderQueueItemRepository();

            var saved = repo.Save(item);

            var update = BuildTestItem(PartnerId);
            update.Id = saved.Id;
            update.ProcessStatus = Statuses.Processed;
            update.ProcessStatusDescription = "Yes!";

            repo.Save(update);

            var list = repo.GetByPartnerId(PartnerId, Statuses.Processed);

            Assert.IsTrue(list.Any(x => x.Id == saved.Id && x.ProcessStatusDescription == "Yes!"));
        }

        [TestMethod]
        public void ShouldUpdateQueueItems()
        {
            var item1 = BuildTestItem(PartnerId);
            var item2 = BuildTestItem(PartnerId);

            var repo = new CommitOrderQueueItemRepository();

            var saved1 = repo.Save(item1);

            saved1.ProcessStatus = Statuses.PartnerConnectError;
            item2.ProcessStatus = Statuses.CatalogPartnerError;

            var listForSave = new List<CommitOrderQueueItem> { saved1, item2 };

            repo.Save(listForSave);

            var list = repo.GetByPartnerId(PartnerId);

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Any(x => x.Id == saved1.Id && x.ProcessStatus == Statuses.PartnerConnectError));
            Assert.IsTrue(list.Any(x => x.Id == item2.Id && x.ProcessStatus == Statuses.CatalogPartnerError));
        }

        private static CommitOrderQueueItem BuildTestItem(int partnerId)
        {
            var item = new CommitOrderQueueItem
                           {
                               ClientId = "ClientId",
                               InsertedDate = DateTime.Now,
                               PartnerId = partnerId,
                               Order = OrderXml,
                               ProcessStatus = Statuses.NotProcessed
                           };
            return item;
        }
    }
}
