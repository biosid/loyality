using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests.ServiceTests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.Tests.StubServices;

    [TestClass]
    public class UpdateClientPhoneTests : TestBase
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var service = new ArmSecurityStub();
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestMethod]
        public void ShouldUpdateClientPhoneNumber()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                var request = new UpdateClientPhoneNumberRequest
                {
                    ClientId = "Success",
                    PhoneNumber = "79261234567",
                    UserId = "rapidsoft"
                };

                var response = client.UpdateClientPhoneNumber(request);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);
            }
        }
    }
}
