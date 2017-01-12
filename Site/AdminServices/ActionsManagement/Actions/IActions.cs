using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Inputs;
using Vtb24.Arms.AdminServices.ActionsManagement.Actions.Models.Outputs;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.ActionsManagement.Actions
{
    public interface IActions
    {
        ActionsSearchResult SearchActions(ActionsSearchCriteria criteria, PagingSettings paging);

        ActionHistoryResult GetActionHistory(long id, PagingSettings paging);

        Action GetAction(long id);

        long CreateAction(Action action);

        void UpdateAction(Action action);

        void DeleteAction(long id);
    }
}
