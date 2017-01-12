namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Service;
    using RapidSoft.Loaylty.PromoAction.Settings;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

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
    /// 1. Базовое Муль.: Если Partner.Id = 111, то коэф 1000
    /// 2. Базовое Адд.: Если Partner.Id = 111, то коэф 0,
    /// 2.1 Усл.коэф: Если ClientProfile.KLADR = 7700000000000, то коэф -100, приоритет 2
    /// 2.2 Усл.коэф: Если ClientProfile.KLADR = 7800000000000, то коэф -300, приоритет 1
    /// 3. Не базовое Муль.: Если ClientProfile.Audiences contain Vip, то коэф 0.5
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class Demo2Tests
    {
        private const string InitalNumberAlias = "p.p1";

        private static readonly string ClientProfileAudiences = string.Format(
            "{0}.{1}", ApiSettings.ClientProfileObjectName, ApiSettings.PromoActionPropertyName);

        private static string domainName;

        private static long domainId;

        #region Additional test attributes
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            var mock = new Mock<IUserService>();
            var superUser = TestHelper.BuildSuperUser();
            mock.Setup(x => x.GetUserPrincipalByName(TestDataStore.TestUserId)).Returns(superUser);
            var service = mock.Object;
            ArmSecurity.UserServiceCreator = () => service;

            var domain = CreateDemoRuleDomain();
            domainName = domain.Name;
            domainId = domain.Id;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TestHelper.DeleteTestRuleDomain(new[] { domainId });
            ArmSecurity.UserServiceCreator = null;
        }
        #endregion

        [TestMethod]
        public void ShouldCalculate1()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "111" },
                                  { "ClientProfile.KLADR", "7700000000000" },
                                  { ClientProfileAudiences, "Vip" }
                              };

            var result = new MechanicsService().CalculateSingleValue(domainName, initalNumber, context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, ((initalNumber * 1000) - 100) * 0.5m);

            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldCalculate2()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "111" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { ClientProfileAudiences, "Vip" }
                              };

            var result = new MechanicsService().CalculateSingleValue(domainName, initalNumber, context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, ((initalNumber * 1000) - 300) * 0.5m);

            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldCalculate3()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "222" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { ClientProfileAudiences, "Vip" }
                              };

            var result = new MechanicsService().CalculateSingleValue(domainName, initalNumber, context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber * 0.5m);

            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldCalculate4()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "222" },
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { ClientProfileAudiences, "NotVip" }
                              };

            var result = new MechanicsService().CalculateSingleValue(domainName, initalNumber, context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber);

            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldCalculate5()
        {
            var initalNumber = new Random().Next(1000);
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "111" },
                                  { "ClientProfile.KLADR", "55555555555555" },
                                  { ClientProfileAudiences, "NotVip" }
                              };

            var result = new MechanicsService().CalculateSingleValue(domainName, initalNumber, context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(result.PromoResult, initalNumber * 1000);

            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldGenerate1()
        {
            var context = new Dictionary<string, string>
                              {
                                  { ClientProfileAudiences, "Vip" }
                              };

            var param = new GenerateSqlParameters(domainName, InitalNumberAlias, context, null);

            var result = new MechanicsService().GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, 
                "p.p1 * CASE " +
                    "WHEN Partner.Id=111 " +
                       "THEN 1000 " +
                       "ELSE 1 " +
                    "END + CASE " +
                    "WHEN Partner.Id=111 " +
                        "THEN " +
                            "CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END " +
                        "ELSE 0 " +
                    "END");
            Assert.AreEqual(
                result.ActionSql,
                "CASE "+
                    "WHEN " +
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 0.5 + 0) > (0) " +
                    "THEN " +
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 0.5 + 0) " +
                    "ELSE " +
                        "11.11 " + 
                "END");
        }

        [TestMethod]
        public void ShouldGenerate2()
        {
            var context = new Dictionary<string, string>
                              {
                                  { "ClientProfile.KLADR", "7800000000000" },
                                  { ClientProfileAudiences, "Vip" }
                              };

            var param = new GenerateSqlParameters(domainName, InitalNumberAlias, context, null);

            var result = new MechanicsService().GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, "p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN -300 ELSE 0 END");
            Assert.AreEqual(
                result.ActionSql,
                "CASE " + 
                    "WHEN " + 
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN -300 ELSE 0 END) * 0.5 + 0) > (0) " + 
                    "THEN " + 
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN -300 ELSE 0 END) * 0.5 + 0) " + 
                    "ELSE 11.11 " + 
                "END");
        }

        [TestMethod]
        public void ShouldGenerate3()
        {
            var param = new GenerateSqlParameters(domainName, InitalNumberAlias, null, null);

            var result = new MechanicsService().GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.BaseSql, 
                "p.p1 * "+
                    "CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END" +
                " + " +
                    "CASE " +
                        "WHEN Partner.Id=111 THEN " +
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
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 1 + 0) > (0) " +
                    "THEN " +
                        "((p.p1 * CASE WHEN Partner.Id=111 THEN 1000 ELSE 1 END + CASE WHEN Partner.Id=111 THEN CASE WHEN ClientProfile.KLADR='7700000000000' THEN -100 WHEN ClientProfile.KLADR='7800000000000' THEN -300 ELSE 0 END ELSE 0 END) * 1 + 0) " +
                    "ELSE " +
                        "11.11 " +
                    "END");
        }

        [TestMethod]
        public void ShouldGenerate4()
        {
            var context = new Dictionary<string, string>
                              {
                                  { "Partner.Id", "111" },
                                  { "ClientProfile.KLADR", "55555555555555" },
                                  { ClientProfileAudiences, "NotVip" }
                              };

            var param = new GenerateSqlParameters(domainName, InitalNumberAlias, context, null);

            var result = new MechanicsService().GenerateSql(param);

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

        private static RuleDomain CreateDemoRuleDomain()
        {
            var partnerIdVar = new value
                                     {
                                         type = valueType.attr,
                                         attr =
                                             new[]
                                                 { new valueAttr { @object = "Partner", name = "Id", type = valueType.numeric } }
                                     };
            var partnerIdLiteral = new value { type = valueType.numeric, Text = new[] { "111" } };

            var clientProfileKladrVar = new value
                                     {
                                         type = valueType.attr,
                                         attr =
                                             new[] { new valueAttr { @object = "ClientProfile", name = "KLADR", type = valueType.@string } }
                                     };
            var clientProfileKladrLiteral1 = new value { type = valueType.@string, Text = new[] { "7700000000000" } };
            var clientProfileKladrLiteral2 = new value { type = valueType.@string, Text = new[] { "7800000000000" } };

            var clientProfileAudiencesVar = new value
                                                {
                                                    type = valueType.attr,
                                                    attr =
                                                        new[]
                                                            {
                                                                new valueAttr
                                                                    {
                                                                        @object =
                                                                            ApiSettings.ClientProfileObjectName,
                                                                        name =
                                                                            ApiSettings.PromoActionPropertyName,
                                                                        type = valueType.@string
                                                                    }
                                                            }
                                                };

            var clientProfileAudiencesLiteral = new value { type = valueType.@string, Text = new[] { "Vip" } };

            var partnerIdPredicate = new filter
                                         {
                                             Item =
                                                 new equation
                                                     {
                                                         @operator = equationOperator.eq,
                                                         value = new[]
                                                                     {
                                                                         partnerIdVar, 
                                                                         partnerIdLiteral
                                                                     }
                                                     }
                                         };

            var clientProfileKladrPredicate1 = new filter
                                                   {
                                                       Item =
                                                           new equation
                                                               {
                                                                   @operator = equationOperator.eq,
                                                                   value =
                                                                       new[]
                                                                           {
                                                                               clientProfileKladrVar, 
                                                                               clientProfileKladrLiteral1
                                                                           }
                                                               }
                                                   };
            var clientProfileKladrPredicate2 = new filter
                                                   {
                                                       Item =
                                                           new equation
                                                               {
                                                                   @operator = equationOperator.eq,
                                                                   value =
                                                                       new[]
                                                                           {
                                                                               clientProfileKladrVar, 
                                                                               clientProfileKladrLiteral2
                                                                           }
                                                               }
                                                   };

            var clientProfileAudiencesPredicate = new filter
                                                      {
                                                          Item =
                                                              new equation
                                                                  {
                                                                      @operator = equationOperator.cn,
                                                                      value =
                                                                          new[]
                                                                              {
                                                                                  clientProfileAudiencesVar,
                                                                                  clientProfileAudiencesLiteral
                                                                              }
                                                                  }
                                                      };

            var rd = new RuleDomain
                         {
                             Name = "Тестовый домен правил " + Guid.NewGuid(),
                             Description = @"1. Базовое Муль.: Если Partner.Id = 111, то коэф 1000
2. Базовое Адд.: Если Partner.Id = 111, то коэф 0,
2.1 Усл.коэф: Если ClientProfile.KLADR = 7700000000000, то коэф -100
2.2 Усл.коэф: Если ClientProfile.KLADR = 7800000000000, то коэф -300
3. Не базовое Муль.: Если ClientProfile.Audiences contain Vip, то коэф 0.5",
                             UpdatedUserId = "FSY",
                             LimitType = LimitTypes.Fixed,
                             LimitFactor = 11.11m
                         };

            var ruleDomainResult = new AdminMechanicsService().SaveRuleDomain(rd, TestDataStore.TestUserId);

            Assert.AreEqual(ruleDomainResult.Success, true);

            var ruleDomain = ruleDomainResult.RuleDomain;

            var rule1 = new Rule
                            {
                                Name = "Test Rule 1",
                                Factor = 1000,
                                Type = RuleTypes.BaseMultiplication,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = partnerIdPredicate.Serialize(),
                                Priority = 0,
                                UpdatedUserId = "FSY"
                            };

            var rule2 = new Rule
                            {
                                Name = "Test Rule 2",
                                Factor = 0,
                                Type = RuleTypes.BaseAddition,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = partnerIdPredicate.Serialize(),
                                Priority = 0,
                                UpdatedUserId = "FSY"
                            };
            var rule2CondFactor1 = new ConditionalFactor { Factor = -100, Predicate = clientProfileKladrPredicate1, Priority = 2 };
            var rule2CondFactor2 = new ConditionalFactor { Factor = -300, Predicate = clientProfileKladrPredicate2, Priority = 1 };
            rule2.SetConditionalFactors(new[] { rule2CondFactor1, rule2CondFactor2 });

            var rule3 = new Rule
                            {
                                Name = "Test Rule 3",
                                Factor = 0.5m,
                                Type = RuleTypes.Multiplication,
                                RuleDomainId = ruleDomain.Id,
                                Predicate = clientProfileAudiencesPredicate.Serialize(),
                                UpdatedUserId = "FSY"
                            };

            var ruleResult = new AdminMechanicsService().SaveRule(rule1, true, TestDataStore.TestUserId);
            Assert.AreEqual(ruleResult.Success, true);
            ruleResult = new AdminMechanicsService().SaveRule(rule2, true, TestDataStore.TestUserId);
            Assert.AreEqual(ruleResult.Success, true);
            ruleResult = new AdminMechanicsService().SaveRule(rule3, true, TestDataStore.TestUserId);
            Assert.AreEqual(ruleResult.Success, true);

            return ruleDomain;
        }
    }
}
