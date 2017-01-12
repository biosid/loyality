namespace RapidSoft.Loaylty.PromoAction.Tests.RuleCalculator
{
    using System;
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
    public class RuleCalculatorUnionEvalTests
    {
        private readonly ILog log = LogManager.GetLogger(typeof(RuleCalculatorUnionEvalTests));

        #region Union OR
        [TestMethod]
        [DeploymentItem("FilterXMLs\\XMLFile1.xml", "FilterXMLs")]
        public void ShouldEvalTrueXmlFile1Xml()
        {
            var rule = new Rule
                           {
                               Factor = 5.5m,
                               Id = 5,
                               InsertedDate = DateTime.Now,
                               Predicate = TestHelper.ReadFile("XMLFile1.xml"),
                               Priority = 5,
                               RuleDomain = null,
                               RuleDomainId = 5,
                               Type = RuleTypes.BaseAddition,
                               UpdatedUserId = "I"
                           };

            var context = new Dictionary<string, string> { { "Product.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

            var s = Stopwatch.StartNew();
            var result = ruleCalculator.CalculateRule(rule);
            s.Stop();

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, 5.5m);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }
        #endregion

        #region Union AND
        [TestMethod]
        [DeploymentItem("FilterXMLs\\XMLFile2.xml", "FilterXMLs")]
        public void ShouldEvalFalseXmlFile2Xml()
        {
            var rule = new Rule
            {
                Factor = 5.5m,
                Id = 5,
                InsertedDate = DateTime.Now,
                Predicate = TestHelper.ReadFile("XMLFile2.xml"),
                Priority = 5,
                RuleDomain = null,
                RuleDomainId = 5,
                Type = RuleTypes.BaseMultiplication,
                UpdatedUserId = "I"
            };

            var context = new Dictionary<string, string> { { "Product.City", "6" }, { "Man.City", "5" } };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

            var s = Stopwatch.StartNew();
            var result = ruleCalculator.CalculateRule(rule);
            s.Stop();

            Assert.AreEqual(result.Code, EvaluateResultCode.True);
            Assert.AreEqual(result.Factor, 5.5m);

            log.Debug("Время: " + s.ElapsedMilliseconds + " мс.");
        }
        #endregion
    }
}
