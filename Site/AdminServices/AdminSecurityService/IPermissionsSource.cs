using System.Collections.Generic;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService
{
    public interface IPermissionsSource
    {
        string Name { get; }

        IEnumerable<string> this[PermissionKeys key] { get; }

        IEnumerable<KeyValuePair<PermissionKeys, string[]>> Enumerate();
    }
}
