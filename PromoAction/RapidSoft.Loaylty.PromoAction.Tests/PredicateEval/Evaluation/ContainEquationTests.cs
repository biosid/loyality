namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Evaluation;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
	[TestClass]
	public class ContainEquationTests
	{
		[TestMethod]
		public void ShouldReturnTrue()
		{
			var variable = VariableValue.BuildValue("23");
			var list = VariableValue.BuildValue(new[] { "12", "23", "34" });
			Assert.IsTrue(new ContainEquation(new[] { list, variable }).Evaluate());

			variable = VariableValue.BuildValue(5.5m);
			list = VariableValue.BuildValue(new[] { 5.40m, 5.50m, 5.60m });
			Assert.IsTrue(new ContainEquation(new[] { list, variable }).Evaluate());

			variable = VariableValue.BuildValue(true);
			list = VariableValue.BuildValue(new[] { true, false });
			Assert.IsTrue(new ContainEquation(new[] { list, variable }).Evaluate());

			var dt = new DateTime(2105, 4, 2, 5, 33, 26);
			variable = VariableValue.BuildValue(dt);
			list = VariableValue.BuildValue(new[] { dt, dt.AddDays(1), dt.AddDays(-1) });
			Assert.IsTrue(new ContainEquation(new[] { list, variable }).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnFalse()
		{
			var variable = VariableValue.BuildValue("23");
			var list = VariableValue.BuildValue(new[] { "12", "23-", "34" });
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());

			variable = VariableValue.BuildValue(5.5m);
			list = VariableValue.BuildValue(new[] { 5.40m, 5.51m, 5.60m });
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());

			variable = VariableValue.BuildValue(true);
			list = VariableValue.BuildValue(new[] { false, false });
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());

			var dt = new DateTime(2105, 4, 2, 5, 33, 26);
			variable = VariableValue.BuildValue(dt);
			list = VariableValue.BuildValue(new[] { dt.AddDays(1), dt.AddDays(-1) });
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());
		}

		[TestMethod]
		public void ShouldReturnFalseWithNull()
		{
			var variable = VariableValue.BuildValue(null);
			var list = VariableValue.BuildValue(new[] { "12", "23", "34" });
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());

			variable = VariableValue.BuildValue("23");
			list = VariableValue.BuildValue(null);
			Assert.IsFalse(new ContainEquation(new[] { list, variable }).Evaluate());
		}
	}
}
