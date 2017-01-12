namespace RapidSoft.Loaylty.PromoAction.Tests.Repositories
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities.History;
    using RapidSoft.Loaylty.PromoAction.Repositories;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleHistoryRepositoryTests
    {
        private readonly List<long> ids = new List<long>();

        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            // NOTE: Используется только если БД нету
            // Database.SetInitializer(new CreateDatabaseIfNotExists<MechanicsDbContext>());
            // var ctx = MechanicsDbContext.Get();
            // ctx.Database.Delete();
            // ctx.Database.CreateIfNotExists();
        }

        // [TestInitialize()]
        // public void MyTestInitialize() { }

        [TestCleanup]
        public void MyTestCleanup()
        {
            TestHelper.DeleteTestRuleDomain(this.ids);
        }
        #endregion

        [TestMethod]
        public void ShouldCreateHistoryCreateItem()
        {
            var repo = new RuleRepository();
            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            var rule = TestHelper.BuildTestRule(ruleDomainId: ruleDomain.Id);

            repo.Save(rule);

            var countHistoryItems = this.GetCountHistoryItems(HistoryEvent.Create, rule.Id);

            Assert.AreEqual(countHistoryItems, 1);
        }

        [TestMethod]
        public void ShouldCreateHistoryUpdateItem()
        {
            var repo = new RuleRepository();
            var doamin = TestHelper.InsertTestRuleDomain();
            this.ids.Add(doamin.Id);
            var rule = TestHelper.InsertTestRule(doamin);

            rule.Priority = 500;

            repo.Save(rule);

            var countHistoryItems = this.GetCountHistoryItems(HistoryEvent.Update, rule.Id);

            Assert.AreEqual(countHistoryItems, 1);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldCreateHistoryDeleteItem()
        {
            var repo = new RuleRepository();
            var doamin = TestHelper.InsertTestRuleDomain();
            this.ids.Add(doamin.Id);
            var rule = TestHelper.InsertTestRule(doamin);

            repo.DeleteById(rule.Id, "We");

            var countHistoryItems = this.GetCountHistoryItems(HistoryEvent.Delete, rule.Id);

            Assert.AreEqual(countHistoryItems, 1);
        }

        private long GetCountHistoryItems(HistoryEvent @event, long ruleId)
        {
            var context = MechanicsDbContext.Get();

            return context.Set<RuleHistory>().Count(x => x.Event == @event && x.RuleId == ruleId);
        }
    }
}
