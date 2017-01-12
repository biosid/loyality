using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Models.Groups
{
    public class GroupModel
    {
        public string Name { get; set; }

        public int UsersNumber { get; set; }

        public bool IsDeleteDenied { get; set; }

        public string DenyDeleteReason { get; set; }

        public static GroupModel Map(AdminGroup original)
        {
            return new GroupModel
            {
                Name = original.Name,
                UsersNumber = original.Users.Length
            };
        }
    }
}
