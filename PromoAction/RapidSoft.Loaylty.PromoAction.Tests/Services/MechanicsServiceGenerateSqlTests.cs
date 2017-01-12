namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.InputParameters;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Service;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class MechanicsServiceGenerateSqlTests
    {
        private const string Alias = "p.P1";
        private readonly string ruleDomainName = Guid.NewGuid().ToString();
        private readonly IDictionary<string, string> context = 
            new Dictionary<string, string>
                {
                    { "Product.City", "5" },
                    { "ClientProfile.Audiences", "TEST_TA;TEST_TA2;TEST_TA3" } 
                };

        private readonly RuleDomain domain = new RuleDomain { LimitType = LimitTypes.Fixed, LimitFactor = 50 };

        private readonly IRuleDomainRepository ruleDomainRepo = new MockRuleDomainRepository(new RuleDomain());

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
        public void ShouldReturnInitalNumberBecauseRuleDomainNotFound()
        {
            var ruleDomainRepository = new MockRuleDomainRepository(null);
            var service = new MechanicsService(new MockRuleRepository(null), ruleDomainRepository);

            var param = new GenerateSqlParameters(this.ruleDomainName, Alias, this.context, null);

            var result = service.GenerateSql(param);

            Assert.IsNull(result.BaseSql);
            Assert.IsNull(result.ActionSql);
            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesNotExecuted);
            Assert.AreEqual(result.ResultDescription, "Домен правил не найден");
        }
        
        [TestMethod]
        public void ShouldReturnCalculatedNumberWhenFixedLimitType()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var mockRuleRepo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var mockDomainRepo = new MockRuleDomainRepository(this.domain);

            var service = new MechanicsService(mockRuleRepo, mockDomainRepo);

            var param = new GenerateSqlParameters(this.ruleDomainName, Alias, this.context, null);

            var result = service.GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreNotEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);

            Assert.AreEqual(result.BaseSql, "p.P1 * 1.1 + 2.2");
            Assert.AreEqual(result.ActionSql, "CASE WHEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) > (0) THEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) ELSE 50 END");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldReturnCalculatedNumberWhenProcentLimitType()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var mockRuleRepo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var mockDomainRepo =
                new MockRuleDomainRepository(new RuleDomain { LimitType = LimitTypes.Percent, LimitFactor = 50 });

            var service = new MechanicsService(mockRuleRepo, mockDomainRepo);

            var param = new GenerateSqlParameters(this.ruleDomainName, Alias, this.context, null);

            var result = service.GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreNotEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);

            Assert.AreEqual(result.BaseSql, "p.P1 * 1.1 + 2.2");
            Assert.AreEqual(result.ActionSql, "CASE WHEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) > 0 THEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) ELSE (p.P1 * 50 / 100) END");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldReturnTSqlNumber()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml");

            var mockDomainRepo = new MockRuleDomainRepository(this.domain);
            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), mockDomainRepo);

            var param = new GenerateSqlParameters(
                "zxbxcvbxvb",
                Alias,
                new Dictionary<string, string>(),
                new Dictionary<string, string> { { "Product.City", "PriceRUR" } });

            var result = service.GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(result.BaseSql, "p.P1 * CASE WHEN PriceRUR=5 THEN 1 ELSE 1 END + 0");
            Assert.AreEqual(result.ActionSql, "CASE WHEN ((p.P1 * CASE WHEN PriceRUR=5 THEN 1 ELSE 1 END + 0) * 1 + 0) > (0) THEN ((p.P1 * CASE WHEN PriceRUR=5 THEN 1 ELSE 1 END + 0) * 1 + 0) ELSE 50 END");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldReturnTSqlNumber2()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml");

            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), ruleDomainRepository);

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", null, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(result.BaseMultiplicationSql, "CASE WHEN Product.City=5 THEN 1 ELSE 1 END");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldReturnNotCalculatedNumber()
        {
            const int Priority = 5;
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml", priority: Priority);

            var repo = new MockRuleRepository(new List<Rule> { rule, rule });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", null, null);

            Assert.AreEqual(result.ResultCode, 1);
            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesNotExecuted);
            Assert.IsTrue(result.ResultDescription.Contains("Должно быть только одно правило по приоритету"));
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationCnStringOrEqString.xml", "FilterXMLs")]
        public void ShouldReturnTSqlWithCalculatedContainTA()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationCnStringOrEqString.xml");

            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), this.ruleDomainRepo);

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", this.context, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(
                result.BaseMultiplicationSql,
                "1",
                "Так как входит в целевую аудиторию и объединение ИЛИ, то вторую часть с User.Name не вычисляем (ленивое вычисление)");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationCnStringOrEqString.xml", "FilterXMLs")]
        public void ShouldReturnTSqlWithoutCalculatedContainTA()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationCnStringOrEqString.xml");
            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), this.ruleDomainRepo);

            var context2 = new Dictionary<string, string>
                              {
                                  { "Product.City", "5" },
                                  { "ClientProfile.Audiences", "ANOTHER_TA, ANOTHER_TA2" }
                              };

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", context2, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(
                result.BaseMultiplicationSql,
                "CASE WHEN User.Name='Вася' THEN 1 ELSE 1 END",
                "Так как НЕ входит в целевую аудиторию, то вторую часть с User.Name конвертим");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationDnCnStringAndEqString.xml", "FilterXMLs")]
        public void ShouldReturnTSqlWithCalculatedDontContainTA()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationDnCnStringAndEqString.xml");
            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), this.ruleDomainRepo);

            var context2 = new Dictionary<string, string>
                              {
                                  { "Product.City", "5" },
                                  { "ClientProfile.Audiences", "ANOTHER_TA, ANOTHER_TA2" }
                              };

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", context2, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(
                result.BaseMultiplicationSql,
                "CASE WHEN User.Name='Вася' THEN 1 ELSE 1 END",
                "Так как входит в НЕ целевую аудиторию и объединение И, то вторую часть с User.Name вычисляем");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationDnCnStringAndEqString.xml", "FilterXMLs")]
        public void ShouldReturnTSqlWithoutCalculatedDontContainTA()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationDnCnStringAndEqString.xml");
            var service = new MechanicsService(new MockRuleRepository(new List<Rule> { rule }), this.ruleDomainRepo);

            var result = service.GenerateSqlWithoutLimit("zxbxcvbxvb", this.context, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);

            Assert.AreEqual(
                result.BaseMultiplicationSql,
                "1",
                "Так как входит в целевую аудиторию и объединение И, то вторую часть с User.Name не вычисляем (ленивое вычисление)");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldReturnCalculatedNumberWhenFixedLimitTypeWithFixedStopLimitType()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var domain = new RuleDomain
                             {
                                 LimitType = LimitTypes.Fixed,
                                 LimitFactor = 50,
                                 StopLimitType = LimitTypes.Fixed,
                                 StopLimitFactor = 22,
                             };

            var mockRuleRepo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var mockDomainRepo = new MockRuleDomainRepository(domain);

            var service = new MechanicsService(mockRuleRepo, mockDomainRepo);

            var param = new GenerateSqlParameters(this.ruleDomainName, Alias, this.context, null);

            var result = service.GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreNotEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);

            Assert.AreEqual(result.BaseSql, "p.P1 * 1.1 + 2.2");
            Assert.AreEqual(result.ActionSql, "CASE WHEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) > (22) THEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) ELSE 50 END");

            Assert.IsNull(result.ResultDescription);
        }

        [TestMethod]
        public void ShouldReturnCalculatedNumberWhenProcentLimitTypeWithProcentStopLimitType()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var domain = new RuleDomain
                             {
                                 LimitType = LimitTypes.Percent,
                                 LimitFactor = 50,
                                 StopLimitType = LimitTypes.Percent,
                                 StopLimitFactor = 22,
                             };

            var mockRuleRepo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var mockDomainRepo =
                new MockRuleDomainRepository(domain);

            var service = new MechanicsService(mockRuleRepo, mockDomainRepo);

            var param = new GenerateSqlParameters(this.ruleDomainName, Alias, this.context, null);

            var result = service.GenerateSql(param);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreNotEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);

            Assert.AreEqual(result.BaseSql, "p.P1 * 1.1 + 2.2");
            Assert.AreEqual(result.ActionSql, "CASE WHEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) > (p.P1 * 22 / 100) THEN ((p.P1 * 1.1 + 2.2) * 3.3 + 4.4) ELSE (p.P1 * 50 / 100) END");

            Assert.IsNull(result.ResultDescription);
        }

        // NOTE: Тест создан по багу VTBPLK-1149
        [TestMethod]
        // ReSharper disable InconsistentNaming
        public void ShouldCalculateWith_p_CategoryId()
        // ReSharper restore InconsistentNaming
        {
            // NOTE: Создаем домен 
            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);

            // NOTE: Создаем предикат
            var predicate =
                @"<filter xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <equation operator=""eq"">
    <value type=""attr"">
      <attr object=""p"" name=""CategoryId"" type=""numeric"" />
    </value>
    <value type=""numeric"">12345</value>
  </equation>
