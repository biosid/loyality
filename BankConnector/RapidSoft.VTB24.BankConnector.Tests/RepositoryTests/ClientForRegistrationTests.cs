using Faker;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.VTB24.BankConnector.DataModels;
using RapidSoft.VTB24.BankConnector.DataSource;
using RapidSoft.VTB24.BankConnector.DataSource.Repository;
using System;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using Microsoft.Practices.Unity;

    [TestClass]
    public class ClientForRegistrationTests : TestBase
	{
		private readonly Guid testSessionId = new Guid("B1B96FC1-E344-4BD9-B10D-F5342C2F47D5");
		private readonly Guid testSessionId2 = new Guid("8353F751-1519-4B3E-8714-8499A587A824");

		private const string testClientId = "test_client_id";
		private const string testClientId2 = "test_client_id2";

		[TestInitialize]
		public void RefreshTestData()
		{
			using (var uow = CreateUow())
			{
			    var repository = uow.ClientForRegistrationRepository;
                repository.DeleteBySessionId(testSessionId);
                repository.DeleteBySessionId(testSessionId2);
                var trash = repository.GetById(x => x.ClientId == testClientId);
				if (trash != null)
				{
                    repository.Delete(trash);
				}
				uow.Save();
			}
		}

		[TestMethod]
		public void ShouldAddCorrectInsertedDate()
		{
			using (var uow = CreateUow())
			{
				var repository = uow.ClientForRegistrationRepository;

				var clientForRegistration = GetTestClientDBRecord(testClientId);
				clientForRegistration.RequestSessionId = testSessionId;
				clientForRegistration.ResponseSessionId = testSessionId2;

				repository.Add(clientForRegistration);
				uow.Save();
			}

			using (var uow = CreateUow())
			{
				var repository = uow.ClientForRegistrationRepository;

				var clientForRegistration = repository.GetById(x => x.ClientId == testClientId);

				Assert.IsNotNull(clientForRegistration);
				Assert.IsTrue((DateTime.Now - clientForRegistration.InsertedDate) < new TimeSpan(0, 0, 5));
			}
		}

		private ClientForRegistration GetTestClientDBRecord(string clientId)
		{
			var random = new Random();
			var phone = "7" + random.Next(100000, 999999) + random.Next(1000, 9999);
			return new ClientForRegistration
			{
				ClientId = clientId,
				Email = Internet.Email(),
				FirstName = Name.First(),
				LastName = Name.Last(),
				MiddleName = "X",
				MobilePhone = phone,
				BirthDate = new DateTime(2013, 6, 15).AddYears(-random.Next(20, 50)),
			};
		}

	}
}
