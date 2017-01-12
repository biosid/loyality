namespace RapidSoft.Loaylty.PromoAction.Mechanics
{
    using System.Collections.Generic;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Api.FilterBuilder;
    using RapidSoft.Loaylty.PromoAction.Settings;

    internal static class RuleExtensions
    {
        public static bool IsPromoAction(this Rule rule, bool linkedTargetAudienceOnly)
        {
            rule.ThrowIfNull("rule");
            var predicate = rule.GetDeserializedPredicate();

            var isPredicate = predicate.IsOperatePromoAction(linkedTargetAudienceOnly);

            if (isPredicate)
            {
                return true;
            }

            // NOTE: VTBPLK-1594: Проверяем условные коэффициенты
            var condFactors = rule.GetDeserializedConditionalFactors();
            if (condFactors != null)
            {
                var condFactorPredicates = condFactors.Select(x => x.Predicate).Where(x => x != null);
                foreach (var condFactorPredicate in condFactorPredicates)
                {
                    var isCondFactorPredicate = condFactorPredicate.IsOperatePromoAction(linkedTargetAudienceOnly);

                    if (isCondFactorPredicate)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool IsOperatePromoAction(this filter predicate, bool linkedTargetAudienceOnly)
        {
            if (predicate == null)
            {
                return false;
            }

            var equation = predicate.Item as equation;

            if (equation != null)
            {
                return equation.IsOperatePromoAction(linkedTargetAudienceOnly);
            }

            var union = predicate.Item as union;

            if (union != null)
            {
                return union.AnyOperatePromoAction(linkedTargetAudienceOnly);
            }

            return false;
        }

        private static bool AnyOperatePromoAction(this union union, bool linkedTargetAudienceOnly)
        {
            if (union.equation != null && union.equation.AnyOperatePromoAction(linkedTargetAudienceOnly))
            {
                return true;
            }

            if (union.union1 != null)
            {
                return union.union1.Any(x => AnyOperatePromoAction(x, linkedTargetAudienceOnly));
            }

            return false;
        }

        private static bool AnyOperatePromoAction(this IEnumerable<equation> equations, bool linkedTargetAudienceOnly)
        {
            return equations.Any(x => IsOperatePromoAction(x, linkedTargetAudienceOnly));
        }

        private static bool IsOperatePromoAction(this equation equation, bool linkedTargetAudienceOnly)
        {
            var values = equation.value;

            // Если оперирует переменной "ClientProfile.Audiences"(ApiSettings.ClientProfileObjectName и ApiSettings.PromoActionPropertyName)
            if (values.Any(IsOperateClientProfileAudiences))
            {
                // Если надо только привязнные к ЦА
                if (linkedTargetAudienceOnly)
                {
                    // Если оперирует литерам начинающимся с "Audience_" (ApiSettings.TargetAudienceLiteralPrefix)
                    return values.Any(IsLinkedTargetAudience);
                }

                return true;
            }

            return false;
        }

        private static bool IsOperateClientProfileAudiences(this value value)
        {
            return value.type == valueType.attr
                   && value.attr.Any(y => y.name == ApiSettings.PromoActionPropertyName && y.@object == ApiSettings.ClientProfileObjectName);
        }

        private static bool IsLinkedTargetAudience(this value value)
        {
            return value.type != valueType.attr && string.Join(string.Empty, value.Text).StartsWith(ApiSettings.TargetAudienceLiteralPrefix);
        }
    }
}