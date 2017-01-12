namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Service;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class IntegrationTests
    {
        private readonly List<long> ids = new List<long>();

        [TestCleanup]
        public void MyTestCleanup()
        {
            TestHelper.DeleteTestRuleDomain(this.ids);
        }

        #region IMechanicsService
        [TestMethod]
        public void ShouldConvertWithoutCondFactors()
        {
            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);
            TestHelper.InsertTestRule(ruleDomain, "EquationEqNumeric.xml", RuleTypes.BaseMultiplication, 500, 1.1m);

            var service = new MechanicsService();

            var result = service.GenerateSqlWithoutLimit(ruleDomain.Name, null, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.IsNull(result.ResultDescription);

            Assert.AreEqual(result.BaseMultiplicationSql, "CASE WHEN Product.City=5 THEN 1.1 ELSE 1 END");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        [DeploymentItem("FilterXMLs\\EquationNotEqNumeric.xml", "FilterXMLs")]
        [DeploymentItem("FilterXMLs\\XMLFile1.xml", "FilterXMLs")]
        public void ShouldConvertWithCondFactors()
        {
            var ruleDomain = TestHelper.InsertTestRuleDomain();
            this.ids.Add(ruleDomain.Id);

            var condFactors = new[]
                                  {
                                      new ConditionalFactor
                                          {
                                              Factor = 1.2m,
                                              Predicate = TestHelper.ReadPredicate("EquationEqNumeric.xml"),
                                              Priority = 5
                                          },
                                      new ConditionalFactor
                                          {
                                              Factor = 1.3m,
                                              Predicate = TestHelper.ReadPredicate("EquationNotEqNumeric.xml"),
                                              Priority = 6
                                          }
                                  };

            var rule = TestHelper.InsertTestRule(ruleDomain, "XMLFile1.xml", RuleTypes.BaseMultiplication, 500, 1.1m, condFactors);
            Assert.AreNotEqual(rule.Id, 0);

            var service = new MechanicsService();

            var result = service.GenerateSqlWithoutLimit(ruleDomain.Name, null, null);

            Assert.AreEqual(result.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.IsNull(result.ResultDescription);

            Assert.AreEqual(
                result.BaseMultiplicationSql, 
                "CASE " +
                    "WHEN (Product.City=6 OR Product.City=5) " +
                        "THEN " + 
                            "CASE " +
                                "WHEN Product.City!=5 THEN 1.3 " +
                                "WHEN Product.City=5 THEN 1.2 " +
                                "ELSE 1.1 " +
                            "END " +
                    "ELSE 1 " +
                "END");
            Assert.AreEqual(result.BaseAdditionSql, "0");
            Assert.AreEqual(result.MultiplicationSql, "1");
            Assert.AreEqual(result.AdditionSql, "0");
        }
        #endregion
    }
}
