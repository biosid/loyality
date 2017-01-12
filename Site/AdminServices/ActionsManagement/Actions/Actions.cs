using System.Collections.Generic;
using System.Linq;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Models;
using Vtb24.Arms.AdminServices.AdminMechanicsService;
using Vtb24.Arms.AdminServices.Infrastructure;
using Vtb24.Arms.AdminServices.Models;
using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions
{
    public class Actions : IActions
    {
        public const string AUDIENCE_ATTRIBUTE_TOKEN = "__TARGET_AUDIENCE__";
        public const string AUDIENCE_ATTRIBUTE_REGEXP = "^(Audience_[0-9]+)|(" + AUDIENCE_ATTRIBUTE_TOKEN + ")$";
        public const string AUDIENCE_ATTRIBUTE_START = "Audience_";

        private const string AUDIENCE_ATTRIBUTE_NAME = "ClientProfile.Audiences";

        public Actions(IAdminSecurityService security)
        {
            _security = security;
        }

        private readonly IAdminSecurityService _security;

        public ActionsSearchResult SearchActions(ActionsSearchCriteria criteria, PagingSettings paging)
        {
            var parameters = new GetRulesParameters
            {
                RuleDomainId = criteria.MechanicId,
                DateTimeFrom = criteria.From,
                DateTimeTo = criteria.To,
                DateTimeUntil = criteria.Until,
                SearchTerm = criteria.Term,
                Status = MappingsToService.ToRuleStatuses(criteria.Status),
                Type = MappingsToService.ToRuleTypes(criteria.Type),
                SortProperty = MappingsToService.ToSortProperty(criteria.Sort),
                SortDirect = MappingsToService.ToSortDirections(criteria.SortDirection),
                CountSkip = paging.Skip,
                CountTake = paging.Take,
                CalcTotalCount = true,
                UserId = _security.CurrentUser
            };

            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.GetRules(parameters);

                response.AssertSuccess();

                var actions = response.Rules.Select(MappingsFromService.ToAction).ToArray();
                return new ActionsSearchResult(actions, response.TotalCount ?? actions.Length, paging);
            }
        }

        public ActionHistoryResult GetActionHistory(long id, PagingSettings paging)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var parameters = new GetRuleHistoryParameters
                {
                    RuleId = id,
                    CalcTotalCount = true,
                    CountSkip = paging.Skip,
                    CountTake = paging.Take,
                    UserId = _security.CurrentUser
                };

                var response = service.GetRuleHistory(parameters);

                response.AssertSuccess();

                var historyRecords = MappingsFromService.ToHistoryRecords(response.RuleHistories).ToArray();

                return new ActionHistoryResult(historyRecords, response.TotalCount ?? historyRecords.Length, paging);
            }
        }

        public Action GetAction(long id)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.GetRule(id, _security.CurrentUser);

                response.AssertSuccess();

                return response.Rule != null
                           ? MappingsFromService.ToAction(response.Rule)
                           : null;
            }
        }

        public long CreateAction(Action action)
        {
            ValidateAction(action);

            if (!GetOperations(action).Any(ContainsTargetAudienceToken))
                return CreateRule(MappingsToService.ToRule(action));

            var actionName = action.Name;
            action.Name = "[Черновик - " + actionName + "]";

            var ruleId = CreateRule(MappingsToService.ToRule(action));

            action.Id = ruleId;
            action.Name = actionName;
            SetTargetAudience(action, ruleId);

            UpdateAction(action);

            return ruleId;
        }

        public void UpdateAction(Action action)
        {
            ValidateAction(action);

            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.UpdateRule(MappingsToService.ToRule(action), _security.CurrentUser);

                response.AssertSuccess();
            }
        }

        public void DeleteAction(long id)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.DeleteRuleById(id, _security.CurrentUser);

                response.AssertSuccess();
            }
        }

        public static string FormatAudienceName(long id)
        {
            return "Audience_" + id.ToString("D");
        }

        private static void ValidateAction(Action action)
        {
            if (action.Type != ActionType.BaseAddition && action.Type != ActionType.BaseMultiplication)
                return;

            if (action.Predicate != null && GetOperations(action.Predicate).Any(ContainsTargetAudience))
                throw new BaseActionPredicateWithTargetException();

            if (action.ConditionalFactors != null && GetOperations(action.ConditionalFactors).Any(ContainsTargetAudience))
                throw new BaseActionConditionalFactorsWithTargetException();
        }

        private long CreateRule(Rule rule)
        {
            using (var service = new AdminMechanicsServiceClient())
            {
                var response = service.CreateRule(rule, _security.CurrentUser);

                response.AssertSuccess();

                return response.Rule.Id;
            }
        }

        private static bool ContainsTargetAudience(PredicateOperation op)
        {
            return op.Attribute == AUDIENCE_ATTRIBUTE_NAME &&
                   op.Values != null && op.Values.Length > 0 &&
                   (op.Values[0] == AUDIENCE_ATTRIBUTE_TOKEN ||
                    op.Values[0].StartsWith(AUDIENCE_ATTRIBUTE_START));
        }

        private static bool ContainsTargetAudienceToken(PredicateOperation op)
        {
            return op.Attribute == AUDIENCE_ATTRIBUTE_NAME &&
                   op.Values != null && op.Values.Length > 0 &&
                   op.Values[0] == AUDIENCE_ATTRIBUTE_TOKEN;
        }

        #region GetOperations

        private static IEnumerable<PredicateOperation> GetOperations(Action action)
        {
            return action.Predicate != null
                       ? (action.ConditionalFactors != null
                              ? GetOperations(action.Predicate).Concat(GetOperations(action.ConditionalFactors))
                              : GetOperations(action.Predicate))
                       : (action.ConditionalFactors != null
                              ? GetOperations(action.ConditionalFactors)
                              : Enumerable.Empty<PredicateOperation>());
        }

        private static IEnumerable<PredicateOperation> GetOperations(IEnumerable<ConditionalFactor> conditionalFactors)
        {
            return conditionalFactors.Where(f => f.Predicate != null)
                                     .SelectMany(f => GetOperations(f.Predicate));
        }

        private static IEnumerable<PredicateOperation> GetOperations(Predicate predicate)
        {
            return predicate.Union != null
                       ? (predicate.Operation != null
                              ? GetOperations(predicate.Union).Concat(new[] { predicate.Operation })
                              : GetOperations(predicate.Union))
                       : (predicate.Operation != null
                              ? new[] { predicate.Operation }
                              : Enumerable.Empty<PredicateOperation>());
        }

        private static IEnumerable<PredicateOperation> GetOperations(PredicateUnion un)
        {
            return
                (un.Operations ?? Enumerable.Empty<PredicateOperation>())
                    .Concat(un.Unions != null
                                ? un.Unions.SelectMany(GetOperations)
                                : Enumerable.Empty<PredicateOperation>());
        }

        #endregion

        #region SetTargetAudience

        private static void SetTargetAudience(Action action, long ruleId)
        {
            var values = new[] { FormatAudienceName(ruleId) };

            if (action.Predicate != null)
                SetTargetAudience(action.Predicate, values);

            if (action.ConditionalFactors != null)
                SetTargetAudience(action.ConditionalFactors, values);
        }

        private static void SetTargetAudience(IEnumerable<ConditionalFactor> conditionalFactors, string[] values)
        {
            foreach (var conditionalFactor in conditionalFactors.Where(f => f.Predicate != null))
                SetTargetAudience(conditionalFactor.Predicate, values);
        }

        private static void SetTargetAudience(Predicate predicate, string[] values)
        {
            if (predicate.Operation != null)
                SetTargetAudience(predicate.Operation, values);
            else
                SetTargetAudience(predicate.Union, values);
        }

        private static void SetTargetAudience(PredicateUnion un, string[] values)
        {
            if (un.Operations != null)
                foreach (var op in un.Operations)
                    SetTargetAudience(op, values);
            if (un.Unions != null)
                foreach (var subUn in un.Unions)
                    SetTargetAudience(subUn, values);
        }

        private static void SetTargetAudience(PredicateOperation op, string[] values)
        {
            if (ContainsTargetAudienceToken(op))
                op.Values = values;
        }

        #endregion
    }
}
