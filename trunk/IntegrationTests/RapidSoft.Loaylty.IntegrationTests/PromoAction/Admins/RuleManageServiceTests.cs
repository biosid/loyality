namespace RapidSoft.Loaylty.IntegrationTests.PromoAction.Admins
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Loaylty.PromoAction.WsClients.AdminMechanicsService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleManageServiceTests
    {
        private const string Filter1 = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""like"">
    <value type=""attr"">
      <attr object=""Prodicts"" name=""Vendor"" type=""string"" />
    </value>
    <value type=""string"">1C</value>
  </equation>
</filter>";

        private const string Filter2 = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""gt"">
    <value type=""attr"">
      <attr object=""Prodicts"" name=""PriceRUR"" type=""numeric"" />
    </value>
    <value type=""numeric"">1500</value>
  </equation>
</filter>";

        private static readonly List<long> Ids = new List<long>();
        private readonly RuleDomain rd = CreateRuleDomain();
        private static string userId = TestHelper.UserId;

        [ClassCleanup]
        public static void ClassCleanup()
        {
            foreach (var id in Ids)
            {
                var id1 = id;
                WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.DeleteRuleDomainById(id1, TestHelper.VtbSystemUserId));
            }
        }

        [TestMethod, TestCategory("WS")]
        public void ShouldCreate()
        {
            var client = new AdminMechanicsServiceClient();

            var rule = new Rule
                           {
                               Name = "Test",
                               RuleDomainId = this.rd.Id,
                               Type = RuleTypes.BaseAddition,
                               Predicate = Filter1,
                               Factor = 5.555m,
                               ConditionalFactors = null,
                               DateTimeFrom = null,
                               DateTimeTo = DateTime.Now.AddMonths(5),
                               UpdatedUserId = "We",
                               IsExclusive = true,
                               IsNotExcludedBy = true,
                               Priority = 500,
                           };

            var saved = client.CreateRule(rule, userId);

            Assert.AreNotEqual(saved.Rule.Id, default(long));
            Assert.AreNotEqual(saved.Rule.Id, rule.Id);
        }

        [TestMethod]
        public void ShouldReturnById()
        {
            var client = new AdminMechanicsServiceClient();

            var rule = new Rule
                           {
                               Name = "Test",
                               RuleDomainId = this.rd.Id,
                               Type = RuleTypes.BaseAddition,
                               Predicate = Filter1,
                               Factor = 5.555m,
                               ConditionalFactors = null,
                               DateTimeFrom = null,
                               DateTimeTo = DateTime.Now.AddMonths(5),
                               UpdatedUserId = "We",
                               IsExclusive = true,
                               IsNotExcludedBy = true,
                               Priority = 500,
                           };

            var saved = client.CreateRule(rule, userId);

            var getted = client.GetRule(saved.Rule.Id, userId);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.Rule.RuleDomainId, rule.RuleDomainId);
            Assert.AreEqual(getted.Rule.Factor, rule.Factor);
            Assert.AreEqual(getted.Rule.Type, RuleTypes.BaseAddition);
        }

        [TestMethod]
        public void ShouldReturnAll()
        {
            var client = new AdminMechanicsServiceClient();

            var rule = new Rule
                           {
                               Name = "Test",
                               RuleDomainId = this.rd.Id,
                               Type = RuleTypes.BaseAddition,
                               Predicate = Filter1,
                               Factor = 5.555m,
                               ConditionalFactors = null,
                               DateTimeFrom = null,
                               DateTimeTo = DateTime.Now.AddMonths(5),
                               UpdatedUserId = "We",
                               IsExclusive = true,
                               IsNotExcludedBy = true,
                               Priority = 500,
                           };

            var saved = client.CreateRule(rule, userId);

            var all = client.GetRules(new GetRulesParameters { RuleDomainId = this.rd.Id, UserId = userId });

            var getted = all.Rules.FirstOrDefault(x => x.Id == saved.Rule.Id);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.RuleDomainId, rule.RuleDomainId);
            Assert.AreEqual(getted.Factor, rule.Factor);
            Assert.AreEqual(getted.Type, RuleTypes.BaseAddition);
        }

        [TestMethod]
        public void ShouldUpdate()
        {
            var client = new AdminMechanicsServiceClient();

            var rule = new Rule
                           {
                               Name = "Test",
                               RuleDomainId = this.rd.Id,
                               Type = RuleTypes.BaseAddition,
                               Predicate = Filter1,
                               Factor = 5.555m,
                               ConditionalFactors = null,
                               DateTimeFrom = null,
                               DateTimeTo = DateTime.Now.AddMonths(5),
                               UpdatedUserId = "We",
                               IsExclusive = true,
                               IsNotExcludedBy = true,
                               Priority = 500,
                           };
            var saved = client.CreateRule(rule, userId);

            var updateRule = new Rule
                                 {
                                     Name = "Test",
                                     Id = saved.Rule.Id,
                                     RuleDomainId = this.rd.Id,
                                     Type = RuleTypes.BaseAddition,
                                     Predicate = Filter2,
                                     Factor = 5.566m,
                                     ConditionalFactors = null,
                                     DateTimeFrom = null,
                                     DateTimeTo = DateTime.Now.AddMonths(2),
                                     UpdatedUserId = "We",
                                     IsExclusive = true,
                                     IsNotExcludedBy = true,
                                     Priority = 520,
                                 };

            client.UpdateRule(updateRule, userId);

            var getted = client.GetRule(saved.Rule.Id, userId);

            Assert.IsNotNull(getted);
            Assert.AreEqual(getted.Rule.RuleDomainId, updateRule.RuleDomainId);
            Assert.AreEqual(getted.Rule.Factor, updateRule.Factor);
            Assert.AreEqual(getted.Rule.Type, RuleTypes.BaseAddition);
            }

        [TestMethod]
        public void ShouldDelete()
        {
            var client = new AdminMechanicsServiceClient();

            var rule = new Rule
                           {
                               Name = "Test",
                               RuleDomainId = this.rd.Id,
                               Type = RuleTypes.BaseAddition,
                               Predicate = Filter1,
                               Factor = 5.555m,
                               ConditionalFactors = null,
                               DateTimeFrom = null,
                               DateTimeTo = DateTime.Now.AddMonths(5),
                               UpdatedUserId = "We",
                               IsExclusive = true,
                               IsNotExcludedBy = true,
                               Priority = 500,
                           };
            var saved = client.CreateRule(rule, userId);

            client.DeleteRuleById(saved.Rule.Id, userId);

            var deleted = client.GetRule(saved.Rule.Id, userId);

            Assert.IsNotNull(deleted);
            Assert.IsNull(deleted.Rule);
        }

        private static RuleDomain CreateRuleDomain()
        {
            var repo = new AdminMechanicsServiceClient();

            var domain = new RuleDomain
                             {
                                 Name = "Name " + Guid.NewGuid(), 
                                 Description = "Описалово",
                                 LimitFactor = 1,
                                 LimitType = LimitTypes.Fixed,
                                 UpdatedUserId = "I"
                             };

            var saved = repo.SaveRuleDomain(domain, userId);

            Ids.Add(saved.RuleDomain.Id);

            return saved.RuleDomain;
        }
    }
}
