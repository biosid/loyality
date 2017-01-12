namespace RapidSoft.Loaylty.PromoAction.Tests.RuleCalculator
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleCalculatorWithConditionalCoefficientTests
    {
        [TestMethod]
        public void ShouldEvalWithoutConditionalCoefficient()
        {
            const int Coef = 5;
            var rule = new Rule { Factor = Coef, Predicate = TestHelper.ReadFile("EquationEqNumeric.xml") };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, new Dictionary<string, string> { { "Product.City", "5" } }, TestHelper2.Aliases));

            var ruleResult = ruleCalculator.EvaluatePredicate(rule.GetDeserializedPredicate());
            var resultExt = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(ruleResult.Code, EvaluateResultCode.True);
            Assert.AreEqual(resultExt.Factor, 5);
            Assert.AreEqual(resultExt.Code, EvaluateResultCode.True);
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        [DeploymentItem("FilterXMLs\\XMLFile1.xml", "FilterXMLs")]
        public void ShouldEvalWithConditionalCoefficientConvert()
        {
            const RuleTypes RuleType = RuleTypes.BaseMultiplication;
            const decimal Coef = 5.1m;
            const decimal CondCoef = 6.2m;
            var rule = new Rule { Factor = Coef, Type = RuleType, Predicate = TestHelper.ReadFile("EquationEqNumeric.xml") };
            var condCoefs = new[]
                                {
                                    new ConditionalFactor { Factor = CondCoef, Predicate = TestHelper.ReadPredicate("XMLFile1.xml") }
                                };
            rule.SetConditionalFactors(condCoefs);

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, TestHelper2.EmptyDic, TestHelper2.Aliases));

            var ruleResult = ruleCalculator.EvaluatePredicate(rule.GetDeserializedPredicate());
            var resultExt = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(ruleResult.Code, EvaluateResultCode.ConvertibleToSQL);

            Assert.AreEqual(resultExt.Code, EvaluateResultCode.ConvertibleToSQL);
            Assert.AreEqual(resultExt.Factor, RuleType.GetDefaultFactor());

            Assert.AreEqual(
                TestHelper2.Convert(resultExt), 
                "CASE " + 
                    "WHEN Product.City=5 THEN CASE WHEN (Product.City=6 OR Product.City=5) THEN 6.2 ELSE 5.1 END " + 
                    "ELSE 1 " +
                "END");
        }

        [TestMethod]
        public void ShouldEvalWithConditionalCoefficient()
        {
            const int Coef = 5;
            const int CondCoef = 6;
            var rule = new Rule { Factor = Coef, Predicate = TestHelper.ReadFile("EquationEqNumeric.xml") };
            var condCoefs = new[]
                                {
                                    new ConditionalFactor
                                        {
                                            Factor = CondCoef,
                                            Predicate = TestHelper.ReadPredicate("EquationEqNumeric.xml")
                                        }
                                };
            rule.SetConditionalFactors(condCoefs);

            var ruleCalculator =
                new RuleCalculator(
                    new EvaluationSettings(TestHelper2.MockTracer, new Dictionary<string, string> { { "Product.City", "5" } }, TestHelper2.Aliases));

            var ruleResult = ruleCalculator.EvaluatePredicate(rule.GetDeserializedPredicate());
            var resultExt = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(ruleResult.Code, EvaluateResultCode.True);

            Assert.AreEqual(resultExt.Code, EvaluateResultCode.True);
            Assert.AreEqual(resultExt.Factor, CondCoef);
        }
    }
}
