using System.ComponentModel.DataAnnotations;
using Vtb24.Arms.AdminSecurity.Models.Shared;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminSecurity.Models.Groups
{
    public class GroupEditModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        // ReSharper restore InconsistentNaming

        public bool IsNewGroup { get; set; }

        public string[] AllUsers { get; set; }

        public PermissionsEditModel Permissions { get; set; }

        [Required(ErrorMessage = "Необходимо указать название")]
        [StringLength(16, ErrorMessage = "Превышена допустимая длина названия (16 символов)")]
        [RegularExpression(@"[a-zA-Z0-9._-]*", ErrorMessage = "Название должно состоять из латинских букв, цифр, символов . _ -")]
        public string Name { get; set; }

        public string[] Users { get; set; }

        public GroupsQueryModel GroupsQueryModel
        {
            get
            {
                return string.IsNullOrWhiteSpace(query)
                           ? new GroupsQueryModel()
                           : new GroupsQueryModel().MixQuery(query);
            }
        }

        public static GroupEditModel Create(string query, PermissionNode[] nodes)
        {
            return new GroupEditModel
            {
                query = query,
                IsNewGroup = true,
                Permissions = new PermissionsEditModel
                {
                    Nodes = nodes
                }
            };
        }

        public static GroupEditModel Map(AdminGroup group, string query, PermissionNode[] nodes)
        {
            return new GroupEditModel
            {
                query = query,
                IsNewGroup = false,
                Name = group.Name,
                Users = group.Users,
                Permissions = new PermissionsEditModel
                {
                    Nodes = nodes,
                    Permissions = group.Permissions
                }
            };
        }
    }
}
