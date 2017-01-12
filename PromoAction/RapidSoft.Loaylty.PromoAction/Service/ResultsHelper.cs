namespace RapidSoft.Loaylty.PromoAction.Service
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.GroupCalculators;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Tracer;

    /// <summary>
    /// Класс содержит вспомогательные методы-расширители для работы с коллекцией результатов вычисления групп правил.
    /// </summary>
    internal static class ResultsHelper
    {
        /// <summary>
        /// Возвращает коэффициент для группы правил с типом <paramref name="ruleType"/>.
        /// </summary>
        /// <param name="results">
        /// Коллекция результатов вычисления групп правил.
        /// </param>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        /// <returns>
        /// Искомый коэффициент.
        /// </returns>
        public static decimal GetRulesFactor(this IEnumerable<RuleGroupResult> results, RuleTypes ruleType)
        {
            var result = results.FirstOrDefault(x => x.Code == EvaluateResultCode.True && x.RuleType == ruleType);
            return result == null ? ruleType.GetDefaultFactor() : result.Factor;
        }

        /// <summary>
        /// Возвращает коэффициент для группы правил с типом <paramref name="ruleType"/> с указанием найдено ли правило с предикатом = true или нет.
        /// </summary>
        /// <param name="results">
        /// Коллекция результатов вычисления групп правил.
        /// </param>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        /// <param name="isFound">
        /// Признак указывающий найдено ли в группе правил с типом <paramref name="ruleType"/> правило с предикатом = true или нет.
        /// </param>
        /// <returns>
        /// Искомый коэффициент.
        /// </returns>
        public static decimal GetRulesFactor(this IEnumerable<RuleGroupResult> results, RuleTypes ruleType, ITracer tracer, out bool isFound)
        {
            var result = results.FirstOrDefault(x => x.Code == EvaluateResultCode.True && x.RuleType == ruleType);
            if (result == null || result.Code == EvaluateResultCode.False)
            {
                tracer.Trace(ruleType, "Правила не найдены");
                isFound = false;
                return ruleType.GetDefaultFactor();
            }

            var factor = result.Factor;
            isFound = true;

            tracer.Trace(ruleType, "Итоговый коэффициент: " + factor);
            return factor;
        }

        /// <summary>
        /// Возвращает SQL для группы правил с типом <paramref name="ruleType"/>.
        /// </summary>
        /// <param name="results">
        /// Коллекция результатов вычисления групп правил.
        /// </param>
        /// <param name="ruleType">
        /// Тип правил.
        /// </param>
        /// <returns>
        /// SQL для группы правил с типом <paramref name="ruleType"/>.
        /// </returns>
        public static string ConvertToSql(this IEnumerable<RuleGroupResult> results, RuleTypes ruleType)
        {
            using (var writer = new StringWriter())
            {
                var result = results.FirstOrDefault(x => x.RuleType == ruleType);
                if (result == null || result.Code == EvaluateResultCode.False)
                {
                    writer.Write(ruleType.GetDefaultFactor().ToString(CultureInfo.InvariantCulture));
                }
                else
                {
                    result.Converter.Convert(writer);
                }

                return writer.ToString();
            }
        }
    }
}