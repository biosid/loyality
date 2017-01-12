namespace RapidSoft.Loaylty.PromoAction.Tracer
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using RapidSoft.Extensions;
    using RapidSoft.Loaylty.PromoAction.Api.Entities;
    using RapidSoft.Loaylty.PromoAction.Mechanics.Aggregates;
    using RapidSoft.Loaylty.PromoAction.Mechanics.PredicateEval;
    using RapidSoft.Loaylty.PromoAction.Mechanics.RuleCalculator;

    public class Tracer : ITracer
    {
        private object obj = new object();

        private readonly List<TracerItem> tracerItems = new List<TracerItem>();

        public void Trace(RuleTypes ruleType, string comment = null)
        {
            var innerComment = ruleType + ": " + comment;
            var item = new TracerItem(ruleType) { Data = null, Comment = innerComment };
            this.Add(item);
        }

        public void Trace(RuleTypes ruleType, AggregateRuleResult ruleResult, string comment)
        {
            var resultCode = this.FriendlyEvalResult(ruleResult.Code);
            var addComment = comment != null ? " " + comment : string.Empty;

            var innerComment =
                string.Format(
                    "{3}: Для группы правил \"{0}\" результат вычисления \"{1}\".{2}",
                    ruleType,
                    resultCode,
                    addComment,
                    ruleType);

            var item = new TracerItem(ruleType) { Data = new object[] { ruleResult }, Comment = innerComment };
            this.Add(item);
        }

        public string[] GetMessages()
        {
            lock (obj)
            {
                Console.WriteLine(this.tracerItems.Any(x => x == null));
                var grouped = this.tracerItems.GroupBy(x => x.RuleType).OrderBy(x => x.Key);
                var retVal = grouped.SelectMany(GetMessages).ToArray();
                return retVal;
            }
        }

        private IEnumerable<string> GetMessages(IEnumerable<TracerItem> items)
        {
            return items.OrderBy(y => y.Ticks).Select(y => y.Comment);
        }

        public void Trace(Rule rule, RuleResult ruleResult, string comment = null)
        {
            var resultCode = this.FriendlyEvalResult(ruleResult.Code);
            var resultFactor = this.FormatRuleFinalFactor(ruleResult);
            var addComment = comment != null ? " " + comment : string.Empty;
            var innerComment =
                string.Format(
                    "{6}: Для правила \"{0} ({1})\" с коэфф. {2} результат вычисления \"{3} ({4})\".{5}",
                    rule.Name ?? "null",
                    rule.Id,
                    rule.Factor,
                    resultCode,
                    resultFactor,
                    addComment,
                    rule.Type);
            var item = new TracerItem(rule.Type) { Data = new object[] { rule, ruleResult }, Comment = innerComment };
            this.Add(item);
        }

        public void Trace(Rule rule, EvaluateResult evalResult, string comment = null)
        {
            var resultCode = this.FriendlyEvalResult(evalResult.Code);
            var addComment = comment != null ? " " + comment : string.Empty;
            var innerComment = string.Format(
                "{5}: Для правила \"{0} ({1})\" с коэфф. {2} предикат равен \"{3}\".{4}",
                rule.Name ?? "null",
                rule.Id,
                rule.Factor,
                resultCode,
                addComment,
                rule.Type);
            var item = new TracerItem(rule.Type) { Data = new object[] { rule, evalResult }, Comment = innerComment };
           this.Add(item);
        }

        private void Add(TracerItem item)
        {
            item.ThrowIfNull("item");
            lock (obj)
            {
                this.tracerItems.Add(item);
            }
        }

        private string FormatRuleFinalFactor(RuleResult ruleResult)
        {
            if (ruleResult.Code == EvaluateResultCode.True && ruleResult.IsFactorAvaible)
            {
                return "итоговый коэф. с учетом условных коэфф. = "
                       + ruleResult.Factor.ToString(CultureInfo.InvariantCulture);
            }

            return "итоговый коэф. неисчислимый";
        }

        private string FriendlyEvalResult(EvaluateResultCode evaluateResultCode)
        {
            switch (evaluateResultCode)
            {
                case EvaluateResultCode.False:
                    {
                        return false.ToString();
                    }

                case EvaluateResultCode.True:
                    {
                        return true.ToString();
                    }

                case EvaluateResultCode.ConvertibleToSQL:
                    {
                        return "сonvertible";
                    }

                default:
                    {
                        var mess = string.Format("Тип результата предиката {0} не поддерживается", evaluateResultCode);
                        throw new NotSupportedException(mess);
                    }
            }
        }
    }

    internal class TracerItem
    {
        public TracerItem(RuleTypes ruleType)
        {
            this.RuleType = ruleType;
            this.Ticks = DateTime.Now.Ticks;
        }

        public long Ticks { get; private set; }

        public RuleTypes RuleType { get; private set; }

        public object[] Data { get; set; }

        public string Comment { get; set; }
    }
}