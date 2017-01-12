namespace RapidSoft.Loaylty.PromoAction.Tests.AdminServices
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Service;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleDomainManageServiceTests
    {
        private readonly List<long> ids = new List<long>();

        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            var mock = new Mock<IUserService>();
            var superUser = TestHelper.BuildSuperUser();
            mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            ArmSecurity.UserServiceCreator = null;
        }

        [TestCleanup]
        public void MyTestCleanup()
        {
            TestHelper.DeleteTestRuleDomain(this.ids);
        }

        [TestMethod]
        public void ShouldCreate()
        {
            var client = new AdminMechanicsService();

            var ruleDomain = new RuleDomain
                                 {
                                     Name = "Правило" + Guid.NewGuid(),
                                     Description = "Jgbcfkjfj",
                                     LimitFactor = 56,
                                     LimitType = LimitTypes.Percent,
                                     UpdatedUserId = "We"
                                 };

            var saved = client.SaveRuleDomain(ruleDomain, TestDataStore.TestUserId);

            this.ids.Add(saved.RuleDomain.Id);

            Assert.AreNotEqual(saved.RuleDomain.Id, default(long));
            Assert.IsNotNull(new RuleDomainRepository().Get(saved.RuleDomain.Id));
        }

        [TestMethod]
        public void ShouldReturnById()
        {
            var client = new AdminMechanicsService();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };

            new RuleDomainRepository().Save(ruleDomain);

            var getted = client.GetRuleDomain(ruleDomain.Id, TestDataStore.TestUserId);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.RuleDomain.Name, ruleDomain.Name);
            Assert.AreEqual(getted.RuleDomain.Description, ruleDomain.Description);

            this.ids.Add(getted.RuleDomain.Id);
        }

        [TestMethod]
        public void ShouldReturnAll()
        {
            var client = new AdminMechanicsService();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };

            new RuleDomainRepository().Save(ruleDomain);

            this.ids.Add(ruleDomain.Id);

            var all = client.GetAllRuleDomains(TestDataStore.TestUserId);

            var getted = all.RuleDomains.FirstOrDefault(x => x.Id == ruleDomain.Id);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.Name, ruleDomain.Name);
            Assert.AreEqual(getted.Description, ruleDomain.Description);
            Assert.AreEqual(getted.LimitFactor, 56);
            Assert.AreEqual(getted.LimitType, LimitTypes.Percent);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            var client = new AdminMechanicsService();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   LimitFactor = 56,
                                   LimitType = LimitTypes.Percent,
                                   Description = "Jgbcfkjfj",
                                   UpdatedUserId = "We"
                               };
            new RuleDomainRepository().Save(ruleDomain);
            this.ids.Add(ruleDomain.Id);

            var updateRuleDomain = new RuleDomain
                                       {
                                           Id = ruleDomain.Id,
                                           Name = "Новое название " + Guid.NewGuid(),
                                           LimitFactor = 1,
                                           LimitType = LimitTypes.Fixed,
                                           Description = "Описалово",
                                           UpdatedUserId = "I"
                                       };

            client.SaveRuleDomain(updateRuleDomain, TestDataStore.TestUserId);

            var updated = client.GetRuleDomain(ruleDomain.Id, TestDataStore.TestUserId);

            Assert.IsNotNull(updated);
            Assert.AreEqual(updated.RuleDomain.Name, updateRuleDomain.Name);
            Assert.AreEqual(updated.RuleDomain.Description, updateRuleDomain.Description);
            Assert.AreEqual(updated.RuleDomain.LimitFactor, 1);
            Assert.AreEqual(updated.RuleDomain.LimitType, LimitTypes.Fixed);
        }

        [TestMethod]
        public void ShouldDelete()
        {
            var repo = new RuleDomainRepository();
            var client = new AdminMechanicsService();

            var ruleDomain = new RuleDomain
                               {
                                   Name = "Правило" + Guid.NewGuid(),
                                   Description = "Jgbcfkjfj",
                                   LimitFactor = 1,
                                   LimitType = LimitTypes.Fixed,
                                   UpdatedUserId = "We"
                               };
            repo.Save(ruleDomain);
            this.ids.Add(ruleDomain.Id);

            client.DeleteRuleDomainById(ruleDomain.Id, TestDataStore.TestUserId);

            var fromDB = repo.Get(ruleDomain.Id);
            Assert.IsNull(fromDB);
        }
    }
}
