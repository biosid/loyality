using Microsoft.VisualStudio.TestTools.UnitTesting;
using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Repository;
using System;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using Microsoft.Practices.Unity;

    [TestClass]
    public class OrderPaymentResponseTests : TestBase
    {
        private const string TestSessionId = "CCBB2B68-CB0F-4281-807B-AC76974099EA";

        [TestInitialize]
        public void RefreshTestData()
        {
            using (var uow = CreateUow())
            {
                uow.OrderPaymentResponseRepository.DeleteBySessionId(Guid.Parse(TestSessionId));                
                uow.Save();
            }
        }

        [TestMethod]
        public void AddOrdersForPaymentTest()
        {
			var order = new OrderPaymentResponse { OrderId = 1, Status = (int)ReceivedOrderStatus.Approved, EtlSessionId = Guid.Parse(TestSessionId) };

            using (var uow = CreateUow())
            {
                uow.OrderPaymentResponseRepository.Add(order);                
                uow.Save();
            }
        }
    }
}
