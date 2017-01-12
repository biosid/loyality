namespace RapidSoft.VTB24.ArmSecurity.Interfaces
{
    using System.Collections.Generic;
    using System.Security.Principal;

    public interface IVtb24Principal // : IPrincipal
    {
        IIdentity Identity { get; }

        bool HasPermission(string permission);
        bool HasPermissions(IEnumerable<string> permissions);

        bool HasPermissionForPartner(string permission, string partnerId);
        bool HasPermissionsForPartner(IEnumerable<string> permissions, string partnerId);
    }
}