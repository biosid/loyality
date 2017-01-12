using RapidSoft.VTB24.BankConnector.DataSource.Repository;

namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using RapidSoft.VTB24.BankConnector.DataModels;
	using RapidSoft.VTB24.BankConnector.DataSource;

    [TestClass]
    public class ClientForActivationTests : TestBase
	{
		private readonly Guid testSessionId = new Guid("B1B96FC1-E344-4BD9-B10D-F5342C2F47D5");
		private readonly Guid testSessionId2 = new Guid("8353F751-1519-4B3E-8714-8499A587A824");

		private const string testClientId = "test_client_id";

		[TestInitialize]
		public void RefreshTestData()
		{
			using (var uow = CreateUow())
			{
			    var repository = uow.ClientForActivationRepository;
                repository.DeleteBySessionId(testSessionId);
                repository.DeleteBySessionId(testSessionId2);
                repository.DeleteByClientId(testClientId);
				uow.Save();
			}
		}

		[TestMethod]
		public void AddClientForActivationTest()
		{
			var client = new ClientForActivation
				             {
					             BirthDate = DateTime.Now.AddYears(-25),
					             ClientId = testClientId,
					             EtlSessionId = testSessionId,
					             FirstName = Path.GetRandomFileName(),
					             LastName = Path.GetRandomFileName(),
					             Gender = 0,
					             MiddleName = Path.GetRandomFileName()
				             };
			ClientForActivation recordFromBase = null;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
                repository.Add(client);
				uow.Save();
                recordFromBase = repository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
			}

			Assert.IsNotNull(recordFromBase);
			Assert.AreEqual(testSessionId, recordFromBase.EtlSessionId);
			Assert.AreEqual(testClientId, recordFromBase.ClientId);
			Assert.AreEqual(client.Id, recordFromBase.Id);
		}

		[TestMethod]
		public void DeleteClientForActivationTest()
		{
			var client = new ClientForActivation
			{
				BirthDate = DateTime.Now.AddYears(-25),
				ClientId = testClientId,
				EtlSessionId = testSessionId,
				FirstName = Path.GetRandomFileName(),
				LastName = Path.GetRandomFileName(),
				Gender = 0,
				MiddleName = Path.GetRandomFileName()
			};
			ClientForActivation recordFromBaseExists = null;
			ClientForActivation recordNotExists = null;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
                repository.Add(client);
				uow.Save();
                recordFromBaseExists = repository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
                repository.Delete(
					x => x.EtlSessionId == testSessionId && x.ClientId == testClientId);
				uow.Save();
                recordNotExists = repository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
			}

			Assert.IsNotNull(recordFromBaseExists);
			Assert.IsNull(recordNotExists);
		}

		[TestMethod]
		public void UpdateSingleRecordTest()
		{
			var client = new ClientForActivation
			{
				BirthDate = DateTime.Now.AddYears(-25),
				ClientId = testClientId,
				EtlSessionId = testSessionId,
				FirstName = Path.GetRandomFileName(),
				LastName = Path.GetRandomFileName(),
				Gender = 0,
				MiddleName = Path.GetRandomFileName()
			};
			ClientForActivation recordFromBaseOld = null;
			ClientForActivation recordFromBaseNew = null;
			var newFirstName = Path.GetRandomFileName();
			string oldFirstName = null;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
                repository.Add(client);
				uow.Save();
                recordFromBaseOld = repository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
				oldFirstName = recordFromBaseOld.FirstName;
			}

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;

				client.FirstName = newFirstName;

                repository.Update(client);
				uow.Save();
                recordFromBaseNew = repository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
			}

			Assert.IsNotNull(recordFromBaseNew);
			Assert.AreNotEqual(oldFirstName, recordFromBaseNew.FirstName);
			Assert.AreEqual(recordFromBaseNew.FirstName, newFirstName);
		}

		[TestMethod]
		public void GetCorrectUniqueClientRecordTestReturnUnique()
		{
			var client = new ClientForActivation
			{
				BirthDate = DateTime.Now.AddYears(-25),
				ClientId = testClientId,
				EtlSessionId = testSessionId,
				FirstName = Path.GetRandomFileName(),
				LastName = Path.GetRandomFileName(),
				Gender = 0,
				MiddleName = Path.GetRandomFileName()
			};

			List<ClientForActivation> recordFromBase;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
				repository.Add(client);
				uow.Save();
			}

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
                recordFromBase = repository.GetUniqueClientIdBySession(testSessionId).ToList();
			}

			Assert.IsNotNull(recordFromBase);
            Assert.AreEqual(1, recordFromBase.Count());
            Assert.AreEqual(client.ClientId, recordFromBase.First().ClientId);
            Assert.AreEqual(client.FirstName, recordFromBase.First().FirstName);
            Assert.AreEqual(client.EtlSessionId, recordFromBase.First().EtlSessionId);
		}

		[TestMethod]
		public void ShouldNotReturnRecordForAlreadyActivatedClient()
		{
			var client = new ClientForActivation
			{
				BirthDate = DateTime.Now.AddYears(-25),
				ClientId = testClientId,
				EtlSessionId = testSessionId,
				FirstName = Path.GetRandomFileName(),
				LastName = Path.GetRandomFileName(),
				Gender = 0,
				MiddleName = Path.GetRandomFileName(),
				Status = (int)ActivateClientStatus.Success
			};
			var client2 = new ClientForActivation
			{
				BirthDate = DateTime.Now.AddYears(-25),
				ClientId = testClientId,
				EtlSessionId = testSessionId2,
				FirstName = Path.GetRandomFileName(),
				LastName = Path.GetRandomFileName(),
				Gender = 0,
				MiddleName = Path.GetRandomFileName()
			};
			IEnumerable<ClientForActivation> recordFromBase = null;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
				repository.Add(client);
				repository.Add(client2);
				uow.Save();
			}

			using (var uow = CreateUow())
			{
				recordFromBase = uow.ClientForActivationRepository.GetUniqueClientIdBySession(testSessionId2).ToList();
			}

			Assert.IsNotNull(recordFromBase);
			Assert.AreEqual(0, recordFromBase.Count());
		}

		[TestMethod]
		public void SetOldStatusForDuplicateRecordsTest()
		{
			var client = new ClientForActivation
				             {
					             BirthDate = DateTime.Now.AddYears(-25),
					             ClientId = testClientId,
					             EtlSessionId = testSessionId,
					             FirstName = Path.GetRandomFileName(),
					             LastName = Path.GetRandomFileName(),
					             Gender = 0,
					             MiddleName = Path.GetRandomFileName()
				             };
			var client2 = new ClientForActivation
				              {
					              BirthDate = DateTime.Now.AddYears(-25),
					              ClientId = testClientId,
					              EtlSessionId = testSessionId2,
					              FirstName = Path.GetRandomFileName(),
					              LastName = Path.GetRandomFileName(),
					              Gender = 0,
					              MiddleName = Path.GetRandomFileName(),
								  Status = (int)ActivateClientStatus.Success
				              };
			ClientForActivation updatedClient;

			using (var uow = CreateUow())
			{
                var repository = uow.ClientForActivationRepository;
				repository.Add(client);
				repository.Add(client2);
				uow.Save();

				var duplicates = (from c in repository.GetAll()
								  join s in repository.GetOtherEtlSessionsClientRecords(testSessionId) on c.ClientId equals s.ClientId into lj
								  from j in lj.DefaultIfEmpty()
								  where c.EtlSessionId == testSessionId && j.EtlSessionId != null
								  select c).ToList();
				duplicates.ForEach(
					x =>
						{
							x.Status = (int)ActivateClientStatus.AlreadyActivated;
							x.Reason = String.Format("Client with ClientId = ({0}) already activated", x.ClientId);
							repository.Update(x);
						});

				uow.Save();
			}

			using (var uow = CreateUow())
			{
                updatedClient = uow.ClientForActivationRepository.GetById(x => x.ClientId == testClientId && x.EtlSessionId == testSessionId);
			}

			Assert.IsNotNull(updatedClient);
			Assert.AreEqual(ActivateClientStatus.AlreadyActivated, (ActivateClientStatus)updatedClient.Status);
		}
	}
}
