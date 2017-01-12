namespace RapidSoft.Loaylty.PromoAction.Tests.RuleCalculator
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class RuleCalculatorEquationEvalTests
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RuleCalculatorEquationEvalTests));

        #region Numeric
        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationEqNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml", coef: 5.5m);
            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

            var s = Stopwatch.StartNew();
            var result = ruleCalculator.CalculateRule(rule);
            s.Stop();

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, 5.5m);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationEqNumericXmlManyTimes()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml", coef: 5.5m);

            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

            RuleResult result = null;

            var s = Stopwatch.StartNew();
            for (var i = 0; i < 10; i++)
            {
                result = ruleCalculator.CalculateRule(rule);
            }

            s.Stop();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, 5.5m);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationEqNumericXmlManyTimes2()
        {
            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

            RuleResult result = null;

            var list = new List<Rule>();

            for (var i = 0; i < 100; i++)
            {
                list.Add(TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml", coef: 5.5m));
            }

            var s = Stopwatch.StartNew();
            foreach (var rule1 in list)
            {
                result = ruleCalculator.CalculateRule(rule1);
            }

            s.Stop();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, 5.5m);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationEqNumericXmlManyTimes3()
        {
            var alias = new Dictionary<string, string> { { "Product.City", "table.col" } };
            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, aliases: alias));

            RuleResult result = null;

            var list = new List<Rule>();

            for (var i = 0; i < 100; i++)
            {
                list.Add(TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml", coef: 5.5m));
            }

            var s = Stopwatch.StartNew();
            foreach (var rule1 in list)
            {
                result = ruleCalculator.CalculateRule(rule1);
            }

            s.Stop();

            Assert.IsNotNull(result);
            Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalFalseForEqNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEqNumeric.xml");
            var context = new Dictionary<string, string> { { "Product.City", "6" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.False);
            Assert.AreEqual(result.Factor, rule.Type.GetDefaultFactor());
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationNotEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationNotEqNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationNotEqNumeric.xml");
            var context = new Dictionary<string, string> { { "Product.City", "6" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, rule.Factor);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationNotEqNumeric.xml", "FilterXMLs")]
        public void ShouldEvalFalseForNotEqNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationNotEqNumeric.xml");
            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.False);
            Assert.AreEqual(result.Factor, rule.Type.GetDefaultFactor());
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEmNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationEmNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEmNumeric.xml");
            var context = new Dictionary<string, string> { { "Product.City", null } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, rule.Factor);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationEmNumeric.xml","FilterXMLs")]
        public void ShouldEvalFalseForEmNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationEmNumeric.xml");
            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.False);
            Assert.AreEqual(result.Factor, rule.Type.GetDefaultFactor());
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationNemNumeric.xml", "FilterXMLs")]
        public void ShouldEvalTrueEquationNemNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationNemNumeric.xml");

            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, rule.Factor);
        }

        [TestMethod]
        [DeploymentItem("FilterXMLs\\EquationNemNumeric.xml", "FilterXMLs")]
        public void ShouldEvalFalseForNemNumericXml()
        {
            var rule = TestHelper.BuildTestRuleWithPredicateFromFile("EquationNemNumeric.xml");

            var context = new Dictionary<string, string> { { "Product.City", null } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));
            var result = ruleCalculator.CalculateRule(rule);

            Assert.AreEqual(result.Code, EvaluateResultCode.False);
            Assert.AreEqual(result.Factor, rule.Type.GetDefaultFactor());
        }
        #endregion
    }
}
