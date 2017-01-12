namespace RapidSoft.Loaylty.PromoAction.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Security.Principal;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Repositories;
    using RapidSoft.Loaylty.PromoAction.Settings;
    using RapidSoft.VTB24.ArmSecurity.Entities;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    using Rule = RapidSoft.Loaylty.PromoAction.Api.Entities.Rule;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Для тестов можно отключить.")]
    internal static class TestHelper
    {
        private static int priorityValue;

        public static int GetNextPriority()
        {
            return priorityValue++;
        }

        public static void DeleteTestRuleDomain(IEnumerable<long> ids)
        {
            if (ids == null)
            {
                return;
            }

            foreach (var sql in ids.Select(id => string.Format("DELETE FROM [promo].[RuleDomains] WHERE [Id] = {0}", id)))
            {
                MechanicsDbContext.Get().Database.ExecuteSqlCommand(sql);
            }
        }

        public static RuleDomain BuildTestRuleDomain(string name = null)
        {
            return new RuleDomain
                       {
                           Name = name ?? "RuleDomain " + Guid.NewGuid(),
                           Description = "Description RuleDomain",
                           LimitType = LimitTypes.Percent,
                           LimitFactor = 50,
                           UpdatedUserId = "I",
                           InsertedDate = DateTime.Now
                       };
        }

        public static Rule BuildTestRuleWithPredicateFromFile(string predicateFileName, RuleTypes type = RuleTypes.BaseMultiplication, int? priority = null, decimal coef = 1)
        {
            var innerPriority = priority ?? GetNextPriority();
            var rule = BuildTestRule(type: type, priority: innerPriority, coef: coef);
            rule.Predicate = ReadFile(predicateFileName);
            return rule;
        }

        public static Rule BuildTestRule(string name = null, RuleTypes type = RuleTypes.BaseMultiplication, int? priority = null, decimal coef = 1, long ruleDomainId = 0)
        {
            var innerName = name ?? "Test";
            var innerPriority = priority ?? GetNextPriority();

            return new Rule
                       {
                           Name = innerName,
                           Factor = coef,
                           Type = type,
                           InsertedDate = DateTime.Now,
                           UpdatedUserId = "I",
                           Predicate = "Какой-то xml",
                           Priority = innerPriority,
                           RuleDomainId = ruleDomainId,
                           DateTimeFrom = null,
                           DateTimeTo = null
                       };
        }

        public static RuleDomain InsertTestRuleDomain(string name = null)
        {
            var repo = new RuleDomainRepository();
            var ruleDomain = BuildTestRuleDomain(name);
            repo.Save(ruleDomain);

            return ruleDomain;
        }

        public static Rule InsertTestRule(
            RuleDomain ruleDomain,
            string predicateFileName = "EquationEqNumeric.xml",
            RuleTypes type = RuleTypes.BaseMultiplication,
            int? priority = null, 
            decimal coef = 1,
            ConditionalFactor[] conditionalFactors = null)
        {
            var repo = new RuleRepository();

            var innerPriority = priority ?? GetNextPriority();
            var rule = BuildTestRuleWithPredicateFromFile(predicateFileName, type, innerPriority, coef);

            rule.SetConditionalFactors(conditionalFactors);

            rule.RuleDomainId = ruleDomain.Id;

            repo.Save(rule);

            return rule;
        }

        public static RuleDomain InsertTestRuleDomainWithoutBaseRule(string name = null)
        {
            var repo = new RuleDomainRepository();
            var ruleDomain = BuildTestRuleDomain(name);

            ruleDomain.Rules = new List<Rule>
                                {
                                    BuildTestRule(type: RuleTypes.Multiplication)
                                };
            repo.Save(ruleDomain);

            return ruleDomain;
        }

        public static string ReadFile(string fileName)
        {
            using (var sr = new StreamReader("FilterXMLs/" + fileName))
            {
                return sr.ReadToEnd();
            }
        }

        public static filter ReadPredicate(string fileName)
        {
            using (var sr = new StreamReader("FilterXMLs/" + fileName))
            {
                var s = sr.ReadToEnd();

                return s.Deserialize<filter>();
            }
        }

        public static string GetTruePredicate()
        {
            var f = new filter
                           {
                               Item = TestHelper2.BuildTrueEquation()
                           };
            return f.Serialize();
        }

        public static TargetAudience InsertTestTargetAudience(string name = null, string userId = "I")
        {
            var id = "Id " + Guid.NewGuid();
            var targetAudienceName = name ?? "Name " + Guid.NewGuid();
            var repo = new TargetAudienceRepository();

            var targetAudience = new TargetAudience { Id = id, Name = targetAudienceName, CreateUserId = userId };

            repo.Save(targetAudience);

            return targetAudience;
        }

        public static TargetAudienceClientLink InsertTargetAudienceClientLink(string targetAudienceId = null, string clientId = null, string userId = "I")
        {
            var taid = targetAudienceId ?? InsertTestTargetAudience().Id;
            var cid = clientId ?? 555.ToString(CultureInfo.InvariantCulture);

            var repo = new TargetAudienceClientLinkRepository();

            var link = new TargetAudienceClientLink { TargetAudienceId = taid, ClientId = cid, CreateUserId = userId };

            repo.Insert(link);

            return link;
        }

		public static void DeleteTestTargetAudienceClientLinks(string targetAudienceId = null, string clientId = null)
		{
			var repo = new TargetAudienceClientLinkRepository();
			var selected = (IQueryable<TargetAudienceClientLink>)repo.GetAll();

			if (!string.IsNullOrEmpty(targetAudienceId))
			{
				selected = selected.Where(x => x.TargetAudienceId == targetAudienceId);
			}

			if (!string.IsNullOrEmpty(clientId))
			{
				selected = selected.Where(x => x.ClientId == clientId);
			}

			foreach (var targetAudienceClientLink in selected)
			{
				repo.DeleteById(targetAudienceClientLink.TargetAudienceId, targetAudienceClientLink.ClientId, null);
			}
		}

        public static filter BuildPromoActionPredicate()
        {
            var literal = new value { type = valueType.@string, Text = new[] { "Какое-то значение" } };
            var promoVariableAttr = new valueAttr
                                        {
                                            @object = ApiSettings.ClientProfileObjectName,
                                            name = ApiSettings.PromoActionPropertyName,
                                            type = valueType.@string
                                        };

            var promoVariable = new value { type = valueType.attr, attr = new[] { promoVariableAttr } };
            var promoEquation = new equation { value = new[] { promoVariable, literal } };

            return new filter { Item = promoEquation };
        }

        public static filter BuildPromoActionLinkedToTargetAudiencePredicate()
        {
            var literalTA = new value
                                {
                                    type = valueType.@string,
                                    Text = new[] { ApiSettings.TargetAudienceLiteralPrefix + "ID" }
                                };
            var promoVariableAttr = new valueAttr
                                        {
                                            @object = ApiSettings.ClientProfileObjectName,
                                            name = ApiSettings.PromoActionPropertyName,
                                            type = valueType.@string
                                        };

            var promoVariable = new value { type = valueType.attr, attr = new[] { promoVariableAttr } };
            var promoEquationWithTA = new equation { value = new[] { promoVariable, literalTA } };
            return new filter { Item = promoEquationWithTA };
        }

        public static filter BuildRulePredicate()
        {
            return new filter { Item = TestHelper2.BuildTrueEquation() };
        }

        public static IVtb24Principal BuildSuperUser()
        {
            return new SuperPrincipal { Identity = new SuperIdentity() };
        }

        public class SuperIdentity : IIdentity
        {
            public string Name
            {
                get
                {
                    return "User";
                }
            }

            public string AuthenticationType
            {
                get
                {
                    return "AuthenticationType";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return true;
                }
            }
        }

        public class SuperPrincipal : IVtb24Principal
        {
            public IIdentity Identity { get; set; }

            public bool HasPermission(string permission)
            {
                return true;
            }

            public bool HasPermissions(IEnumerable<string> permissions)
            {
                return true;
            }

            public bool HasPermissionForPartner(string permission, string partnerId)
            {
                return true;
            }

            public bool HasPermissionsForPartner(IEnumerable<string> permissions, string partnerId)
            {
                return true;
            }
        }
    }
}
