namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using VTB24.ArmSecurity;
    using VTB24.ArmSecurity.Interfaces;

    [TestClass]
    public class AdminMechanicsServiceTests
    {
		private readonly List<long> ids = new List<long>();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var mock = new Mock<IUserService>();
            var superUser = TestHelper.BuildSuperUser();
            mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
			TestHelper.BuildPromoActionLinkedToTargetAudiencePredicate();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

		[TestCleanup]
		public void MyTestCleanup()
		{
			TestHelper.DeleteTestRuleDomain(this.ids);
		}
    }
}
