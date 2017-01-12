using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests.ServiceTests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Entities;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Tests.StubServices;

    [TestClass]
    public class ProfileCustomFieldsValuesTests : TestBase
    {
        private const string UserId = "testuser";

        private const string ClientId = "vtb1";

        private const string FieldName1 = "TestCustomField_1";
        private const string FieldName2 = "TestCustomField_2";

        private int fieldId1;
        private int fieldId2;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var service = new ArmSecurityStub();
            ArmSecurity.UserServiceCreator = () => service;
        }

        [TestInitialize]
        public void RefreshTestData()
        {
            var fieldNames = new[] { FieldName1, FieldName2 };

            using (var uow = CreateUow())
            {
                var repository = uow.ProfileCustomFieldsRepository;

                var fields = repository.GetAll().Where(f => fieldNames.Contains(f.Name)).ToList();
                fields.ForEach(repository.Delete);

                uow.Save();
            }

            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();
                fieldId1 = client.AppendProfileCustomField(FieldName1, UserId).Result;
                fieldId2 = client.AppendProfileCustomField(FieldName2, UserId).Result;
            }
        }

        [TestMethod]
        public void ShouldReturnCustomFields()
        {
            using (var uow = CreateUow())
            {
                var values = uow.ProfileCustomFieldsValuesRepository.GetByClientId("vtb1");

                Assert.IsNotNull(values);
            }
        }

        [TestMethod]
        public void ShouldReturnCustomFieldsInProfile()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IClientManagementService>();

                var response = client.GetClientProfile(ClientId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                Assert.IsNotNull(response.Result);
                Assert.IsNotNull(response.Result.CustomFields);
                Assert.IsTrue(response.Result.CustomFields.Length >= 2);
            }
        }

        [TestMethod]
        public void ShouldSetProfileCustomFields()
        {
            const string FieldValue1 = "value1";
            const string FieldValue2 = "value2";

            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IClientManagementService>();

                var request = new UpdateClientRequest
                {
                    ClientId = ClientId,
                    UpdateProperties = new[] { UpdateProperty.CustomFields },
                    CustomFields =
                        new Dictionary<int, string>
                        {
                            { fieldId1, FieldValue1 },
                            { fieldId2, FieldValue2 }
                        }
                };

                var response = client.UpdateClient(request);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                var profileResponse = client.GetClientProfile(ClientId);

                Assert.IsNotNull(profileResponse);
                Assert.IsTrue(profileResponse.Success, profileResponse.Error);
                Assert.AreEqual(0, profileResponse.ResultCode);
                Assert.IsNull(profileResponse.Error);

                Assert.IsNotNull(profileResponse.Result);
                Assert.IsNotNull(profileResponse.Result.CustomFields);

                var customFields = profileResponse.Result.CustomFields;

                Assert.IsTrue(customFields.Length >= 2);

                Assert.AreEqual(customFields.First(f => f.Id == fieldId1).Value, FieldValue1);
                Assert.AreEqual(customFields.First(f => f.Id == fieldId2).Value, FieldValue2);
            }
        }

        [TestMethod]
        public void ShouldSetAndUpdateProfileCustomField()
        {
            const string FieldValue1 = "value1";
            const string FieldValue2 = "value2";

            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IClientManagementService>();

                var request = new UpdateClientRequest
                {
                    ClientId = ClientId,
                    UpdateProperties = new[] { UpdateProperty.CustomFields },
                    CustomFields =
                        new Dictionary<int, string>
                        {
                            { fieldId1, FieldValue1 }
                        }
                };

                var response = client.UpdateClient(request);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                request = new UpdateClientRequest
                {
                    ClientId = ClientId,
                    UpdateProperties = new[] { UpdateProperty.CustomFields },
                    CustomFields =
                        new Dictionary<int, string>
                        {
                            { fieldId1, FieldValue2 }
                        }
                };

                response = client.UpdateClient(request);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                var profileResponse = client.GetClientProfile(ClientId);

                Assert.IsNotNull(profileResponse);
                Assert.IsTrue(profileResponse.Success, profileResponse.Error);
                Assert.AreEqual(0, profileResponse.ResultCode);
                Assert.IsNull(profileResponse.Error);

                Assert.IsNotNull(profileResponse.Result);
                Assert.IsNotNull(profileResponse.Result.CustomFields);

                var customFields = profileResponse.Result.CustomFields;

                Assert.IsTrue(customFields.Length >= 2);

                Assert.AreEqual(customFields.First(f => f.Id == fieldId1).Value, FieldValue2);
            }
        }
    }
}
