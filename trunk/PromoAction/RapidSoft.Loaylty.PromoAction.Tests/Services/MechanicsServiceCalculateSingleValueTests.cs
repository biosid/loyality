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
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Service;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;
    using RapidSoft.VTB24.ArmSecurity;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class MechanicsServiceCalculateSingleValueTests
    {
        private readonly string ruleDomainName = Guid.NewGuid().ToString();

        private readonly IDictionary<string, string> context = new Dictionary<string, string>
                                                                   {
                                                                       { "Product.City", "5" }
                                                                   };

        private readonly List<long> ids = new List<long>();
        private readonly IRuleDomainRepository ruleDomainRepo = new MockRuleDomainRepository(null);
        private Dictionary<string, string> clientContext = new Dictionary<string, string>();

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
            var service = new MechanicsService(new MockRuleRepository(null), this.ruleDomainRepo);

            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, clientContext);

            //Assert.AreEqual(result.PromoResult, initalValue);
            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesNotExecuted);
            Assert.AreEqual(result.ResultDescription, "Домен правил не найден");
            TestHelper2.WriteTrace(result.TraceMessages);
        }
        
        [TestMethod]
        public void ShouldReturnCalculatedNumber()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var repo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);
            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, this.context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, (((initalValue * Fac1) + Fac2) * Fac3) + Fac4);
            Assert.IsNull(result.ResultDescription);
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnCalculatedFactors()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = TestHelper.GetTruePredicate() };
            var rule3 = new Rule { Factor = Fac3, Type = RuleTypes.Multiplication, Predicate = TestHelper.GetTruePredicate() };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var repo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);

            var result = service.CalculateFactors(this.ruleDomainName, this.context);
            
            Assert.AreEqual(Fac1, result.BaseMultiplicationFactor);
            Assert.AreEqual(Fac2, result.BaseAdditionFactor);
            Assert.AreEqual(Fac3, result.MultiplicationFactor);
            Assert.AreEqual(Fac4, result.AdditionFactor);

            Assert.IsNull(result.ResultDescription);
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnCalculatedNumberByRuleWithoutPredicate()
        {
            const decimal Fac1 = 1.1m;
            const decimal Fac2 = 2.2m;
            const decimal Fac3 = 3.3m;
            const decimal Fac4 = 4.4m;
            var rule1 = new Rule { Factor = Fac1, Type = RuleTypes.BaseMultiplication, Predicate = null };
            var rule2 = new Rule { Factor = Fac2, Type = RuleTypes.BaseAddition, Predicate = null };
            var rule3 = new Rule
                {
                    Factor = Fac3,
                    Type = RuleTypes.Multiplication,
                    Predicate = (new filter() { Item = TestHelper2.BuildFalseEquation() }).Serialize()
                };
            var rule4 = new Rule { Factor = Fac4, Type = RuleTypes.Addition, Predicate = TestHelper.GetTruePredicate() };

            var repo = new MockRuleRepository(new List<Rule> { rule1, rule2, rule3, rule4 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);
            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, this.context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(result.PromoResult, ((initalValue * Fac1) + Fac2) + Fac4);
            Assert.IsNull(result.ResultDescription);
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnNotCalculatedNumber()
        {
            const int Priority = 5;
            const decimal Fac1 = 1.1m;
            var rule1 = new Rule
                            {
                                Factor = Fac1,
                                Priority = Priority,
                                Type = RuleTypes.BaseMultiplication,
                                Predicate = TestHelper.GetTruePredicate()
                            };

            var repo = new MockRuleRepository(new List<Rule> { rule1, rule1 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);
            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, this.context);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesNotExecuted);
            //Assert.AreEqual(result.PromoResult, initalValue);
            Assert.IsTrue(result.ResultDescription.Contains("Должно быть только одно правило по приоритету"));
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEmNumeric.xml", "FilterXMLs")]
        public void ShouldReturnNotCalculatedNumber2()
        {
            const int Priority = 5;
            const decimal Fac1 = 1.1m;
            var rule1 = new Rule
                            {
                                Factor = Fac1,
                                Priority = Priority,
                                Type = RuleTypes.BaseMultiplication,
                                Predicate = TestHelper.ReadFile("EquationEmNumeric.xml")
                            };

            var repo = new MockRuleRepository(new List<Rule> { rule1 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);
            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, clientContext);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesNotExecuted);
            Assert.IsTrue(result.ResultDescription.Contains("Вычисление коэффициентов не возможно, в контексте не достаточно данных"));
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnOnlyBaseRulesExecuted()
        {
            var value1 = new value { type = valueType.@string, Text = new[] { "12345" } };
            var value2 = new value { type = valueType.@string, Text = new[] { "54321" } };

            var filterTrue = new filter { Item = new equation { @operator = equationOperator.eq, value = new[] { value1, value1 } } };
            var filterFalse = new filter { Item = new equation { @operator = equationOperator.eq, value = new[] { value1, value2 } } };

            var rule1 = new Rule { Factor = 8.8m, Type = RuleTypes.BaseAddition, Predicate = filterTrue.Serialize() };
            var rule2 = new Rule { Factor = 8.8m, Type = RuleTypes.Addition, Predicate = filterFalse.Serialize() };

            var repo = new MockRuleRepository(new List<Rule> { rule1, rule2 });
            var ruleDomainRepository = new MockRuleDomainRepository(new RuleDomain());

            var service = new MechanicsService(repo, ruleDomainRepository);
            var initalValue = Convert.ToDecimal(new Random().Next(100));

            var result = service.CalculateSingleValue(this.ruleDomainName, initalValue, clientContext);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(result.PromoResult, initalValue + 8.8m);
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnFixedLimitedNumber()
        {
            const decimal LimitFactor = 2;
            var value1 = new value { type = valueType.@string, Text = new[] { "12345" } };
            var filterTrue = new filter { Item = new equation { @operator = equationOperator.eq, value = new[] { value1, value1 } } };
            var rule1 = new Rule { Factor = -15000m, Type = RuleTypes.BaseAddition, Predicate = filterTrue.Serialize() };
            var domain = new RuleDomain { LimitType = LimitTypes.Fixed, LimitFactor = LimitFactor };

            var ruleRepo = new MockRuleRepository(new List<Rule> { rule1 });
            var domainRepo = new MockRuleDomainRepository(domain);

            var service = new MechanicsService(ruleRepo, domainRepo);

            var result = service.CalculateSingleValue(this.ruleDomainName, 50, clientContext);

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(result.PromoResult, LimitFactor);
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnFixedLimitedNumberWithStopFixedLimit()
        {
            const decimal LimitFactor = 110;
            const decimal StopLimitFactor = 100;

            const decimal RuleFactor = -50m;
            var rule1 = new Rule { Factor = RuleFactor, Type = RuleTypes.BaseAddition, Predicate = null };

            // NOTE: Если получено значение меньше StopLimitFactor, до должно вернуть LimitFactor
            var domain = new RuleDomain
                             {
                                 LimitType = LimitTypes.Fixed,
                                 LimitFactor = LimitFactor,
                                 StopLimitType = LimitTypes.Fixed,
                                 StopLimitFactor = StopLimitFactor
                             };

            var ruleRepo = new MockRuleRepository(new List<Rule> { rule1 });
            var domainRepo = new MockRuleDomainRepository(domain);

            var service = new MechanicsService(ruleRepo, domainRepo);

            var result = service.CalculateSingleValue(this.ruleDomainName, 155, new Dictionary<string, string>());

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(155 + RuleFactor, result.PromoResult, "Стоп лимит НЕ должен сработать");
            TestHelper2.WriteTrace(result.TraceMessages);

            result = service.CalculateSingleValue(this.ruleDomainName, 145, new Dictionary<string, string>());

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(LimitFactor, result.PromoResult, "Стоп лимит должен сработать");
            TestHelper2.WriteTrace(result.TraceMessages);
        }

        [TestMethod]
        public void ShouldReturnPercentLimitedNumberWithPercentStopLimit()
        {
            const decimal LimitFactor = 10;
            const decimal StopLimitFactor = 15;

            const decimal RuleFactor1 = -50m;
            var rule1 = new Rule { Factor = RuleFactor1, Type = RuleTypes.BaseAddition, Predicate = null };

            // NOTE: Если получено значение меньше StopLimitFactor процентов от числа, до должно вернуть LimitFactor процентов от числа
            var domain = new RuleDomain
                             {
                                 LimitType = LimitTypes.Percent,
                                 LimitFactor = LimitFactor,
                                 StopLimitType = LimitTypes.Percent,
                                 StopLimitFactor = StopLimitFactor
                             };

            var ruleRepo = new MockRuleRepository(new List<Rule> { rule1 });
            var domainRepo = new MockRuleDomainRepository(domain);

            var service = new MechanicsService(ruleRepo, domainRepo);

            var result = service.CalculateSingleValue(this.ruleDomainName, 100, new Dictionary<string, string>());

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(100 + RuleFactor1, result.PromoResult, "Стоп лимит НЕ должен сработать");
            TestHelper2.WriteTrace(result.TraceMessages);

            const decimal RuleFactor2 = -90m;
            var rule2 = new Rule { Factor = RuleFactor2, Type = RuleTypes.BaseAddition, Predicate = null };
            var ruleRepo2 = new MockRuleRepository(new List<Rule> { rule2 });
            var service2 = new MechanicsService(ruleRepo2, domainRepo);

            result = service2.CalculateSingleValue(this.ruleDomainName, 100, new Dictionary<string, string>());

            Assert.AreEqual(result.Success, true);
            Assert.AreEqual(100 * LimitFactor / 100, result.PromoResult, "Стоп лимит должен сработать");
            TestHelper2.WriteTrace(result.TraceMessages);
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
            var result2 = mechanicsService.CalculateSingleValue(ruleDomain.Name, 200, contextLocal2);

            Assert.AreEqual(true, result2.Success);
            Assert.AreEqual(200 + 5000, result2.PromoResult);
            TestHelper2.WriteTrace(result2.TraceMessages);

            var contextLocal3 = new Dictionary<string, string> { { "p.CategoryId", "54321" } };
            var result3 = mechanicsService.CalculateSingleValue(ruleDomain.Name, 200, contextLocal3);

            Assert.AreEqual(true, result3.Success);
            Assert.AreEqual(200, result3.PromoResult);
            TestHelper2.WriteTrace(result3.TraceMessages);
        }
    }
}
