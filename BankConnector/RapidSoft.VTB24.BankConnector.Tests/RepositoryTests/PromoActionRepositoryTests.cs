using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Repository;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using System.Linq;

    using Microsoft.Practices.Unity;

    [TestClass]
    public class PromoActionRepositoryTests : TestBase
    {
        [TestMethod]
        public void ShouldReturnMaxIndexByDate()
        {
            var date = DateTime.Now.Date.AddDays(5000); // Заведомо нет импортов.

            using (var uow = CreateUow())
            {
                var repo = uow.PromoActionRepository;

                var index = repo.GetAll().Where(x => x.DateSent == date).Select(x => x.IndexSent).DefaultIfEmpty(0).Max();

                Assert.AreEqual(0, index, "На заведомо большую дату (" + date + ") не должно быть имопртов.");
            }
        }
    }
}
