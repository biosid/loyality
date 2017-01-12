namespace RapidSoft.Loaylty.PromoAction.Tests.RuleCalculator
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class RuleResultTests
	{
		[TestMethod]
		public void ShouldRenderFalseRuleResult()
		{
			var result = RuleResult.BuildFalse(RuleTypes.Multiplication);

			Assert.AreEqual(result.Code, EvaluateResultCode.False);

			Assert.AreEqual(result.Factor, 1);

			Assert.AreEqual(TestHelper2.Convert(result), "1");
		}

		[TestMethod]
		public void ShouldRenderTrueRuleResultWithoutCondFactors()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Coeff = 5.3m;
			var result = RuleResult.BuildTrue(RuleType, Coeff);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);

			Assert.AreEqual(result.Factor, Coeff);
			Assert.AreEqual(result.IsFactorAvaible, true);
			Assert.AreEqual(TestHelper2.Convert(result), "5.3");
		}

		[TestMethod]
		public void ShouldRenderTrueRuleResultWithOneTrueCondFactor()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Coeff = 5.3m;
			const decimal CondCoeff = 6.999m;

			var condResults = new[] { ConditionalResult.BuildTrue(CondCoeff) };

			var result = RuleResult.BuildTrue(RuleType, Coeff, condResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);

			Assert.AreEqual(result.Factor, CondCoeff);
			Assert.AreEqual(result.IsFactorAvaible, true);
			Assert.AreEqual(TestHelper2.Convert(result), "6.999");
		}

		[TestMethod]
		public void ShouldRenderTrueRuleResultWithOneConvertibleToSQLCondFactor()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Coeff = 5.3m;
			const decimal CondCoeff = 6.999m;

			var condResults = new[] { ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ"), CondCoeff) };

			var result = RuleResult.BuildTrue(RuleType, Coeff, condResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN УСЛ.ПРЕДИКАТ THEN 6.999 ELSE 5.3 END");
		}

		[TestMethod]
		public void ShouldRenderTrueRuleResultWithManyConvertibleToSQLCondFactor()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Coeff = 5.3m;
			const decimal CondCoeff1 = 6.999m;
			const decimal CondCoeff2 = 1.00001m;

			var condResults = new[]
				                  {
					                  ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ1"), CondCoeff1),
					                  ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ2"), CondCoeff2)
				                  };

			var result = RuleResult.BuildTrue(RuleType, Coeff, condResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.True);
			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN УСЛ.ПРЕДИКАТ1 THEN 6.999 WHEN УСЛ.ПРЕДИКАТ2 THEN 1.00001 ELSE 5.3 END");
		}

		[TestMethod]
		public void ShouldRenderConvertibleRuleResultWithoutCondFactors()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal Factor = 5.3m;
			var predicateEval = new MockConverter("ПРЕДИКАТ");

			var result = RuleResult.BuildConvertible(RuleType, predicateEval, Factor);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);

			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРЕДИКАТ THEN 5.3 ELSE 0 END");
		}

		[TestMethod]
		public void ShouldRenderConvertibleRuleResultWithOneTrueCondFactor()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;
			const decimal Coeff = 5.3m;
			const decimal CondCoeff = 6.999m;
			var predicateEval = new MockConverter("ПРЕДИКАТ");

			var condResults = new[] { ConditionalResult.BuildTrue(CondCoeff) };

			var result = RuleResult.BuildConvertible(RuleType, predicateEval, Coeff, condResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, 1);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРЕДИКАТ THEN 6.999 ELSE 1 END");
		}

		[TestMethod]
		public void ShouldRenderConvertibleRuleResultWithConvertibleTrueCondFactor()
		{
			const RuleTypes RuleType = RuleTypes.Multiplication;
			const decimal Coeff = 5.3m;
			const decimal CondCoeff = 6.999m;

			var condResults = new[]
				                  {
					                  ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ. ПРЕДИКАТ"), CondCoeff)
				                  };

			var result = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), Coeff, condResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, 1);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРЕДИКАТ THEN CASE WHEN УСЛ. ПРЕДИКАТ THEN 6.999 ELSE 5.3 END ELSE 1 END");
		}

		[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
		[TestMethod]
		public void ShouldRenderValidCaseWithAllConvertable()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal RuleFactor = 56.7m;
			const decimal CondFactor1 = 0.7834m;
			const decimal ConfFactor2 = 0.777m;

			var condRuleResults = new[]
				                      {
					                      ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ1"), CondFactor1),
					                      ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ.ПРЕДИКАТ2"), ConfFactor2)
				                      };

			var result = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), RuleFactor, condRuleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(
				TestHelper2.Convert(result), 
					"CASE " + 
						"WHEN ПРЕДИКАТ " + 
							"THEN " +
								"CASE " +
									"WHEN УСЛ.ПРЕДИКАТ1 THEN 0.7834 " +
									"WHEN УСЛ.ПРЕДИКАТ2 THEN 0.777 " + 
									"ELSE 56.7 " +
								"END " +
						"ELSE 0 " +
					"END");
		}

		[TestMethod]
		public void ShouldRenderValidCaseWithTrueCond()
		{
			const RuleTypes RuleType = RuleTypes.Addition;
			const decimal RuleFactor = 56.7m;
			const decimal CondFactor1 = 0.7834m;
			const decimal ConfFactor2 = 0.777m;

			var condRuleResults = new[]
				                      {
					                      ConditionalResult.BuildConvertible(RuleType, new MockConverter("УСЛ. ПРЕДИКАТ"), CondFactor1), 
										  ConditionalResult.BuildTrue(ConfFactor2)
				                      };

			var result = RuleResult.BuildConvertible(RuleType, new MockConverter("ПРЕДИКАТ"), RuleFactor, condRuleResults);

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(result.Factor, 0);
			Assert.AreEqual(result.IsFactorAvaible, false);
			Assert.AreEqual(TestHelper2.Convert(result), "CASE WHEN ПРЕДИКАТ THEN CASE WHEN УСЛ. ПРЕДИКАТ THEN 0.7834 ELSE 0.777 END ELSE 0 END");
		}
	}
}
