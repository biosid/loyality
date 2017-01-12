namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class LikeEquationTests
	{
		private readonly IEvalStrategySelector factory = new EvalStrategySelector(new VariableResolver(null, TestHelper2.Aliases));

		[TestMethod]
		public void ShouldReturnTrue()
		{
			Assert.IsTrue(new LikeEquation(TestHelper2.BuildTwoValue("фывапролджэ", "вапрол", valueType.@string), this.factory).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnSql()
		{
			var values = TestHelper2.BuildTwoValueWithAttr("вапрол", valueType.@string, aliases: TestHelper2.Aliases);
			var result = new LikeEquation(values, this.factory).EvaluateExt();

			Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
			Assert.AreEqual(TestHelper2.Convert(result), "table.column LIKE '%вапрол%'");
		}

		[TestMethod]
		public void ShouldReturnFalse()
		{
			Assert.IsFalse(new LikeEquation(TestHelper2.BuildTwoValue("фывапро", "ролджэ", valueType.@string), this.factory).Evaluate());
		}
	}
}