namespace RapidSoft.VTB24.BankConnector.Tests.RepositoryTests
{
    using System;

    using Faker;

    using Microsoft.Practices.Unity;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.VTB24.BankConnector.DataModels;
    using RapidSoft.VTB24.BankConnector.DataSource;

    [TestClass]
    public class ClientPersonalMessageTests : TestBase
    {
        private readonly Guid testSessionId = new Guid("71231619-C0C1-4836-8FE1-A0A6F95944F1");

        private const string testClient = "ClientIdTest";

        [TestInitialize]
        public void Init()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientPersonalMessageRepository;
                foreach (var t in repository.GetAll())
                    repository.Delete(t);

                uow.Save();
            }
        }

        [TestMethod]
        public void Insert()
        {
            using (var uow = CreateUow())
            {
                var repository = uow.ClientPersonalMessageRepository;
                repository.DeleteBySessionId(this.testSessionId);
                repository.Add( new ClientPersonalMessage()
                                    {
                                        ClientId = testClient,
                                        FromDateTime = DateTime.Now,
                                        Message = Lorem.Paragraph(),
                                        SessionId = this.testSessionId,
                                        ToDateTime = DateTime.Now.AddDays(1)
                                    });                
                uow.Save();
            }
        }
    }
}