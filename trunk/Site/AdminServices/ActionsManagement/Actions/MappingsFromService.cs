using System.Collections.Generic;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers;
using Vtb24.Arms.AdminServices.AdminMechanicsService;
using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions
{
    internal static class MappingsFromService
    {
        public static ActionStatus ToActionStatus(RuleStatuses original)
        {
            switch (original)
            {
                case RuleStatuses.Active:
                    return ActionStatus.Active;
                case RuleStatuses.NotActive:
                    return ActionStatus.NotActive;
            }

            return ActionStatus.Unknown;
        }

        public static ActionType ToActionType(RuleTypes original)
        {
            switch (original)
            {
                case RuleTypes.Addition:
                    return ActionType.Addition;
                case RuleTypes.Multiplication:
                    return ActionType.Multiplication;
                case RuleTypes.BaseAddition:
                    return ActionType.BaseAddition;
                case RuleTypes.BaseMultiplication:
                    return ActionType.BaseMultiplication;
            }

            return ActionType.Unknown;
        }

        public static ActionApproveStatus ToActionApproveStatus(ApproveStatus original)
        {
            switch (original)
            {
                case ApproveStatus.NotApproved:
                    return ActionApproveStatus.NotApproved;
                case ApproveStatus.Approved:
                    return ActionApproveStatus.Approved;
                case ApproveStatus.Correction:
                    return ActionApproveStatus.Correction;
            }

            return ActionApproveStatus.Unknown;
        }

        public static Action ToAction(Rule original)
        {
            Predicate predicate;

            try
            {
                predicate = PredicateHelpers.XmlToPredicate(original.Predicate);
            }
            catch (PredicateMappingException)
            {
                predicate = new Predicate();
            }

            ConditionalFactor[] conditionalFactors;

            try
            {
                conditionalFactors = PredicateHelpers.XmlToConditionalFactors(original.ConditionalFactors);
            }
            catch (PredicateMappingException)
            {
                conditionalFactors = new ConditionalFactor[0];
            }

            return new Action
            {
                Id = original.Id,
                Name = original.Name,
                From = original.DateTimeFrom,
                To = original.DateTimeTo,
                Priority = original.Priority,
                IsExclusive = original.IsExclusive,
                IsNotExcludedBy = original.IsNotExcludedBy,
                Status = ToActionStatus(original.Status),
                ApproveStatus = ToActionApproveStatus(original.Approved),
                ApproveMessage = original.ApproveDescription,
                Predicate = predicate,
                MechanicId = original.RuleDomainId,
                Type = ToActionType(original.Type),
                Factor = original.Factor,
                ConditionalFactors = conditionalFactors,
                Updated = original.UpdatedDate ?? original.InsertedDate,
                UpdatedBy = original.UpdatedUserId
            };
        }

        public static IEnumerable<HistoryRecord> ToHistoryRecords(IEnumerable<RuleHistory> original)
        {
            using (var enumerator = original.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    yield break;

                var current = enumerator.Current;

                while (enumerator.MoveNext())
                {
                    yield return ToHistoryRecord(enumerator.Current, current);
                    current = enumerator.Current;
                }

                yield return ToHistoryRecord(null, current);
            }
        }

        public static HistoryRecord ToHistoryRecord(RuleHistory previous, RuleHistory original)
        {
            return new HistoryRecord
            {
                When = original.UpdatedDate ?? original.InsertedDate,
                Who = original.UpdatedUserId,
                What = previous == null
                           ? "Создание механики"
                           : string.Join(", ", GetWhatIsChanged(original, previous))
            };
        }

        private static IEnumerable<string> GetWhatIsChanged(RuleHistory rule1, RuleHistory rule2)
        {
            if (rule1.Name != rule2.Name)
                yield return "Наименование";
            if (rule1.DateTimeFrom != rule2.DateTimeFrom)
                yield return "Период с";
            if (rule1.DateTimeTo != rule2.DateTimeTo)
                yield return "Период по";
            if (rule1.Status != rule2.Status)
                yield return "Статус";
            if (rule1.Priority != rule2.Priority)
                yield return "Приоритет";
            if (rule1.IsExclusive != rule2.IsExclusive)
                yield return "Исключающая";
            if (rule1.IsNotExcludedBy != rule2.IsNotExcludedBy)
                yield return "Неисключаемая";
            if (rule1.Predicate != rule2.Predicate)
                yield return "Условие";
            if (rule1.ConditionalFactors != rule2.ConditionalFactors)
                yield return "Условные факторы";
            if (rule1.Type != rule2.Type)
                yield return "Тип";
            if (rule1.Factor != rule2.Factor)
                yield return "Фактор";
        }
    }
}
