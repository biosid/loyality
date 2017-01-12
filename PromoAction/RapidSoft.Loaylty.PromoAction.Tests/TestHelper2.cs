namespace RapidSoft.Loaylty.PromoAction.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Mechanics;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Converters;
    using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval.Values;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;
    using RapidSoft.Loaylty.PromoAction.Tests.Mocks;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    public static class TestHelper2
    {
        public static readonly MockTracer MockTracer = new MockTracer();
        public static readonly string Numeric = "5";
        public static readonly string OtherNumeric = "6";
        public static readonly string String = "абв";
        public static readonly string OtherString = "где";
        public static readonly string True = true.ToString();
        public static readonly string False = false.ToString();
        public static readonly string DateTimeNow = DateTime.Now.ToString(CultureInfo.InvariantCulture);
        public static readonly string DateTimeNowPlus5Days = DateTime.Now.AddDays(5).ToString(CultureInfo.InvariantCulture);

        public static readonly IDictionary<string, string> EmptyDic = new Dictionary<string, string>();

        public static readonly IDictionary<string, string> Aliases = new Dictionary<string, string>
                                                                         {
                                                                             {
                                                                                 "Object.Name",
                                                                                 "table.column"
                                                                             },
                                                                             {
                                                                                 "AnotherObject.Name",
                                                                                 "anotherTable.column"
                                                                             }
                                                                         };

        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Для тестов можно отключить.")]
        public static EvaluationSettings SettingsContext = new EvaluationSettings(MockTracer, EmptyDic);
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Для тестов можно отключить.")]
        public static EvaluationSettings SettingsAliases = new EvaluationSettings(MockTracer, null, Aliases);
        [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Для тестов можно отключить.")]
        public static EvaluationSettings Settings = new EvaluationSettings(MockTracer, EmptyDic, Aliases);

        public static VariableValue[] BuildTwoValue(string val1, string val2, valueType type1, valueType? type2 = null, IDictionary<string, string> context=null)
        {
            var realType2 = !type2.HasValue ? type1 : type2.Value;

            var resolver = new VariableResolver(context, null);

            return
                new[] { new value { type = type1, Text = new[] { val1 } }, new value { type = realType2, Text = new[] { val2 } } }
                    .Select(resolver.ResolveVariable).ToArray();
        }

        public static value[] BuildTwoValue2(string val1, string val2, valueType type1, valueType? type2 = null)
        {
            var realType2 = !type2.HasValue ? type1 : type2.Value;

            return
                new[] { new value { type = type1, Text = new[] { val1 } }, new value { type = realType2, Text = new[] { val2 } } };
        }

        public static VariableValue[] BuildTwoValueWithAttr(string val2, valueType type1, valueType? type2 = null, IDictionary<string, string> aliases = null)
        {
            var realType2 = !type2.HasValue ? type1 : type2.Value;

            var resolver = new VariableResolver(null, aliases);

            return
                new[]
                    {
                        new value
                            {
                                type = valueType.attr,
                                attr = new[] { new valueAttr { @object = "Object", name = "Name", type = type1 } }
                            },
                        new value { type = realType2, Text = new[] { val2 } }
                    }.Select(resolver.ResolveVariable).ToArray();
        }

        public static value[] BuildTwoValueWithAttr2(string val2, valueType type1, valueType? type2 = null)
        {
            var realType2 = !type2.HasValue ? type1 : type2.Value;

            return
                new[]
                    {
                        new value
                            {
                                type = valueType.attr,
                                attr = new[] { new valueAttr { @object = "Object", name = "Name", type = type1 } }
                            },
                        new value { type = realType2, Text = new[] { val2 } }
                    };
        }

        public static VariableValue[] BuildOneValue(string val1, valueType type1, IDictionary<string, string> context = null)
        {
            var resolver = new VariableResolver(context, null);
            return new[] { new value { type = type1, Text = new[] { val1 } } }.Select(resolver.ResolveVariable).ToArray();
        }

        public static VariableValue[] BuildOneValueWithAttr(string val1, valueType type1, IDictionary<string, string> aliases = null)
        {
            var resolver = new VariableResolver(null, aliases);

            return new[]
                       {
                           new value
                               {
                                   type = valueType.attr,
                                   attr = new[] { new valueAttr { @object = "Object", name = "Name", type = type1 } }
                               }
                       }.Select(resolver.ResolveVariable).ToArray();
        }

        public static value[] BuildOneValueWithAttr2(string val1, valueType type1)
        {
            return new[]
                       {
                           new value
                               {
                                   type = valueType.attr,
                                   attr = new[] { new valueAttr { @object = "Object", name = "Name", type = type1 } }
                               }
                       };
        }

        public static equation BuildTrueEquation()
        {
            return new equation { @operator = equationOperator.eq, value = BuildTwoValue2(Numeric, Numeric, valueType.numeric) };
        }

        public static equation BuildFalseEquation()
        {
            return new equation { @operator = equationOperator.eq, value = BuildTwoValue2(Numeric, OtherNumeric, valueType.numeric) };
        }

        public static union BuildUnion(string operation, equation operand1, equation operand2)
        {
            return new union { type = operation, equation = new[] { operand1, operand2 } };
        }

        public static filter BuildUnion(string operation, bool operand1, bool operand2)
        {
            return new filter
                       {
                           Item =
                               new union
                                   {
                                       type = operation,
                                       equation =
                                           new[]
                                               {
                                                   operand1 ? BuildTrueEquation() : BuildFalseEquation(),
                                                   operand2 ? BuildTrueEquation() : BuildFalseEquation()
                                               }
                                   }
                       };
        }

        public static equation BuildEquationWithAttr()
        {
            return new equation { @operator = equationOperator.eq, value = BuildTwoValueWithAttr2(OtherNumeric, valueType.numeric) };
        }

        public static filter BuildUnionWithAttr(string operation, bool operand2)
        {
            var union = new union
                            {
                                type = operation,
                                equation =
                                    new[] { BuildEquationWithAttr(), operand2 ? BuildTrueEquation() : BuildFalseEquation() }
                            };

            return new filter { Item = union };
        }

        public static filter BuildUnionWithAttr(string operation, bool operand2, union innerUnion)
        {
            var union = new union
                       {
                           type = operation,
                           equation =
                               new[] { BuildEquationWithAttr(), operand2 ? BuildTrueEquation() : BuildFalseEquation() },
                           union1 = new[] { innerUnion }
                       };

            return new filter { Item = union };
        }

        public static filter BuildUnion(string operation, bool operand1, bool operand2, object innerUnion)
        {
            var union = new union
                            {
                                type = operation,
                                equation =
                                    new[]
                                        {
                                            operand1 ? BuildTrueEquation() : BuildFalseEquation(),
                                            operand2 ? BuildTrueEquation() : BuildFalseEquation()
                                        },
                                union1 = new[] { innerUnion as union }
                            };
            return new filter { Item = union };
        }

        public static string Convert(EvaluateResult result)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                result.Convert(sw);
            }

            return sb.ToString();
        }

        public static string Convert(IPredicateSqlConverter converter)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                converter.Convert(sw);
            }

            return sb.ToString();
        }

        public static EvaluateResult EvaluatePredicate(
            this filter predicate,
            EvaluationSettings settings)
        {
            var strategy = settings.EvalStrategySelector.SelectEvalStrategy(predicate, settings);

            return strategy.EvaluateExt();
        }

        public static string Convert(RuleResult resultExt)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                resultExt.Convert(sw);
            }

            return sb.ToString();
        }

        public static string Convert(ISqlConvert resultExt)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                resultExt.Convert(sw);
            }

            return sb.ToString();
        }

        public static string Convert(AggregateRuleResult resultExt)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                resultExt.Converter.Convert(sw);
            }

            return sb.ToString();
        }

        public static string Convert(RuleGroupResult resultExt)
        {
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            {
                resultExt.Converter.Convert(sw);
            }

            return sb.ToString();
        }


        public static void WriteTrace(IEnumerable<string> messages)
        {
            if (messages == null)
            {
                return;
            }

            Console.WriteLine("--------------------------");
            foreach (var message in messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}
