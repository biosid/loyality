using System;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Models
{
    public class AdminUser
    {
        public string Login { get; set; }

        public DateTime WhenCreated { get; set; }

        public string[] Groups { get; set; }

        public IPermissionsSource Permissions { get; set; }

        public IPermissionsSource[] InheritedPermissions { get; set; }
    }
}
