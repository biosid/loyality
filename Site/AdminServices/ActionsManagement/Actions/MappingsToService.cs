using System;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers;
using Vtb24.Arms.AdminServices.ActionsManagement.Models;
using Vtb24.Arms.AdminServices.AdminMechanicsService;

using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions
{
    internal static class MappingsToService
    {
        public static Rule ToRule(Action original)
        {
            var rule = new Rule
            {
                Id = original.Id,
                Name = original.Name,
                DateTimeFrom = original.From,
                DateTimeTo = original.To,
                Priority = original.Priority,
                IsExclusive = original.IsExclusive,
                IsNotExcludedBy = original.IsNotExcludedBy,
                Status = ToRuleStatuses(original.Status),
                RuleDomainId = original.MechanicId,
                Type = ToRuleTypes(original.Type),
                Factor = original.Factor
            };

            try
            {
                rule.Predicate = PredicateHelpers.PredicateToXml(original.Predicate);
            }
            catch (PredicateMappingException ex)
            {
                throw new ActionPredicateException("Ошибка при сохранении условия", ex);
            }

            try
            {
                rule.ConditionalFactors = PredicateHelpers.ConditionalFactorsToXml(original.ConditionalFactors);
            }
            catch (PredicateMappingException ex)
            {
                throw new ActionConditionalFactorsException("Ошибка при сохранении условных факторов", ex);
            }

            return rule;
        }

        public static RuleStatuses? ToRuleStatuses(ActionStatus? original)
        {
            return original.HasValue
                       ? ToRuleStatuses(original.Value)
                       : (RuleStatuses?) null;
        }

        public static RuleStatuses ToRuleStatuses(ActionStatus original)
        {
            switch (original)
            {
                case ActionStatus.Active:
                    return RuleStatuses.Active;
                case ActionStatus.NotActive:
                    return RuleStatuses.NotActive;
            }

            throw new InvalidOperationException("Неизвестный статус механики");
        }

        public static RuleTypes? ToRuleTypes(ActionType? original)
        {
            return original.HasValue
                       ? ToRuleTypes(original.Value)
                       : (RuleTypes?) null;
        }

        public static RuleTypes ToRuleTypes(ActionType original)
        {
            switch (original)
            {
                case ActionType.Addition:
                    return RuleTypes.Addition;
                case ActionType.Multiplication:
                    return RuleTypes.Multiplication;
                case ActionType.BaseAddition:
                    return RuleTypes.BaseAddition;
                case ActionType.BaseMultiplication:
                    return RuleTypes.BaseMultiplication;
            }

            throw new InvalidOperationException("Неизвестный тип механики");
        }

        public static SortProperty ToSortProperty(ActionsSort original)
        {
            switch (original)
            {
                case ActionsSort.Priority:
                    return SortProperty.Priority;
                case ActionsSort.DateTimeTo:
                    return SortProperty.DateTimeTo;
            }

            throw new InvalidOperationException("Неизвестный тип сортировки");
        }

        public static SortDirections ToSortDirections(ActionsSortDirection original)
        {
            switch (original)
            {
                case ActionsSortDirection.Asc:
                    return SortDirections.Asc;
                case ActionsSortDirection.Desc:
                    return SortDirections.Desc;
            }

            throw new InvalidOperationException("Неизвестный порядок сортировки");
        }
    }
}
