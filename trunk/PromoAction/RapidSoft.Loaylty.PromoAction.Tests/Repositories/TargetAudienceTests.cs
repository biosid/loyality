namespace RapidSoft.Loaylty.PromoAction.Tests.Repositories
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Tests;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class TargetAudienceTests
    {
        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // NOTE: Используется только если БД нету. ВНИМАНИЕ можно потерять все данные!!!
            // var ctx = PromoActionDbContext.Get();
            // Database.SetInitializer(new CreateDatabaseIfNotExists<PromoActionDbContext>());
            // ctx.Database.Delete();
            // ctx.Database.CreateIfNotExists();
        }

        // [TestInitialize()]
        // public void MyTestInitialize() { }

        // [TestCleanup()]
        // public void MyTestCleanup() { }
        #endregion

        [TestMethod]
        public void ShouldCreate()
        {
            var repo = new TargetAudienceRepository();

            var id = "VIP " + Guid.NewGuid();

            var targetAudience = new TargetAudience { Id = id, Name = "Name " + Guid.NewGuid(), CreateUserId = "I 1", IsSegment = true };

            repo.Save(targetAudience);

            var ctx = MechanicsDbContext.Get();
            Assert.IsNotNull(ctx.TargetAudiences.Single(x => x.Id == targetAudience.Id));
            Assert.IsNotNull(ctx.TargetAudienceHistories.Single(x => x.TargetAudienceId == targetAudience.Id && x.Event == HistoryEvent.Create));

            var obj = ctx.TargetAudiences.FirstOrDefault(t => t.Id == id);
            var objHistory = ctx.TargetAudienceHistories.FirstOrDefault(t => t.TargetAudienceId == id);

            Assert.IsNotNull(obj);
            Assert.IsNotNull(objHistory);
            Assert.IsTrue(obj.IsSegment);
            Assert.IsTrue(objHistory.IsSegment);
        }

        [TestMethod]
        public void ShouldGetById()
        {
            var id = TestHelper.InsertTestTargetAudience().Id;
            var repo = new TargetAudienceRepository();

            var targetAudience = repo.Get(id);

            Assert.IsNotNull(targetAudience);
        }

        [TestMethod]
        public void ShouldGetAll()
        {
            var id = TestHelper.InsertTestTargetAudience().Id;
            var repo = new TargetAudienceRepository();

            var targetAudiences = repo.GetAll();

            Assert.IsTrue(targetAudiences.Any(x => x.Id == id));
        }

        [TestMethod]
        public void ShouldGetByClientId()
        {
            var targetAudienceId = TestHelper.InsertTestTargetAudience().Id;
            var clientId = 500.ToString(CultureInfo.InvariantCulture);
            TestHelper.InsertTargetAudienceClientLink(targetAudienceId: targetAudienceId, clientId: clientId);

            var repo = new TargetAudienceRepository();

            var targetAudiences = repo.GetByClientId(clientId);

            Assert.IsTrue(targetAudiences.Any(x => x.Id == targetAudienceId));
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            var id = TestHelper.InsertTestTargetAudience().Id;
            var repo = new TargetAudienceRepository();

            var targetAudience = repo.Get(id);

            var newName = "TestTargetAudience" + Guid.NewGuid();

            targetAudience.Name = newName;
            targetAudience.UpdateUserId = "I2";

            repo.Save(targetAudience);

            var updatedTargetAudience = repo.Get(id);

            Assert.AreEqual(updatedTargetAudience.Name, newName);

            var ctx = MechanicsDbContext.Get();
            Assert.IsNotNull(ctx.TargetAudienceHistories.Single(x => x.TargetAudienceId == targetAudience.Id && x.Event == HistoryEvent.Update && x.UpdateUserId == "I2"));
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var id = TestHelper.InsertTestTargetAudience().Id;
            var repo = new TargetAudienceRepository();

            repo.DeleteById(id, "I3");

            var deletedRule = repo.Get(id);

            Assert.IsNull(deletedRule);

            var ctx = MechanicsDbContext.Get();
            Assert.IsNotNull(
                ctx.TargetAudienceHistories.Single(
                    x => x.TargetAudienceId == id && x.Event == HistoryEvent.Delete && x.DeleteUserId == "I3"));
        }
    }
}
