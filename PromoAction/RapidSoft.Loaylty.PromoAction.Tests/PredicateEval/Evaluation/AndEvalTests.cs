namespace RapidSoft.Loaylty.PromoAction.Tests.PredicateEval.Evaluation
{
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class AndEvalTests
    {
        [TestMethod]
        public void ShouldReturnTrue()
        {
            Assert.AreEqual(TestHelper2.BuildUnion("and", true, true).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);

            var innerUnion = TestHelper2.BuildUnion("and", true, true);

            Assert.AreEqual(TestHelper2.BuildUnion("and", true, true, innerUnion.Item as union).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.True);
        }

        [TestMethod]
        public void ShouldReturnFalse()
        {
            Assert.AreEqual(TestHelper2.BuildUnion("and", false, true).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);
            Assert.AreEqual(TestHelper2.BuildUnion("and", true, false).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);
            Assert.AreEqual(TestHelper2.BuildUnion("and", false, false).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);

            var innerUnion = TestHelper2.BuildUnion("and", true, true);

            Assert.AreEqual(TestHelper2.BuildUnion("and", false, false, innerUnion.Item as union).EvaluatePredicate(TestHelper2.SettingsContext).Code, EvaluateResultCode.False);
        }

        [TestMethod]
        public void ShouldReturnSql()
        {
            var result = TestHelper2.BuildUnionWithAttr("and", true).EvaluatePredicate(TestHelper2.SettingsAliases);
            Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);
            Assert.AreEqual(TestHelper2.Convert(result), "table.column=" + TestHelper2.OtherNumeric);
        }

        [TestMethod]
        public void ShouldReturnSqlWithInnerUnion()
        {
            var union = TestHelper2.BuildUnionWithAttr("and", true);
            var result = TestHelper2.BuildUnionWithAttr("and", true, union.Item as union).EvaluatePredicate(TestHelper2.SettingsAliases);
            Assert.AreEqual(result.Code, EvaluateResultCode.ConvertibleToSQL);

            // TODO: Избавится от такого дублирования.
            Assert.AreEqual(TestHelper2.Convert(result), "(table.column=" + TestHelper2.OtherNumeric + " AND table.column=" + TestHelper2.OtherNumeric + ")");
        }

        [TestMethod]
        public void ShouldNotReturnSqlBecauseLazyCalculated()
        {
            var union = TestHelper2.BuildUnionWithAttr("and", false);

            var result2 = TestHelper2.BuildUnionWithAttr("and", false, union.Item as union).EvaluatePredicate(TestHelper2.Settings);

            Assert.AreEqual(result2.Code, EvaluateResultCode.False);
        }
    }
}
