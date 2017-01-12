using System;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using Vtb24.Arms.Actions.Models;
using Vtb24.Arms.Helpers;
using Vtb24.Arms.Infrastructure;
using Vtb24.Arms.AdminServices;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models.Helpers;
using Vtb24.Arms.AdminServices.ActionsManagement.Models;
using Vtb24.Arms.AdminServices.Models;

using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.Actions.Controllers
{
    [Authorize]
    public class ActionsController : BaseController
    {
        private const int PAGE_SIZE = 50;

        public ActionsController(IActionsManagement actions, IAdminSecurityService security)
        {
            _actions = actions;
            _security = security;
        }

        private readonly IActionsManagement _actions;
        private readonly IAdminSecurityService _security;

        [HttpGet]
        public ActionResult Index()
        {
            var mechanic = _actions.GetAllMechanics().FirstOrDefault();

            if (mechanic == null)
            {
                throw new SecurityException("Нет доступных механик");
            }

            var query = new ActionsQueryModel
            {
                mechanic = mechanic.Id
            };

            return RedirectToAction("List", "Actions", query);
        }

        [HttpGet]
        public ActionResult List(ActionsOperationMode mode, ActionsQueryModel query)
        {
            if (!query.mechanic.HasValue)
            {
                return HttpNotFound();
            }

            var mechanics = _actions.GetAllMechanics().OrderBy(m => m.Name).ToArray();

            var mechanic = mechanics.FirstOrDefault(m => m.Id == query.mechanic.Value);

            if (mechanic == null)
            {
                return HttpNotFound();
            }

            var criteria = new ActionsSearchCriteria
            {
                MechanicId = mechanic.Id,
                Status = query.status.Map(),
                Type = query.type.Map(),
                SortDirection = ActionsSortDirection.Desc
            };

            switch (mode)
            {
                case ActionsOperationMode.Actual:
                    criteria.From = DateTime.Now.Date;
                    criteria.Sort = ActionsSort.Priority;
                    break;

                case ActionsOperationMode.Archive:
                    criteria.Until = DateTime.Now.Date;
                    criteria.Sort = ActionsSort.DateTimeTo;
                    break;

                case ActionsOperationMode.Search:
                    criteria.From = query.from.HasValue
                                        ? query.from.Value.Date
                                        : (DateTime?) null;
                    criteria.To = query.to.HasValue
                                      ? query.to.Value.Date.AddTicks(TimeSpan.TicksPerDay - 1)
                                      : (DateTime?) null;
                    criteria.Term = query.term;
                    criteria.Sort = ActionsSort.DateTimeTo;
                    break;
            }

            var paging = PagingSettings.ByPage(query.page ?? 1, PAGE_SIZE);

            var actions = _actions.SearchActions(criteria, paging);

            var permissions = ActionsPermissionsModel.Map(_security);

            var model = new ActionsModel
            {
                OperationMode = mode,
                Actions = actions.Select(ActionModel.Map).ToArray(),
                MechanicId = mechanic.Id,
                Mechanics = mechanics.Select(MechanicModel.Map).ToArray(),
                Query = query,
                TotalPages = actions.TotalPages,
                Permissions = permissions
            };
            return View("List", model);
        }

        [HttpGet]
        public ActionResult History(long id, int? page, string query)
        {
            var action = _actions.GetAction(id);

            if (action == null)
            {
                return HttpNotFound();
            }

            var paging = PagingSettings.ByPage(page ?? 1, PAGE_SIZE);

            var historyRecords = _actions.GetActionHistory(id, paging);

            var model = new ActionHistoryModel
            {
                query = query,
                page = page,
                Id = id,
                Name = action.Name,
                HistoryRecords = historyRecords.Select(HistoryRecordModel.Map).ToArray(),
                TotalPages = historyRecords.TotalPages
            };

            return View("History", model);
        }

        [HttpGet]
        public ActionResult Create(ActionsOperationMode mode, long mechanic, string query)
        {
            if (mode != ActionsOperationMode.Actual)
                return HttpNotFound();

            var metadata = _actions.GetMetadataByMechanicId(mechanic);

            var model = new ActionEditModel
            {
                query = query,
                IsNewAction = true,
                Priority = 1,
                Status = Statuses.active,
                Predicate = "{}",
                Metadata = MetadataHelpers.ToJson(metadata),
                MechanicId = mechanic,
                Type = Types.multiplication,
                Factor = 1
            };

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ActionsOperationMode mode, ActionEditModel model)
        {
            if (mode != ActionsOperationMode.Actual)
                return HttpNotFound();

            Predicate predicate;
            ConditionalFactor[] conditionalFactors;

            ExtractPredicateAndConditionalFactors(model, out predicate, out conditionalFactors);

            CheckDates(model);

            if (ModelState.IsValid)
            {
                HandleCreateUpdateExceptions(
                    () => _actions.CreateAction(ToAction(model, predicate, conditionalFactors)),
                    model);
            }

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("List", "Actions", model.ActionsQueryModel);
        }

        [HttpGet]
        public ActionResult Edit(long id, string query)
        {
            var action = _actions.GetAction(id);

            if (action == null)
            {
                return HttpNotFound();
            }

            var metadata = _actions.GetMetadataByMechanicId(action.MechanicId);

            var model = ActionEditModel.Map(action, metadata, query);

            Hydrate(model);

            return View("Edit", model);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ActionEditModel model)
        {
            Predicate predicate;
            ConditionalFactor[] conditionalFactors;

            ExtractPredicateAndConditionalFactors(model, out predicate, out conditionalFactors);

            CheckDates(model);

            if (ModelState.IsValid)
            {
                HandleCreateUpdateExceptions(
                    () => _actions.UpdateAction(ToAction(model, predicate, conditionalFactors)),
                    model);
            }

            if (!ModelState.IsValid)
            {
                Hydrate(model);
                return View("Edit", model);
            }

            return RedirectToAction("List", "Actions", model.ActionsQueryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            _actions.DeleteAction(id);

            return JsonSuccess();
        }

        private void Hydrate(ActionEditModel model)
        {
            var mechanic = _actions.GetMechanic(model.MechanicId);

            model.MechanicName = mechanic.Name;
        }

        private void ExtractPredicateAndConditionalFactors(ActionEditModel model, out Predicate predicate, out ConditionalFactor[] conditionalFactors)
        {
            try
            {
                predicate = PredicateHelpers.JsonToPredicate(model.Predicate);
                if (predicate != null && predicate.Operation == null && predicate.Union == null)
                    predicate = null;
            }
            catch (PredicateMappingException)
            {
                predicate = null;
                ModelState.AddModelError("Predicate", "Неверное условие");
            }

            conditionalFactors = model.ConditionalFactors != null
                                     ? model.ConditionalFactors.Select(ExtractConditionalFactor).ToArray()
                                     : null;
        }

        private ConditionalFactor ExtractConditionalFactor(ConditionalFactorModel model, int index)
        {
            Predicate predicate = null;
            try
            {
                predicate = PredicateHelpers.JsonToPredicate(model.Predicate);
                if (predicate == null || (predicate.Operation == null && predicate.Union == null))
                    ModelState.AddModelError("ConditionalFactors[" + index.ToString("D") + "].Predicate", "Необходимо указать условие");
            }
            catch (PredicateMappingException)
            {
                ModelState.AddModelError("ConditionalFactors[" + index.ToString("D") + "].Predicate", "Неверное условие");
            }

            return new ConditionalFactor
            {
                Factor = model.Factor,
                Priority = model.Priority,
                Predicate = predicate
            };
        }

        private void CheckDates(ActionEditModel model)
        {
            if (model.From.HasValue && model.From.Value.Year < 2000)
                ModelState.AddModelError("From", "Укажите дату не ранее 1.01.2000");

            if (model.To.HasValue && model.To.Value.Year < 2000)
                ModelState.AddModelError("To", "Укажите дату не ранее 1.01.2000");
        }

        private static void SwapDates(ActionEditModel model)
        {
            if (!model.From.HasValue || !model.To.HasValue || model.From.Value < model.To.Value)
                return;

            var tmp = model.From;
            model.From = model.To;
            model.To = tmp;
        }

        private static Action ToAction(ActionEditModel model, Predicate predicate, ConditionalFactor[] conditionalFactors)
        {
            SwapDates(model);

            var action = new Action
            {
                Name = model.Name,
                // выравниваем дату начала на начало суток
                From = model.From.HasValue
                           ? model.From.Value.Date
                           : (DateTime?) null,
                // выравниваем дату конца на конец суток
                To = model.To.HasValue
                         ? model.To.Value.Date.AddTicks(TimeSpan.TicksPerDay - TimeSpan.TicksPerSecond)
                         : (DateTime?) null,
                Priority = model.Priority,
                IsExclusive = model.IsExclusive,
                IsNotExcludedBy = model.IsNotExcludedBy,
                Status = model.Status.Map(),
                Predicate = predicate,
                MechanicId = model.MechanicId,
                Type = model.Type.Map(),
                Factor = model.Factor,
                ConditionalFactors = conditionalFactors
            };

            if (!model.IsNewAction)
                action.Id = model.Id;

            return action;
        }

        private void HandleCreateUpdateExceptions(System.Action statement, ActionEditModel model)
        {
            try
            {
                statement();
            }
            catch (BaseActionPredicateWithTargetException)
            {
                ModelState.AddModelError("Predicate",
                                         "Условие базовой механики не должно содержать целевую аудиторию");
            }
            catch (ActionPredicateException)
            {
                ModelState.AddModelError("Predicate", "Неверное условие");
            }
            catch (BaseActionConditionalFactorsWithTargetException)
            {
                ModelState.AddModelError("ConditionalFactors",
                                         "Условные факторы базовой механики не должны содержать целевую аудиторию");
            }
            catch (ActionConditionalFactorsException)
            {
                ModelState.AddModelError("ConditionalFactors", "Неверные условные факторы");
            }
            catch (ActionsBaseRuleConflictException)
            {
                ModelState.AddModelError("Type",
                                         string.Format("{0} с приоритетом {1} уже существует",
                                                       model.Type.EnumDescription(),
                                                       model.Priority));
            }
        }
    }
}
