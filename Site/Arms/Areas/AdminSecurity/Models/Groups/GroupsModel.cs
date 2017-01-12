namespace Vtb24.Arms.AdminSecurity.Models.Groups
{
    public class GroupsModel
    {
        public GroupModel[] Groups { get; set; }

        public GroupsQueryModel Query { get; set; }

        public int TotalPages { get; set; }
    }
}
