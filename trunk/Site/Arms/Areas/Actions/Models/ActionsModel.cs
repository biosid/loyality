namespace Vtb24.Arms.Actions.Models
{
    public class ActionsModel
    {
        public ActionsOperationMode OperationMode { get; set; }

        public ActionModel[] Actions { get; set; }

        public long MechanicId { get; set; }

        public MechanicModel[] Mechanics { get; set; }

        public ActionsQueryModel Query { get; set; }

        public int TotalPages { get; set; }

        public ActionsPermissionsModel Permissions { get; set; }
    }
}
