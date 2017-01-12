using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PromoAction.Tests.AdminServices
{
    using System.Linq;

    using Moq;

    using RapidSoft.Loaylty.PromoAction.Service;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [TestClass]
    public class AdminMechanicsServiceTests
    {
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var mock = new Mock<IUserService>();
            var superUser = TestHelper.BuildSuperUser();
            mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestMethod]
        public void ShouldReturnMetadata()
        {
            var service = new AdminMechanicsService();

            var domains = service.GetAllRuleDomains(TestDataStore.TestUserId);

            Assert.IsNotNull(domains);
            Assert.AreEqual(true, domains.Success);
            var first = domains.RuleDomains.FirstOrDefault();
            Assert.IsNotNull(first);

            var meta = service.GetMetadataByDomainId(first.Id, TestDataStore.TestUserId);
            Assert.IsNotNull(meta);
            Assert.AreEqual(true, meta.Success);
        }
    }
}
