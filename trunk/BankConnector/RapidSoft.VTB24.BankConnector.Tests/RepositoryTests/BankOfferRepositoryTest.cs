using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Quartz.Util;
using RapidSoft.VTB24.BankConnector.DataModels;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    [TestClass]
    public class BankOfferRepositoryTest : TestBase
    {
        private const string testOfferId = "TestOffer";
        private string testOfferClientId;
        private int ordersForClient;
        
        private DateTime? insertedDate;


        [TestInitialize]
        public void PrepareTestData()
        {
            testOfferClientId = Guid.NewGuid().ToString("N");
            var testOffer = new BankOffer()
            {
                Id = testOfferId,
                Description = "Bank offer for unit test",
                BonusCost = 2000,
                ProductId = "Test product id",
                ExpirationDate = DateTime.Now.AddDays(10),
                ClientId = testOfferClientId,
                OfferId = "SMS",
                CardLast4Digits = "1234",
                Status = BankOfferStatus.Active
            };

            using (var uow = CreateUow())
            {
                var repo = uow.BankOffersRepository;
                repo.Add(new[] { testOffer });
                uow.Save();
            }
            insertedDate = testOffer.InsertedDate;
            ordersForClient = 1;
        }

        [TestCleanup]
        public void RemoveData()
        {
            using (var uow = CreateUow())
            {
                var offer = uow.Context.BankOffers.FirstOrDefault(bo => bo.Id == testOfferId);
                if (offer != null)
                {
                    uow.Context.BankOffers.Remove(offer);
                }
                uow.Save();
            }
        }

        [TestMethod]
        public void CanAddAndRetrieveOffer()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.BankOffersRepository;
                int total;
                var actual = repo.GetOffers(testOfferId, null, null, 0, 1, false, out total);
                Assert.AreEqual(1, actual.Count);
                Assert.AreEqual(testOfferId, actual[0].Id);
                Assert.AreEqual(insertedDate, actual[0].InsertedDate);
            }
        }

        [TestMethod]
        public void CanFindOrdersForClient()
        {
            using (var uow = CreateUow())
            {
                int total;
                var repo = uow.BankOffersRepository;
                var find = repo.GetOffers(string.Empty, testOfferClientId, DateTime.Now.AddDays(3), 0, 10, true, out total);
                Assert.IsNotNull(find);
                Assert.AreEqual(total, find.Count);
                var offer = find.FirstOrDefault();
                Assert.IsNotNull(offer);
                Assert.AreEqual(ordersForClient, total);
                Assert.AreEqual(testOfferId, offer.Id);
            }
        }

        [TestMethod]
        public void Test()
        {
            using (var uow = CreateUow())
            {
                var sessionId = Guid.NewGuid();

                var query = uow.RegisterBankOffersRepository
                               .GetBySessionId(sessionId)
                               .GroupJoin(uow.RegisterBankOffersResponseRepository.GetBySessionId(sessionId),
                                          rbo => rbo.PartnerOrderNum,
                                          rbor => rbor.PartnerOrderNum,
                                          (rbo, rbor) => new { rbo, rbor = rbor.FirstOrDefault() })
                               .Where(bo => bo.rbor == null)
                               .Select(bo => bo.rbo);

                var result = query.ToArray();
            }
        }
    }
}
