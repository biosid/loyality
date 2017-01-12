using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.PromoAction.Tests
{
    using System.Diagnostics.CodeAnalysis;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Settings;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class RuleRuleExtensionsTests
    {
        private readonly value literal;
        private readonly valueAttr variableAttr;
        private readonly value variable;

        private readonly equation notPromoEquation;
        private readonly union notPromoUnion;
        private readonly union notPromoUnionOfUnion;

        private readonly valueAttr promoVariableAttr;
        private readonly value promoVariable;
        private readonly equation promoEquation;
        private readonly union promoUnion;
        private readonly union promoUnionOfUnion;

        private readonly value literalTA;

        private readonly equation promoEquationWithTA;
        private readonly union promoUnionWithTA;
        private readonly union promoUnionOfUnionWithTA;

        public RuleRuleExtensionsTests()
        {
            this.literal = new value { type = valueType.@string, Text = new[] { "Какое-то значение" } };
            this.variableAttr = new valueAttr { @object = "Объект", name = "Свойство", type = valueType.@string };
            this.variable = new value { type = valueType.attr, attr = new[] { this.variableAttr } };
            this.notPromoEquation = new equation { value = new[] { this.variable, this.literal } };

            this.notPromoUnion = new union { equation = new[] { this.notPromoEquation, this.notPromoEquation } };
            this.notPromoUnionOfUnion = new union
                                            {
                                                equation = new[] { this.notPromoEquation },
                                                union1 = new[] { this.notPromoUnion, this.notPromoUnion }
                                            };

            this.promoVariableAttr = new valueAttr
                                         {
                                             @object = ApiSettings.ClientProfileObjectName,
                                             name = ApiSettings.PromoActionPropertyName,
                                             type = valueType.@string
                                         };
            this.promoVariable = new value { type = valueType.attr, attr = new[] { this.promoVariableAttr } };
            this.promoEquation = new equation { value = new[] { this.promoVariable, this.literal } };

            this.promoUnion = new union { equation = new[] { this.notPromoEquation, this.promoEquation } };
            this.promoUnionOfUnion = new union
                                         {
                                             equation = new[] { this.notPromoEquation },
                                             union1 = new[] { this.notPromoUnion, this.promoUnion }
                                         };

            this.literalTA = new value
                                 {
                                     type = valueType.@string,
                                     Text = new[] { ApiSettings.TargetAudienceLiteralPrefix + "ID" }
                                 };

            this.promoEquationWithTA = new equation { value = new[] { this.promoVariable, this.literalTA } };

            this.promoUnionWithTA = new union
                                        {
                                            equation =
                                                new[]
                                                    {
                                                        this.notPromoEquation, this.promoEquation,
                                                        this.promoEquationWithTA
                                                    }
                                        };
            this.promoUnionOfUnionWithTA = new union
                                               {
                                                   equation = new[] { this.notPromoEquation },
                                                   union1 =
                                                       new[]
                                                           {
                                                               this.notPromoUnion, this.promoUnion,
                                                               this.promoUnionWithTA
                                                           }
                                               };
        }

        [TestMethod]
        public void ShouldNotPromoAction()
        {
            var predicate = new filter { Item = this.notPromoEquation };

            var rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            var result = rule.IsPromoAction(false);
            Assert.AreEqual(false, result);

            predicate = new filter { Item = this.notPromoUnionOfUnion };
            rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            result = rule.IsPromoAction(false);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldPromoAction()
        {
            var predicate = new filter { Item = this.promoEquation };

            var rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            var result = rule.IsPromoAction(false);
            Assert.AreEqual(true, result);

            predicate = new filter { Item = this.promoEquationWithTA };
            rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            result = rule.IsPromoAction(false);
            Assert.AreEqual(true, result);

            predicate = new filter { Item = this.promoUnionOfUnion };
            rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            result = rule.IsPromoAction(false);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldPromoActionByCondFactors()
        {
            var predicateNotPromo = new filter { Item = this.notPromoEquation };
            var predicatePromo = new filter { Item = this.promoEquation };

            var rule = new Rule();
            rule.SetDeserializedPredicate(predicateNotPromo);
            var condFactoNotPromo = new ConditionalFactor { Predicate = predicateNotPromo };
            var condFactorPromo = new ConditionalFactor { Predicate = predicatePromo };
            rule.SetConditionalFactors(new[] { condFactoNotPromo, condFactorPromo });

            var result = rule.IsPromoAction(false);
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void ShouldNotPromoActionLinkedToTargetAudience()
        {
            var predicate = new filter { Item = this.promoEquation };
            var rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            var result = rule.IsPromoAction(true);
            Assert.AreEqual(false, result);

            predicate = new filter { Item = this.promoUnionOfUnion };
            rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            result = rule.IsPromoAction(true);
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void ShouldPromoActionLinkedToTargetAudience()
        {
            var predicate = new filter { Item = this.promoEquationWithTA };

            var rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            var result = rule.IsPromoAction(true);
            Assert.AreEqual(true, result);

            predicate = new filter { Item = this.promoUnionOfUnionWithTA };
            rule = new Rule();
            rule.SetDeserializedPredicate(predicate);
            result = rule.IsPromoAction(true);
            Assert.AreEqual(true, result);
        }
    }
}
