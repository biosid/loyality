
using RapidSoft.VTB24.BankConnector.API.Entities;
using RapidSoft.VTB24.BankConnector.Tests.Helpers;

namespace RapidSoft.VTB24.BankConnector.Tests.ServiceTests
{
    using System.Linq;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.BankConnector.API;
    using RapidSoft.VTB24.BankConnector.API.Exceptions;
    using RapidSoft.VTB24.BankConnector.DataSource;
    using RapidSoft.VTB24.BankConnector.Tests.StubServices;

    [TestClass]
    public class ProfileCustomFieldsTests : TestBase
    {
        private const string UserId = "testuser";

        private const string FieldName1 = "TestCustomField_1";
        private const string FieldName2 = "TestCustomField_2";

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
        }

        [TestMethod]
        public void ShouldAppendCustomField()
        {
            GenericBankConnectorResponse<int> response;

            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                response = client.AppendProfileCustomField(FieldName1, UserId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);
            }

            using (var uow = CreateUow())
            {
                var repository = uow.ProfileCustomFieldsRepository;

                var field = repository.GetAll().SingleOrDefault(f => f.Name == FieldName1);

                Assert.IsNotNull(field);
                Assert.AreEqual(response.Result, field.Id);
            }
        }

        [TestMethod]
        public void ShouldAppendTwoCustomFieldsInOrder()
        {
            GenericBankConnectorResponse<int> response1, response2;

            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                response1 = client.AppendProfileCustomField(FieldName1, UserId);

                Assert.IsNotNull(response1);
                Assert.IsTrue(response1.Success, response1.Error);
                Assert.AreEqual(0, response1.ResultCode);
                Assert.IsNull(response1.Error);

                response2 = client.AppendProfileCustomField(FieldName2, UserId);

                Assert.IsNotNull(response2);
                Assert.IsTrue(response2.Success, response2.Error);
                Assert.AreEqual(0, response2.ResultCode);
                Assert.IsNull(response2.Error);
            }

            using (var uow = CreateUow())
            {
                var repository = uow.ProfileCustomFieldsRepository;

                var field1 = repository.GetAll().SingleOrDefault(f => f.Name == FieldName1);
                var field2 = repository.GetAll().SingleOrDefault(f => f.Name == FieldName2);

                Assert.IsNotNull(field1);
                Assert.AreEqual(response1.Result, field1.Id);

                Assert.IsNotNull(field2);
                Assert.AreEqual(response2.Result, field2.Id);

                Assert.IsTrue(field1.Order < field2.Order);
            }
        }

        [TestMethod]
        public void ShouldRemoveCustomField()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                var fieldId = client.AppendProfileCustomField(FieldName1, UserId).Result;

                var response = client.RemoveProfileCustomField(fieldId, UserId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                using (var uow = CreateUow())
                {
                    var repository = uow.ProfileCustomFieldsRepository;

                    var field = repository.GetAll().SingleOrDefault(f => f.Name == FieldName1);

                    Assert.IsNull(field);
                }

                response = client.RemoveProfileCustomField(fieldId, UserId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);
            }

        }

        [TestMethod]
        public void ShouldRenameCustomField()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                var fieldId = client.AppendProfileCustomField(FieldName1, UserId).Result;

                var response = client.RenameProfileCustomField(fieldId, FieldName2, UserId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);

                using (var uow = CreateUow())
                {
                    var repository = uow.ProfileCustomFieldsRepository;

                    var field = repository.GetAll().SingleOrDefault(f => f.Name == FieldName1);

                    Assert.IsNull(field);

                    field = repository.GetAll().SingleOrDefault(f => f.Name == FieldName2);

                    Assert.IsNotNull(field);
                    Assert.AreEqual(fieldId, field.Id);
                }
            }
        }

        [TestMethod]
        public void ShouldReturnAllCustomFields()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                client.AppendProfileCustomField(FieldName1, UserId);
                client.AppendProfileCustomField(FieldName2, UserId);

                var response = client.GetAllProfileCustomFields(UserId);

                Assert.IsNotNull(response);
                Assert.IsTrue(response.Success, response.Error);
                Assert.AreEqual(0, response.ResultCode);
                Assert.IsNull(response.Error);
                Assert.IsTrue(response.Result.Length >= 2);

                var field1 = response.Result.FirstOrDefault(f => f.Name == FieldName1);
                var field2 = response.Result.FirstOrDefault(f => f.Name == FieldName2);

                Assert.IsNotNull(field1);
                Assert.IsNotNull(field2);
            }
        }

        [TestMethod]
        public void ShouldNotAppendCustomFieldWithDuplicateName()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                client.AppendProfileCustomField(FieldName1, UserId);

                var response = client.AppendProfileCustomField(FieldName1, UserId);

                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.AreEqual(response.ResultCode, (int) ExceptionType.ProfileCustomFieldAlreadyExists);
            }
        }

        [TestMethod]
        public void ShouldNotRenameCustomFieldToExistingName()
        {
            using (var container = DataSourceHelper.CreateContainer())
            {
                var client = container.Resolve<IAdminClientManagementService>();

                var fieldId = client.AppendProfileCustomField(FieldName1, UserId).Result;

                client.AppendProfileCustomField(FieldName2, UserId);

                var response = client.RenameProfileCustomField(fieldId, FieldName2, UserId);

                Assert.IsNotNull(response);
                Assert.IsFalse(response.Success);
                Assert.AreEqual(response.ResultCode, (int) ExceptionType.ProfileCustomFieldAlreadyExists);
            }
        }
    }
}
