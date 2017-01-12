namespace RapidSoft.Loaylty.IntegrationTests.PromoAction
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Loaylty.PromoAction.WsClients.AdminMechanicsService;
    using Loaylty.PromoAction.WsClients.MechanicsService;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ResultBase = Loaylty.PromoAction.WsClients.AdminMechanicsService.ResultBase;


    /// <summary>
    /// Добавить тест со следующим набором скидок (правил):
    /// базавая аддитивная: всегда цену товара в рублях умнажаем на 1000
    /// базовая аддитивная: 
    /// усл коэф1 - Если ClientProfile.KLADR = 7700000000000 то аддитивная -100
    /// усл коэф2 - Если ClientProfile.KLADR = 7800000000000 то аддитивная -300
    /// не базовая мультипликативная
    /// если в массиве ClientProfile.Audiences есть элемент Vip то 0.5
    /// </summary>
    /// <remarks>
    /// Таким образом тесты по домену с правилами:
    /// 1. Базовое Муль.: Если Partner.Id = 1, то коэф 1000
    /// 2. Базовое Адд.: Если Partner.Id = 1, то коэф 0,
    /// 2.1 Усл.коэф: Если ClientProfile.KLADR = 7700000000000, то коэф -100, приоритет 2
    /// 2.2 Усл.коэф: Если ClientProfile.KLADR = 7800000000000, то коэф -300, приоритет 1
    /// 3. Не базовое Муль.: Если ClientProfile.Audiences contain Vip, то коэф 0.5
    /// </remarks>
    [TestClass]
    public class Demo2Tests
    {
        private const string InitalNumberAlias = "p.p1";

        private static long domainId;

        private static string domainName;
        private static string userId;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            domainName = CreateDemoRuleDomain();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.DeleteRuleDomainById(domainId, TestHelper.VtbSystemUserId));
        }

        [TestMethod]
        public void ShouldCalculateForKladr7700000000000()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "1" },
                                  { "ClientProfile.KLADR", "7700000000000" },
                                  { "ClientProfile.Audiences", "Vip" }
                              };

            var result = WebClientCaller.CallService<MechanicsServiceClient, CalculateResult>(c => c.CalculateSingleValue(domainName, initalNumber, context));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, ((initalNumber * 1000) - 100) * 0.5m);
        }

        [TestMethod]
        public void ShouldCalculateForKladr7800000000000()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "1" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { "ClientProfile.Audiences", "Vip" }
                              };

            var result = WebClientCaller.CallService<MechanicsServiceClient, CalculateResult>(c => c.CalculateSingleValue(domainName, initalNumber, context));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, ((initalNumber * 1000) - 300) * 0.5m);
        }

        [TestMethod]
        public void ShouldCalculateForOtherPartner()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "222" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { "ClientProfile.Audiences", "Vip" }
                              };

            var result = WebClientCaller.CallService<MechanicsServiceClient, CalculateResult>(c => c.CalculateSingleValue(domainName, initalNumber, context));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber * 0.5m);
        }

        [TestMethod]
        public void ShouldCalculateForOtherAudiences()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "222" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { "ClientProfile.Audiences", "NotVip" }
                              };

            var result = WebClientCaller.CallService<MechanicsServiceClient, CalculateResult>(c => c.CalculateSingleValue(domainName, initalNumber, context));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber);
        }

        [TestMethod]
        public void ShouldCalculateForOtherKladrAndOtherAudiences()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "1" },
                                  { "ClientProfile.KLADR", "55555555555555" },
                                  { "ClientProfile.Audiences", "NotVip" }
                              };

            var result = WebClientCaller.CallService<MechanicsServiceClient, CalculateResult>(c => c.CalculateSingleValue(domainName, initalNumber, context));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber * 1000);
        }

        [TestMethod]
        public void ShouldGenerateOnlyForAudiences()
        {
            var context = new Dictionary<string, string>
                              {
                                  { "ClientProfile.Audiences", "Vip" }
                              };

            var param = new GenerateSqlParameters
                            {
                                RuleDomainName = domainName,
                                InitialNumberAlias = InitalNumberAlias,
                                Context = context,
                                Aliases = null
                            };

            var result =
                WebClientCaller.CallService<MechanicsServiceClient, GenerateResult>(
                    c => c.GenerateSql(param));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, 
                "p.p1 * CASE " +
                    "WHEN Partner.Id=1 " +
                       "THEN 1000 " +
                       "ELSE 1 " +
                    "END + CASE " +
                    "WHEN Partner.Id=1 " +
                        "THEN " +
                            "CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END " +
                        "ELSE 0 " +
                    "END");
            Assert.AreEqual(
                result.ActionSql,
                "CASE "+
                    "WHEN " +
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 0.5 + 0) > (0) " +
                    "THEN " +
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 0.5 + 0) " +
                    "ELSE " +
                        "11.11 " + 
                "END");
        }

        [TestMethod]
        public void ShouldGenerateForAudiencesAndKladr()
        {
            var context = new Dictionary<string, string>
                              {
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { "ClientProfile.Audiences", "Vip" }
                              };

            var param = new GenerateSqlParameters
                            {
                                RuleDomainName = domainName,
                                InitialNumberAlias = InitalNumberAlias,
                                Context = context,
                                Aliases = null
                            };

            var result =
                WebClientCaller.CallService<MechanicsServiceClient, GenerateResult>(
                    c => c.GenerateSql(param));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, "p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN -300 ELSE 0 END");
            Assert.AreEqual(
                result.ActionSql,
                "CASE " + 
                    "WHEN " + 
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN -300 ELSE 0 END) * 0.5 + 0) > (0) " + 
                    "THEN " + 
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN -300 ELSE 0 END) * 0.5 + 0) " + 
                    "ELSE 11.11 " + 
                "END");
        }

        [TestMethod]
        public void ShouldGenerateWithoutContextAndAliases()
        {
            var param = new GenerateSqlParameters
                            {
                                RuleDomainName = domainName,
                                InitialNumberAlias = InitalNumberAlias,
                                Context = null,
                                Aliases = null
                            };

            var result = WebClientCaller.CallService<MechanicsServiceClient, GenerateResult>(c => c.GenerateSql(param));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, 
                "p.p1 * " +
                    "CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END" +
                " + " +
                    "CASE " +
                        "WHEN Partner.Id=1 THEN " +
                                "CASE " +
                                    "WHEN ClientProfile.KLADR='7700000000000' THEN -100 " +
                                    "WHEN ClientProfile.KLADR='7800000000000' THEN -300 " +
                                    "ELSE 0 " +
                                 "END " +
                        "ELSE 0 " +
                    "END");
            Assert.AreEqual(
                result.ActionSql,
                "CASE " +
                    "WHEN " + 
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 1 + 0) > (0) " +
                    "THEN " +
                        "((p.p1 * CASE WHEN Partner.Id=1 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=1 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 1 + 0) " +
                    "ELSE " +
                        "11.11 " +
                    "END");
        }

        [TestMethod]
        public void ShouldGenerateWithLimitsForKnowContext()
        {
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "1" },
                                  { "ClientProfile.KLADR", "55555555555555" },
                                  { "ClientProfile.Audiences", "NotVip" }
                              };

            var param = new GenerateSqlParameters
                            {
                                RuleDomainName = domainName,
                                InitialNumberAlias = InitalNumberAlias,
                                Context = context,
                                Aliases = null
                            };

            var result = WebClientCaller.CallService<MechanicsServiceClient, GenerateResult>(c => c.GenerateSql(param));

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, "p.p1 * 1000 + 0");
            Assert.AreEqual(
                result.ActionSql, 
                "CASE " +
                    "WHEN ((p.p1 * 1000 + 0) * 1 + 0) > (0) " +
                    "THEN ((p.p1 * 1000 + 0) * 1 + 0) " +
                    "ELSE 11.11 " +
                "END");
        }

        private static string CreateDemoRuleDomain()
        {
            var rd = new RuleDomain
                         {
                             Name = "Тестовый домен правил " + Guid.NewGuid(),
                             Description = @"1. Базовое Муль.: Если Partner.Id = 1, то коэф 1000
2. Базовое Адд.: Если Partner.Id = 1, то коэф 0,
2.1 Усл.коэф: Если ClientProfile.KLADR = 7700000000000, то коэф -100
2.2 Усл.коэф: Если ClientProfile.KLADR = 7800000000000, то коэф -300
3. Не базовое Муль.: Если ClientProfile.Audiences contain Vip, то коэф 0.5",
                             UpdatedUserId = "FSY",
                             LimitType = LimitTypes.Fixed,
                             LimitFactor = 11.11m,
                             DefaultBaseAdditionFactor = 0,
                             DefaultBaseMultiplicationFactor = 1
                         };

            userId = TestHelper.UserId;
            var ruleDomainResult =
                WebClientCaller.CallService<AdminMechanicsServiceClient, RuleDomainResult>(
                    c => c.SaveRuleDomain(rd, userId));

            Assert.AreEqual(ruleDomainResult.Success, true, ruleDomainResult.ResultDescription);

            domainId = ruleDomainResult.RuleDomain.Id;

            var ruleDomain = ruleDomainResult.RuleDomain;

            var rule1 = new Rule
                            {
                                Name = "Test",
                                Factor = 1000,
                                Type = RuleTypes.BaseMultiplication,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""eq"">
    <value type=""attr"">
      <attr object=""Partner"" name=""Id"" type=""numeric"" />
    </value>
    <value type=""numeric"">1</value>
  </equation>
</filter>",
                                Priority = 0,
                                UpdatedUserId = "FSY"
                            };

            var rule2 = new Rule
                            {
                                Name = "Test",
                                Factor = 0,
                                Type = RuleTypes.BaseAddition,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""eq"">
    <value type=""attr"">
      <attr object=""Partner"" name=""Id"" type=""numeric"" />
    </value>
    <value type=""numeric"">1</value>
  </equation>
</filter>",
                                Priority = 0,
                                UpdatedUserId = "FSY"
                            };
            rule2.ConditionalFactors = @"<?xml version=""1.0"" encoding=""utf-16""?>
<ArrayOfConditionalFactor xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <ConditionalFactor>
    <Priority>2</Priority>
    <Predicate>
      <equation operator=""eq"">
        <value type=""attr"">
          <attr object=""ClientProfile"" name=""KLADR"" type=""string"" />
        </value>
        <value type=""string"">7700000000000</value>
      </equation>
    </Predicate>
    <Factor>-100</Factor>
  </ConditionalFactor>
  <ConditionalFactor>
    <Priority>1</Priority>
    <Predicate>
      <equation operator=""eq"">
        <value type=""attr"">
          <attr object=""ClientProfile"" name=""KLADR"" type=""string"" />
        </value>
        <value type=""string"">7800000000000</value>
      </equation>
    </Predicate>
    <Factor>-300</Factor>
  </ConditionalFactor>
</ArrayOfConditionalFactor>";

            var rule3 = new Rule
                            {
                                Name = "Test",
                                Factor = 0.5m,
                                Type = RuleTypes.Multiplication,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = @"<?xml version=""1.0"" encoding=""utf-16""?>
<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""cn"">
    <value type=""attr"">
      <attr object=""ClientProfile"" name=""Audiences"" type=""string"" />
    </value>
    <value type=""string"">Vip</value>
  </equation>
</filter>",
                                UpdatedUserId = "FSY"
                            };

            var ruleResult = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleResult>(c => c.CreateRule(rule1, userId));
            Assert.AreEqual(ruleResult.Success, true, ruleResult.ResultDescription);
            var approve = new[] { new Approve { RuleId = ruleResult.Rule.Id, IsApproved = true, Reason = "Test" } };
            var approveResult = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.SetRuleApproved(approve, userId));
            Assert.AreEqual(approveResult.Success, true, ruleResult.ResultDescription);

            ruleResult = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleResult>(c => c.CreateRule(rule2, userId));
            Assert.AreEqual(ruleResult.Success, true, ruleResult.ResultDescription);
            approve = new[] { new Approve { RuleId = ruleResult.Rule.Id, IsApproved = true, Reason = "Test" } };
            approveResult = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.SetRuleApproved(approve, userId));
            Assert.AreEqual(approveResult.Success, true, ruleResult.ResultDescription);

            ruleResult = WebClientCaller.CallService<AdminMechanicsServiceClient, RuleResult>(c => c.CreateRule(rule3, userId));
            Assert.AreEqual(ruleResult.Success, true, ruleResult.ResultDescription);
            approve = new[] { new Approve { RuleId = ruleResult.Rule.Id, IsApproved = true, Reason = "Test" } };
            approveResult = WebClientCaller.CallService<AdminMechanicsServiceClient, ResultBase>(c => c.SetRuleApproved(approve, userId));
            Assert.AreEqual(approveResult.Success, true, ruleResult.ResultDescription);

            return ruleDomain.Name;
        }
    }
}
