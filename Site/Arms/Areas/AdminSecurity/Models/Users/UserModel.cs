using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Models.Users
{
    public class UserModel
    {
        public string Login { get; set; }

        public string[] Groups { get; set; }

        public bool IsDeleteDenied { get; set; }

        public string DenyDeleteReason { get; set; }

        public static UserModel Map(AdminUser original)
        {
            return new UserModel
            {
                Login = original.Login,
                Groups = original.Groups
            };
        }
    }
}
