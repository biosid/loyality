using System;
using System.Linq;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
            : base("недостаточно прав")
        {
        }

        public AccessDeniedException(params PermissionKeys[] keys)
            : base("недостаточно прав, необходимые права: " +
                   string.Join(", ", keys.Select(key => key.ToString())))
        {
        }
    }
}
