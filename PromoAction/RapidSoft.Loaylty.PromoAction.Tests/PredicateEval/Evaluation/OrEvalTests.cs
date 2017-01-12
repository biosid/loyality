namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class OrEvalTests
    {
        [TestMethod]
        public void ShouldReturnTrue()
        {
            Assert.AreEqual(TestHelper2.BuildUnion("or", true, true).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);
            Assert.AreEqual(TestHelper2.BuildUnion("or", false, true).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);
            Assert.AreEqual(TestHelper2.BuildUnion("or", true, false).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);

            var innerUnion = TestHelper2.BuildUnion("or", true, true);

            Assert.AreEqual(TestHelper2.BuildUnion("or", true, true, innerUnion.Item).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);
        }

        [TestMethod]
        public void ShouldReturnFalse()
        {
            Assert.AreEqual(TestHelper2.BuildUnion("or", false, false).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);

            var innerUnion = TestHelper2.BuildUnion("or", false, false);

            Assert.AreEqual(TestHelper2.BuildUnion("or", false, false, innerUnion.Item).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);
        }

        [TestMethod]
        public void ShouldReturnSql()
        {
            var result = TestHelper2.BuildUnionWithAttr("or", false).EvaluatePredicate(TestHelper2.SettingsAliases);
            Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
            Assert.AreEqual(TestHelper2.Convert(result), "table.column=" + TestHelper2.OtherNumeric);
        }

        [TestMethod]
        public void ShouldReturnSqlWithInnerUnion()
        {
            var union = TestHelper2.BuildUnionWithAttr("or", false);
            var result = TestHelper2.BuildUnionWithAttr("or", false, union.Item as union).EvaluatePredicate(TestHelper2.SettingsAliases);
            Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);

            // TODO: Избавится от такого дублирования.
            Assert.AreEqual(TestHelper2.Convert(result), "(table.column=" + TestHelper2.OtherNumeric + " OR table.column=" + TestHelper2.OtherNumeric + ")");
        }

        [TestMethod]
        public void ShouldNotReturnSqlBecauseLazyCalculated()
        {
            var union = TestHelper2.BuildUnionWithAttr("or", true);

            var result2 = TestHelper2.BuildUnionWithAttr("or", true, union.Item as union).EvaluatePredicate(TestHelper2.SettingsAliases);

            Assert.AreEqual(result2.Code, EvaluateResultCode.True);
        }
    }
}
