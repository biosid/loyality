using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Entities;
using RapidSoft.Loaylty.PartnersConnector.Litres.DataAccess.Repositories;

namespace RapidSoft.Loaylty.PartnersConnector.Tests.Litres
{
    [TestClass]
    public class LitresTests
    {
        private const string PRODUCT_ID = "TestProductId";
        private const string CODE_PREFFIX = "TestCode";

        private void PrepareTestCodes()
        {
            using (var context = new LitresContext())
            {
                // удалим старые коды
                var codes = context.LitresDownloadCodes.Where(c => c.PartnerProductId == PRODUCT_ID).ToArray();

                foreach (var code in codes)
                {
                    context.LitresDownloadCodes.Remove(code);
                }

                context.SaveChanges();

                // добавим новые
                foreach (var i in Enumerable.Range(0, 10))
                {
                    context.LitresDownloadCodes.Add(new LitresDownloadCode
                    {
                        PartnerProductId = PRODUCT_ID,
                        Code = CODE_PREFFIX + i,
                        InsertedDate = DateTime.Now
                    });
                }

                context.SaveChanges();
            }
        }

        [TestMethod]
        public void ShouldBindCodes()
        {
            // подготовка
            PrepareTestCodes();

            //тест
            using (var repo = new LitresDownloadCodesRepository())
            {
                repo.BindCodes(PRODUCT_ID, 3, 1);

                repo.Save();

                repo.BindCodes(PRODUCT_ID, 5, 2);

                repo.Save();
            }

            // проверка
            using (var context = new LitresContext())
            {
                Assert.AreEqual(3, context.LitresDownloadCodes.Count(c => c.PartnerProductId == PRODUCT_ID && c.OrderId == 1));
                Assert.AreEqual(5, context.LitresDownloadCodes.Count(c => c.PartnerProductId == PRODUCT_ID && c.OrderId == 2));
                Assert.AreEqual(2, context.LitresDownloadCodes.Count(c => c.PartnerProductId == PRODUCT_ID && c.OrderId == null));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfPartnerProductIdIsNull()
        {
            using (var repo = new LitresDownloadCodesRepository())
            {
                repo.BindCodes(null, 1, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfPartnerProductIdIsEmpty()
        {
            using (var repo = new LitresDownloadCodesRepository())
            {
                repo.BindCodes(string.Empty, 1, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowIfCountIdNonPositive()
        {
            using (var repo = new LitresDownloadCodesRepository())
            {
                repo.BindCodes(PRODUCT_ID, -1, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ShouldThrowIfOutOfCodes()
        {
            PrepareTestCodes();

            using (var repo = new LitresDownloadCodesRepository())
            {
                repo.BindCodes(PRODUCT_ID, 11, 1);
            }
        }
    }
}
