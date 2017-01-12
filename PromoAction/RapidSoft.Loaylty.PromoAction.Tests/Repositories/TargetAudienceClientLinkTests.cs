namespace RapidSoft.Loaylty.PromoAction.Tests.Repositories
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Tests;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class TargetAudienceClientLinkTests
    {
        private const string ClientId = "asdfggggga";
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // NOTE: Используется только если БД нету. ВНИМАНИЕ можно потерять все данные!!!
            // Database.SetInitializer(new CreateDatabaseIfNotExists<MechanicsDbContext>());
            // var ctx = MechanicsDbContext.Get();
            // ctx.Database.Delete();
            // ctx.Database.CreateIfNotExists();
        }

        [TestMethod]
        public void ShouldCreate()
        {
            var targetAudienceId = TestHelper.InsertTestTargetAudience().Id;

            var repo = new TargetAudienceClientLinkRepository();

            var link = new TargetAudienceClientLink { TargetAudienceId = targetAudienceId, ClientId = ClientId, CreateUserId = "I1" };

            repo.Insert(link);

            var ctx = MechanicsDbContext.Get();
            Assert.IsNotNull(ctx.TargetAudienceClientLinks.Single(x => x.TargetAudienceId == targetAudienceId));
            Assert.IsNotNull(ctx.TargetAudienceClientLinkHistories.Single(x => x.TargetAudienceId == targetAudienceId && x.Event == HistoryEvent.Create && x.CreateUserId == "I1"));

            Console.WriteLine(ctx.Database.Connection.ConnectionString);
        }

        [TestMethod]
        public void ShouldGetById()
        {
            var testLink = TestHelper.InsertTargetAudienceClientLink(clientId: ClientId);
            var repo = new TargetAudienceClientLinkRepository();

            var targetAudience = repo.Get(testLink.TargetAudienceId, testLink.ClientId);

            Assert.IsNotNull(targetAudience);
        }

        [TestMethod]
        public void ShouldGetAll()
        {
            var testLink = TestHelper.InsertTargetAudienceClientLink(clientId: ClientId);
            var repo = new TargetAudienceClientLinkRepository();

            var targetAudiences = repo.GetAll();

            Assert.IsTrue(targetAudiences.Any(x => x.TargetAudienceId == testLink.TargetAudienceId));
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var testLink = TestHelper.InsertTargetAudienceClientLink(clientId: ClientId);
            var repo = new TargetAudienceClientLinkRepository();

            repo.DeleteById(testLink.TargetAudienceId, ClientId, "I3");

            var deletedRule = repo.Get(testLink.TargetAudienceId, ClientId);

            Assert.IsNull(deletedRule);

            var ctx = MechanicsDbContext.Get();
            Assert.IsNotNull(
                ctx.TargetAudienceClientLinkHistories.Single(
                    x => x.TargetAudienceId == testLink.TargetAudienceId && x.ClientId == ClientId && x.Event == HistoryEvent.Delete && x.DeleteUserId == "I3"));
        }

        [TestMethod]
        public void ShouldInsertTargetAudienceLink()
        {
            const string TestAudienceId = "Audience_test_1";
            const string TestClientId = "integration_test_client_id_1";

            var repository = new TargetAudienceRepository();
            var testAudience = repository.Exists(TestAudienceId) ? repository.Get(TestAudienceId) : new TargetAudience();
            testAudience.Id = TestAudienceId;
            testAudience.CreateDateTime = DateTime.Now;
            testAudience.CreateDateTimeUtc = DateTime.UtcNow;
            testAudience.CreateUserId = "TestUser";
            testAudience.IsSegment = false;
            testAudience.Name = "Audience_test_1_name";
            repository.Save(testAudience);

            var repositoryLink = new TargetAudienceClientLinkRepository();

            var existed = repositoryLink.Get(TestAudienceId, TestClientId);
            if (existed != null)
            {
                repositoryLink.DeleteById(existed.TargetAudienceId, existed.ClientId, TestDataStore.TestUserId);
            }

            var testLink = new TargetAudienceClientLink();
            testLink.ClientId = TestClientId;
            testLink.TargetAudienceId = TestAudienceId;
            testLink.CreateUserId = "TestUser";
            repositoryLink.Insert(testLink);
        }
    }
}
