namespace RapidSoft.Loaylty.PromoAction.Tests.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Repositories;

    using Rule = RapidSoft.Loaylty.PromoAction.Api.Entities.Rule;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleDomainRepositoryTests
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
        public void ShouldCreate()
        {
            var repo = new RuleDomainRepository();

            var ruleDomain = TestHelper.BuildTestRuleDomain();

            Assert.AreEqual(ruleDomain.Id, 0);

            repo.Save(ruleDomain);

            Assert.AreNotEqual(ruleDomain.Id, 0);

            this.ids.Add(ruleDomain.Id);
        }

        [TestMethod]
        public void ShouldGetById()
        {
            var id = TestHelper.InsertTestRuleDomain().Id;
            var repo = new RuleDomainRepository();

            var rule = repo.Get(id);

            Assert.IsNotNull(rule);

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldGetAll()
        {
            var id = TestHelper.InsertTestRuleDomain().Id;
            var repo = new RuleDomainRepository();

            var rules = repo.GetAll();

            Assert.IsTrue(rules.Any(x => x.Id == id));

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            var id = TestHelper.InsertTestRuleDomain().Id;
            var repo = new RuleDomainRepository();

            var ruleDomain = repo.Get(id);

            var newName = "TestRule" + Guid.NewGuid();

            ruleDomain.Name = newName;

            repo.Save(ruleDomain);

            var updatedRuleDomain = repo.Get(id);

            Assert.AreEqual(updatedRuleDomain.Name, newName);

            Assert.IsNotNull(updatedRuleDomain.UpdatedDate);

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var id = TestHelper.InsertTestRuleDomain().Id;
            var repo = new RuleDomainRepository();

            repo.DeleteById(id, "We");

            var deletedRule = repo.Get(id);

            Assert.IsNull(deletedRule);

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldGetByName()
        {
            var name = "RuleDomain " + Guid.NewGuid();
            var id = TestHelper.InsertTestRuleDomain(name).Id;

            var repo = new RuleDomainRepository();

            var ruleDomain = repo.GetByName(name);

            Assert.IsNotNull(ruleDomain);

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldGetByNameWithRules()
        {
            var name = "RuleDomain " + Guid.NewGuid();
            var id = TestHelper.InsertTestRuleDomainWithoutBaseRule(name).Id;

            var repo = new RuleDomainRepository();

            var ruleDomain = repo.GetByName(name);

            Assert.IsNotNull(ruleDomain);
            Assert.IsNotNull(ruleDomain.Rules);
            Assert.IsTrue(ruleDomain.Rules.Any());

            this.ids.Add(id);
        }

        [TestMethod]
        public void ShouldNotGetByNullName()
        {
            var repo = new RuleDomainRepository();

            var ruleDomain = repo.GetByName(null);

            Assert.IsNull(ruleDomain);
        }

        [TestMethod]
        public void ShouldCreateWithRules()
        {
            var repo = new RuleDomainRepository();

            var ruleDomain = TestHelper.BuildTestRuleDomain();
            ruleDomain.Rules = new List<Rule>
                                   {
                                       TestHelper.BuildTestRule(
                                           type: RuleTypes.BaseMultiplication, priority: 5)
                                   };

            Assert.AreEqual(ruleDomain.Id, 0);

            repo.Save(ruleDomain);

            Assert.AreNotEqual(ruleDomain.Id, 0);

            var savedRuleDomain = repo.GetByName(ruleDomain.Name);

            Assert.AreEqual(savedRuleDomain.Rules.Count, 1);

            this.ids.Add(ruleDomain.Id);
        }
    }
}
