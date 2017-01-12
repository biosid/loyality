namespace RapidSoft.Loaylty.PromoAction.Tests.RuleCalculator
{
	using System.Collections.Generic;
	using System.Diagnostics.CodeAnalysis;

	using Microsoft.VisualStudio.TestTools.UnitTesting;

	using RapidSoft.Extensions;
	using RapidSoft.Loaylty.PromoAction.Api.Entities;
	using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
	using RapidSoft.Loaylty.PromoAction.Mechanics;
	using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
	using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class RuleCalculatorNotContainEquationEvalTests
	{
		[TestMethod]
		public void ShouldEvalTrue()
		{
			const string ContextValue = "WF1;-;WF5";
			var context = new Dictionary<string, string> { { "Obj.Prop", ContextValue } };

			var val1 = new value
						   {
							   type = valueType.attr,
							   attr = new[] { new valueAttr { @object = "Obj", name = "Prop", type = valueType.@string } }
						   };
			var val2 = new value { type = valueType.@string, Text = new[] { "WF2" } };
			var filter = new filter { Item = new equation { @operator = equationOperator.dncn, value = new[] { val1, val2 } } };

			var rule = new Rule { Predicate = filter.Serialize() };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

			var result = ruleCalculator.CalculateRule(rule);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, rule.Factor);
		}

		[TestMethod]
		public void ShouldEvalFalse()
		{
			const string ContextValue = "WF1;WF2;WF5";
			var context = new Dictionary<string, string> { { "Obj.Prop", ContextValue } };

			var val1 = new value
						   {
							   type = valueType.attr,
							   attr = new[] { new valueAttr { @object = "Obj", name = "Prop", type = valueType.@string } }
						   };
			var val2 = new value { type = valueType.@string, Text = new[] { "WF2" } };
			var filter = new filter { Item = new equation { @operator = equationOperator.dncn, value = new[] { val1, val2 } } };

			var rule = new Rule { Predicate = filter.Serialize() };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

			var result = ruleCalculator.CalculateRule(rule);

			Assert.AreEqual(result.Code, EvaluateResultCode.False);
		}

		[TestMethod]
		public void ShouldEvalTrueWithNull()
		{
			var context = new Dictionary<string, string> { { "Obj.Prop", null } };

			var val1 = new value
			{
				type = valueType.attr,
				attr = new[] { new valueAttr { @object = "Obj", name = "Prop", type = valueType.@string } }
			};
			var val2 = new value { type = valueType.@string, Text = new[] { "WF2" } };
			var filter = new filter { Item = new equation { @operator = equationOperator.dncn, value = new[] { val1, val2 } } };

			var rule = new Rule { Predicate = filter.Serialize() };

            var ruleCalculator = new RuleCalculator(new EvaluationSettings(TestHelper2.MockTracer, context));

			var result = ruleCalculator.CalculateRule(rule);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, rule.Factor);
		}
	}
}
