using Vtb24.Arms.AdminServices.ActionsManagement.Actions;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement
{
    public class ActionsManagement : IActionsManagement
    {
        public ActionsManagement(IAdminSecurityService security)
        {
            _actions = new Actions.Actions(security);
            _mechanics = new Mechanics.Mechanics(security);
        }

        private readonly IActions _actions;
        private readonly IMechanics _mechanics;

        #region Механики

        public ActionsSearchResult SearchActions(ActionsSearchCriteria criteria, PagingSettings paging)
        {
            return _actions.SearchActions(criteria, paging);
        }

        public ActionHistoryResult GetActionHistory(long id, PagingSettings paging)
        {
            return _actions.GetActionHistory(id, paging);
        }

        public Action GetAction(long id)
        {
            return _actions.GetAction(id);
        }

        public long CreateAction(Action action)
        {
            return _actions.CreateAction(action);
        }

        public void UpdateAction(Action action)
        {
            _actions.UpdateAction(action);
        }

        public void DeleteAction(long id)
        {
            _actions.DeleteAction(id);
        }

        #endregion

        #region Механики

        public Mechanic[] GetAllMechanics()
        {
            return _mechanics.GetAllMechanics();
        }

        public Mechanic GetMechanic(long id)
        {
            return _mechanics.GetMechanic(id);
        }

        public Metadata[] GetMetadataByMechanicId(long id)
        {
            return _mechanics.GetMetadataByMechanicId(id);
        }

        #endregion
    }
}
