using System;
using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.AdminSecurity.Models.Shared;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Models.Users
{
    public class UserEditModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        // ReSharper restore InconsistentNaming

        public bool IsNewUser { get; set; }

        public string[] AllGroups { get; set; }

        public PermissionsEditModel Permissions { get; set; }

        public ResetUserPasswordModel ResetPassword { get; set; }

        [Required(ErrorMessage = "Необходимо указать логин")]
        [StringLength(16, ErrorMessage = "Превышена допустимая длина логина (16 символов)")]
        [RegularExpression(@"[a-zA-Z0-9._-]*", ErrorMessage = "Логин должен состоять из латинских букв, цифр, символов . _ -")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо указать пароль")]
        [StringLength(64, MinimumLength = 8, ErrorMessage = "Неверная длина пароля (допускается от 8 до 64 символов)")]
        [RegularExpression(@"^(?=.*\d)(?=.*[A-Z])(?=.*[a-z])[0-9A-Za-z]+$", ErrorMessage = "Пароль должен состоять из латинских букв и цифр. Пароль должен включать хотя бы одну цифру, хотя бы одну маленькую букву и хотя бы одну большую букву.")]
        public string Password { get; set; }

        public DateTime WhenCreated { get; set; }

        public string[] Groups { get; set; }

        public UsersQueryModel UsersQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                           ? new UsersQueryModel()
                           : new UsersQueryModel().MixQuery(query);
            }
        }

        public static UserEditModel Create(string query, PermissionNode[] nodes)
        {
            return new UserEditModel
            {
                query = query,
                IsNewUser = true,
                Permissions = new PermissionsEditModel
                {
                    Nodes = nodes
                }
            };
        }

        public static UserEditModel Map(AdminUser original, string query, PermissionNode[] nodes)
        {
            return new UserEditModel
            {
                query = query,
                IsNewUser = false,
                Login = original.Login,
                WhenCreated = original.WhenCreated,
                Groups = original.Groups,
                Permissions = new PermissionsEditModel
                {
                    Nodes = nodes,
                    Permissions = original.Permissions,
                    InheritedPermissions = original.InheritedPermissions
                }
            };
        }
    }
}
