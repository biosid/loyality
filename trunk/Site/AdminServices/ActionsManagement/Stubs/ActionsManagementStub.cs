using System;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Mechanics.Models;
using Vtb24.Arms.AdminServices.Models;
using Action = Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Action;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Stubs
{
    class ActionsManagementStub : IActionsManagement
    {
        #region Механики

        public ActionsSearchResult SearchActions(ActionsSearchCriteria criteria, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public ActionHistoryResult GetActionHistory(long id, PagingSettings paging)
        {
            throw new NotImplementedException();
        }

        public Action GetAction(long id)
        {
            throw new NotImplementedException();
        }

        public long CreateAction(Action action)
        {
            throw new NotImplementedException();
        }

        public void UpdateAction(Action action)
        {
            throw new NotImplementedException();
        }

        public void DeleteAction(long id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Механики

        public Mechanic[] GetAllMechanics()
        {
            throw new NotImplementedException();
        }

        public Mechanic GetMechanic(long id)
        {
            throw new NotImplementedException();
        }

        public Metadata[] GetMetadataByMechanicId(long id)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
