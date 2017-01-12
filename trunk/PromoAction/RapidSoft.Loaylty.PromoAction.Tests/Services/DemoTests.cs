namespace RapidSoft.Loaylty.PromoAction.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using RapidSoft.Loaylty.Logging;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Api.OutputResults;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Service;

    using AttributeTypes = RapidSoft.Loaylty.PromoAction.Api.Entities.AttributeTypes;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    [TestClass]
    public class DemoTests
    {
        private readonly ILog log = LogManager.GetLogger(typeof(DemoTests));

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Для тестов можно отключить.")]
        [TestMethod]
        public void ShouldTransform()
        {
            var ruleDomainName = CreateDemoRuleDomain();

            var price = 2000;

            var service = new MechanicsService();

            // NOTE: Товар доступный, НЕ детский, производитель не известен, цена больше 1500.
            var context1 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "true" }, { "Prodicts.IsKids", "false" }, 
                                   { "Prodicts.Vendor", null }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult1 = service.CalculateSingleValue(ruleDomainName, price, context1);

            Assert.AreEqual(calculateResult1.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(calculateResult1.PromoResult, (price * 0.95m) + (-10));
            TestHelper2.WriteTrace(calculateResult1.TraceMessages);

            // NOTE: Товар доступный, НЕ детский, производитель не известен, цена не больше 1500.
            price = 1000;
            
            var context2 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "true" }, { "Prodicts.IsKids", "false" }, 
                                   { "Prodicts.Vendor", null }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult2 = service.CalculateSingleValue(ruleDomainName, price, context2);

            Assert.AreEqual(calculateResult2.RuleApplyStatus, RuleApplyStatuses.BaseOnlyRulesExecuted);
            Assert.AreEqual(calculateResult2.PromoResult, price * 0.95m);
            TestHelper2.WriteTrace(calculateResult2.TraceMessages);

            // NOTE: Товар доступный, НЕ детский, производитель известен, но не "1С", цена не больше 1500.
            price = 1000;

            var context3 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "true" }, { "Prodicts.IsKids", "false" }, 
                                   { "Prodicts.Vendor", "Home Inc." }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult3 = service.CalculateSingleValue(ruleDomainName, price, context3);

            Assert.AreEqual(calculateResult3.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(calculateResult3.PromoResult, (price * 0.95m) + 50);
            TestHelper2.WriteTrace(calculateResult3.TraceMessages);

            // NOTE: Товар доступный, НЕ детский, производитель известен и "1С", цена не больше 1500.
            price = 1000;

            var context4 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "true" }, { "Prodicts.IsKids", "false" }, 
                                   { "Prodicts.Vendor", "1С / 1C: Maddox Games" }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult4 = service.CalculateSingleValue(ruleDomainName, price, context4);

            Assert.AreEqual(calculateResult4.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(calculateResult4.PromoResult, (price * 0.95m) + (50 + (-150)));
            TestHelper2.WriteTrace(calculateResult4.TraceMessages);

            // NOTE: Товар НЕ доступный, НЕ детский, производитель известен, но не "1С", цена не больше 1500.
            var context5 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "false" }, { "Prodicts.IsKids", "false" },
                                   { "Prodicts.Vendor", "Home Inc." }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult5 = service.CalculateSingleValue(ruleDomainName, price, context5);

            Assert.AreEqual(calculateResult5.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(calculateResult5.PromoResult, (price + 200) + 50);
            TestHelper2.WriteTrace(calculateResult5.TraceMessages);

            // NOTE: Товар НЕ доступный, детский, производитель известен, но не "1С", цена не больше 1500.
            var context6 = new Dictionary<string, string>
                               {
                                   { "Prodicts.Available", "false" }, { "Prodicts.IsKids", "true" },
                                   { "Prodicts.Vendor", "Home Inc." }, { "Prodicts.PriceRUR", price.ToString(CultureInfo.InvariantCulture) }
                               };

            var calculateResult6 = service.CalculateSingleValue(ruleDomainName, price, context6);

            Assert.AreEqual(calculateResult6.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(calculateResult6.PromoResult, (price + 100) + 50);
            TestHelper2.WriteTrace(calculateResult6.TraceMessages);

            // NOTE: Трансформация
            var generateResult = service.GenerateSqlWithoutLimit(ruleDomainName, null, null);

            Assert.AreEqual(generateResult.RuleApplyStatus, RuleApplyStatuses.RulesExecuted);
            Assert.AreEqual(generateResult.BaseMultiplicationSql, "CASE WHEN Prodicts.Available=1 THEN 0.95 ELSE 1 END");
            Assert.AreEqual(generateResult.BaseAdditionSql, "CASE WHEN Prodicts.Available!=1 THEN CASE WHEN Prodicts.IsKids=1 THEN 100 ELSE 200 END ELSE 0 END");
            Assert.AreEqual(generateResult.MultiplicationSql, "1");
            Assert.AreEqual(
                generateResult.AdditionSql,
                    "CASE " +
                        "WHEN Prodicts.Vendor IS NOT NULL " +
                            "THEN (CASE WHEN Prodicts.Vendor IS NOT NULL THEN 50 ELSE 0 END)+(CASE WHEN Prodicts.Vendor LIKE '%1C%' THEN -150 ELSE 0 END) " +
                            "ELSE (CASE WHEN Prodicts.Vendor LIKE '%1C%' THEN -150 ELSE 0 END)+(CASE WHEN Prodicts.PriceRUR>1500 THEN -10 ELSE 0 END) " +
                        "END");
/*
Доступные [Available] товары надо распродать, поэтому делаем скидку 5% => Базовый Мульп. коэффициент 0.95.
C 01.01.2013 доступные [Available] товары тоже будем распродавать, поэтому делаем скидку 1% => Базовый Мульп. коэффициент 0.99.
Не доступные из-за доставки делаем дорожен на 200, но детские [IsKids] всего на 100 => Базовый Адд. коэффициент +200, условный коэффициент +100
Если производитель [Vendor] известен, делаем надбавку на 50 => Исключающие НЕ Базовый Адд. коэффициент +50
Но если производитель [Vendor] 1C, делаем скидку с известного производитель 150 => Не исключаемое НЕ Базовый Адд. коэффициент -150 (итоговая скидка -100)
Для остальных (производитель [Vendor] НЕ известен) имеющих цену [PriceRUR] больше 1500, делаем скидку 10 => Исключаемое НЕ Базовый Адд. коэффициент -10 (итоговая скидка +40)
*/

            var list = new List<long>();
            for (var i = 0; i < 10; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                for (var j = 0; j < 5; j++)
                {
                    service.GenerateSqlWithoutLimit(ruleDomainName, null, null);
                }

                stopwatch.Stop();
                list.Add(stopwatch.ElapsedMilliseconds);
            }

            log.Debug("Время: " + string.Join(", ", list) + " мс.");

            DeleteDemoRuleDomain();
        }

        private static string CreateDemoRuleDomain()
        {
            DeleteDemoRuleDomain();

            var ruleDomainRepository = new RuleDomainRepository();

            var prodictsAttrs = new[]
                                    {
                                        new Api.Entities.Attribute
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Name = "Available",
                                                DisplayName = "Available",
                                                Type = AttributeTypes.Boolean
                                            },
                                        new Api.Entities.Attribute
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Name = "IsKids",
                                                DisplayName = "IsKids",
                                                Type = AttributeTypes.Boolean
                                            },
                                        new Api.Entities.Attribute
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Name = "Vendor",
                                                DisplayName = "Vendor",
                                                Type = AttributeTypes.Text
                                            },
                                        new Api.Entities.Attribute
                                            {
                                                Id = Guid.NewGuid().ToString(),
                                                Name = "PriceRUR",
                                                DisplayName = "PriceRUR",
                                                Type = AttributeTypes.Number
                                            }
                                    };

            var metadatas = new[]
                                {
                                    new EntityMetadata
                                        {
                                            EntityName = "Prodicts",
                                            DisplayName = "Prodicts",
                                            Attributes = prodictsAttrs
                                        }
                                };

            var entitiesMetadata = new EntitiesMetadata { Entities = metadatas };

            var ruleDomain = new RuleDomain
                                 {
                                     Name = "Новогодняя распродажа",
                                     Description =
                                         @"Доступные [Available] товары надо распродать, поэтому делаем скидку 5% => Базовый Мульп. коэффициент 0.95.
C первого числа след. месяца доступные [Available] товары тоже будем распродавать, поэтому делаем скидку 1% => Базовый Мульп. коэффициент 0.99.
Не доступные из-за доставки делаем дорожен на 200, но детские [IsKids] всего на 100 => Базовый Адд. коэффициент +200, условный коэффициент +100
Если производитель [Vendor] известен, делаем надбавку на 50 => Исключающие НЕ Базовый Адд. коэффициент +50
Но если производитель [Vendor] 1C, делаем скидку с известного производитель 150 => Не исключаемое НЕ Базовый Адд. коэффициент -150 (итоговая скидка -100)
Для остальных (производитель [Vendor] НЕ известен) имеющих цену [PriceRUR] больше 1500, делаем скидку 10 => Исключаемое НЕ Базовый Адд. коэффициент -10 (итоговая скидка +40)",
                                     UpdatedUserId = "Fedorov SY"
                                 };

            ruleDomain.SetDeserializedMetadata(entitiesMetadata);

            var now = DateTime.Now;

            var date = new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month)).AddDays(1);

            ruleDomainRepository.Save(ruleDomain);

            // NOTE: Доступные [Available] товары надо распродать, поэтому делаем скидку 5% => Базовый Мульп. коэффициент 0.95.
            var rule1 = new Rule
                            {
                                Name = "Доступные [Available] товары надо распродать, поэтому делаем скидку 5% => Базовый Мульп. коэффициент 0.95.",
                                Factor = 0.95m,
                                Type = RuleTypes.BaseMultiplication,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = date,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY"
                            };
            rule1.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.eq,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "Available",
                                                                            type = valueType.boolean
                                                                        }
                                                                }
                                                    },
                                                new value { type = valueType.boolean, Text = new[] { "true" } }
                                            }
                                }
                    });

            // NOTE: C 01.01.2013 доступные [Available] товары тоже будем распродавать, поэтому делаем скидку 1% => Базовый Мульп. коэффициент 0.99.
            var rule2 = new Rule
                            {
                                Name = "C 01.01.2013 доступные [Available] товары тоже будем распродавать, поэтому делаем скидку 1% => Базовый Мульп. коэффициент 0.99.",
                                Factor = 0.99m,
                                Type = RuleTypes.BaseMultiplication,
                                ConditionalFactors = null,
                                DateTimeFrom = date.AddSeconds(-1),
                                DateTimeTo = null,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY",
                                Priority = 100,
                            };

            rule2.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.eq,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "Available",
                                                                            type = valueType.boolean
                                                                        }
                                                                }
                                                    },
                                                new value { type = valueType.boolean, Text = new[] { "true" } }
                                            }
                                }
                    });

            // NOTE: Не доступные из-за доставки делаем дорожен на 200, но детские [IsKids] всего на 100 => Базовый Адд. коэффициент 200, условный коэффициент 100.
            var rule3 = new Rule
                            {
                                Name =
                                    "Не доступные из-за доставки делаем дорожен на 200, но детские [IsKids] всего на 100 => Базовый Адд. коэффициент 200, условный коэффициент 100.",
                                Factor = 200m,
                                Type = RuleTypes.BaseAddition,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = null,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY"
                            };
            rule3.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.noteq,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "Available",
                                                                            type = valueType.boolean
                                                                        }
                                                                }
                                                    },
                                                new value { type = valueType.boolean, Text = new[] { "true" } }
                                            }
                                }
                    });
            rule3.SetConditionalFactors(
                new[]
                    {
                        new ConditionalFactor
                            {
                                Factor = 100m,
                                Predicate =
                                    new filter
                                        {
                                            Item =
                                                new equation
                                                    {
                                                        @operator = equationOperator.eq,
                                                        value =
                                                            new[]
                                                                {
                                                                    new value
                                                                        {
                                                                            type = valueType.attr,
                                                                            attr =
                                                                                new[]
                                                                                    {
                                                                                        new valueAttr
                                                                                            {
                                                                                                @object = "Prodicts",
                                                                                                name = "IsKids",
                                                                                                type = valueType.boolean
                                                                                            }
                                                                                    }
                                                                        },
                                                                    new value
                                                                        {
                                                                            type = valueType.boolean,
                                                                            Text = new[] { "true" }
                                                                        }
                                                                }
                                                    }
                                        }
                            }
                    });
            
            // NOTE: Если производитель [Vendor] известен, делаем надбавку на 50 => Исключающие НЕ Базовый Адд. коэффициент +50.
            var rule4 = new Rule
                            {
                                Name = "Если производитель [Vendor] известен, делаем надбавку на 50 => Исключающие НЕ Базовый Адд. коэффициент +50.",
                                Factor = 50m,
                                Type = RuleTypes.Addition,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = null,
                                IsExclusive = true,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY"
                            };
            rule4.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.nem,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "Vendor",
                                                                            type = valueType.@string
                                                                        }
                                                                }
                                                    }
                                            }
                                }
                    });

            // NOTE: Но если производитель [Vendor] 1C, делаем скидку с известного производитель 150 => Не исключаемое НЕ Базовый Адд. коэффициент -150 (итоговая скидка -100)
            var rule5 = new Rule
                            {
                                Name = "Но если производитель [Vendor] 1C, делаем скидку с известного производитель 150 => Не исключаемое НЕ Базовый Адд. коэффициент -150 (итоговая скидка -100)",
                                Factor = -150,
                                Type = RuleTypes.Addition,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = null,
                                IsExclusive = false,
                                IsNotExcludedBy = true,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY"
                            };
            rule5.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.like,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "Vendor",
                                                                            type = valueType.@string
                                                                        }
                                                                }
                                                    },
                                                new value { type = valueType.@string, Text = new[] { "1C" } }
                                            }
                                }
                    });

            // NOTE: Для остальных имеющих цену [PriceRUR] больше 1500, делаем скидку 10 => Исключаемое НЕ Базовый Адд. коэффициент -10 (итоговая скидка +40)
            var rule6 = new Rule
                            {
                                Name =
                                    "Для остальных имеющих цену [PriceRUR] больше 1500, делаем скидку 10 => Исключаемое НЕ Базовый Адд. коэффициент -10 (итоговая скидка +40)",
                                Factor = -10,
                                Type = RuleTypes.Addition,
                                ConditionalFactors = null,
                                DateTimeFrom = null,
                                DateTimeTo = null,
                                IsExclusive = false,
                                IsNotExcludedBy = false,
                                RuleDomain = ruleDomain,
                                UpdatedUserId = "Fedorov SY"
                            };
            rule6.SetDeserializedPredicate(
                new filter
                    {
                        Item =
                            new equation
                                {
                                    @operator = equationOperator.gt,
                                    value =
                                        new[]
                                            {
                                                new value
                                                    {
                                                        type = valueType.attr,
                                                        attr =
                                                            new[]
                                                                {
                                                                    new valueAttr
                                                                        {
                                                                            @object = "Prodicts",
                                                                            name = "PriceRUR",
                                                                            type = valueType.numeric
                                                                        }
                                                                }
                                                    },
                                                new value { type = valueType.numeric, Text = new[] { "1500" } }
                                            }
                                }
                    });

            var ruleRepository = new RuleRepository();

            ruleRepository.Save(rule1);
            ruleRepository.SaveApprove(rule1.Id, true, "Test");
            ruleRepository.Save(rule2);
            ruleRepository.SaveApprove(rule2.Id, true, "Test");
            ruleRepository.Save(rule3);
            ruleRepository.SaveApprove(rule3.Id, true, "Test");
            ruleRepository.Save(rule4);
            ruleRepository.SaveApprove(rule4.Id, true, "Test");
            ruleRepository.Save(rule5);
            ruleRepository.SaveApprove(rule5.Id, true, "Test");
            ruleRepository.Save(rule6);
            ruleRepository.SaveApprove(rule6.Id, true, "Test");
            
            return ruleDomain.Name;
        }

        private static void DeleteDemoRuleDomain()
        {
            var ruleDomainRepository = new RuleDomainRepository();

            var exists = ruleDomainRepository.GetByName("Новогодняя распродажа");

            if (exists != null)
            {
                ruleDomainRepository.DeleteById(exists.Id, "Fedorov SY");
            }
        }
    }
}