</filter>";

            // NOTE: Создаем правило
            var rule = TestHelper.BuildTestRule(type: RuleTypes.BaseAddition, coef: 5000, ruleDomainId: ruleDomain.Id);
            rule.Predicate = predicate;

            var adminMechanicsService = new AdminMechanicsService();
            var result1 = adminMechanicsService.SaveRule(rule, true, TestDataStore.TestUserId);

            Assert.AreEqual(true, result1.Success);

            var mechanicsService = new MechanicsService();

            var contextLocal2 = new Dictionary<string, string> { { "p.CategoryId", "12345" } };
            var param2 = new GenerateSqlParameters
                             {
                                 RuleDomainName = ruleDomain.Name,
                                 InitialNumberAlias = "p.PriceRUR",
                                 Context = contextLocal2,
                                 Aliases = null
                             };
            var result2 = mechanicsService.GenerateSql(param2);

            Assert.AreEqual(true, result2.Success);
            Assert.AreEqual("p.PriceRUR * 1 + 5000", result2.BaseSql, "Правило вычисляемо и true по контексту поэтому должно быть \"+ 5000\"");
            Assert.AreEqual("CASE " + 
                "WHEN ((p.PriceRUR * 1 + 5000) * 1 + 0) > 0 " +
                "THEN ((p.PriceRUR * 1 + 5000) * 1 + 0) " +
                "ELSE (p.PriceRUR * 50 / 100) " +
                "END", 
                result2.ActionSql);

            var contextLocal3 = new Dictionary<string, string> { { "p.CategoryId", "54321" } };
            var param3 = new GenerateSqlParameters
                             {
                                 RuleDomainName = ruleDomain.Name,
                                 InitialNumberAlias = "p.PriceRUR",
                                 Context = contextLocal3,
                                 Aliases = null
                             };
            var result3 = mechanicsService.GenerateSql(param3);

            Assert.AreEqual(true, result3.Success);
            Assert.AreEqual("p.PriceRUR * 1 + 0", result3.BaseSql, "Правило вычисляемо и false по контексту поэтому НЕ должно быть \"+ 5000\"");
            Assert.AreEqual("CASE "+
                "WHEN ((p.PriceRUR * 1 + 0) * 1 + 0) > 0 " + 
                "THEN ((p.PriceRUR * 1 + 0) * 1 + 0) " +
                "ELSE (p.PriceRUR * 50 / 100) " +
                "END", 
                result3.ActionSql);

            var param4 = new GenerateSqlParameters
                             {
                                 RuleDomainName = ruleDomain.Name,
                                 InitialNumberAlias = "p.PriceRUR",
                                 Context = null,
                                 Aliases = null
                             };
            var result4 = mechanicsService.GenerateSql(param4);

            Assert.AreEqual(true, result4.Success);
            Assert.AreEqual("p.PriceRUR * 1 + CASE WHEN p.CategoryId=12345 THEN 5000 ELSE 0 END", result4.BaseSql);
            Assert.AreEqual("CASE " +
                "WHEN ((p.PriceRUR * 1 + CASE WHEN p.CategoryId=12345 THEN 5000 ELSE 0 END) * 1 + 0) > 0 " +
                "THEN ((p.PriceRUR * 1 + CASE WHEN p.CategoryId=12345 THEN 5000 ELSE 0 END) * 1 + 0) " +
                "ELSE (p.PriceRUR * 50 / 100) " +
                "END", 
                result4.ActionSql);
        }
    }
}
