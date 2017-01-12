using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RapidSoft.Etl.Logging;
using RapidSoft.VTB24.BankConnector.Processors;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;
using RapidSoft.VTB24.Site.SecurityWebApi;

namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    [TestClass]
    public class BankOffersProcessorTest : TestBase
    {
        [TestMethod]
        public void ProcessBankOffersTest()
        {
            using (var uow = CreateUow())
            {                
                var logger = new EtlLogger.EtlLogger(new Mock<IEtlLogger>().Object, "stub", Guid.NewGuid().ToString());
                var secMock = new Mock<ISecurityWebApi>();
                var processor = new BankOffersProcessor(logger, uow, secMock.Object);
                processor.ProcessBankOffers();
                Assert.AreEqual(1,1);
            }
        }
    }
}
