using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    [TestClass]
    public class OrderAttemptsRepositoryTests : TestBase
    {
        const string TEST_CLIENT_ID = "test-client-id";

        [TestMethod]
        public void ShouldClearAll()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.OrderAttemptsRepository;

                repo.ClearAll();
                uow.Save();

                var ids = repo.Get(DateTime.Now.Date, 0, 100);

                Assert.IsTrue(ids.Length == 0);
            }
        }

        [TestMethod]
        public void ShouldSave()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.OrderAttemptsRepository;

                repo.ClearAll();
                uow.Save();

                repo.Save(TEST_CLIENT_ID);
                uow.Save();

                var ids = repo.Get(DateTime.Now.Date, 0, 100);

                Assert.AreEqual(1, ids.Length);
                Assert.AreEqual(TEST_CLIENT_ID, ids[0]);
            }
        }

        [TestMethod]
        public void ShouldSaveTwice()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.OrderAttemptsRepository;

                repo.ClearAll();
                uow.Save();

                repo.Save(TEST_CLIENT_ID);
                uow.Save();
                repo.Save(TEST_CLIENT_ID);
                uow.Save();

                var ids = repo.Get(DateTime.Now.Date, 0, 100);

                Assert.AreEqual(1, ids.Length);
                Assert.AreEqual(TEST_CLIENT_ID, ids[0]);
            }
        }

        [TestMethod]
        public void ShouldSaveAndClear()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.OrderAttemptsRepository;

                repo.ClearAll();
                uow.Save();

                repo.Save(TEST_CLIENT_ID);
                uow.Save();

                repo.Clear(TEST_CLIENT_ID);
                uow.Save();

                var ids = repo.Get(DateTime.Now.Date, 0, 100);

                Assert.IsTrue(ids.Length == 0);
            }
        }

        [TestMethod]
        public void ShouldSaveMany()
        {
            using (var uow = CreateUow())
            {
                var repo = uow.OrderAttemptsRepository;

                repo.ClearAll();
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "1");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "2");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "1");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "3");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "2");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "3");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "1");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "2");
                uow.Save();

                repo.Save(TEST_CLIENT_ID + "3");
                uow.Save();

                var ids = repo.Get(DateTime.Now.Date, 0, 100);

                Assert.AreEqual(3, ids.Length);
                Assert.AreEqual(TEST_CLIENT_ID + "1", ids[0]);
                Assert.AreEqual(TEST_CLIENT_ID + "2", ids[1]);
                Assert.AreEqual(TEST_CLIENT_ID + "3", ids[2]);
            }
        }
    }
}
