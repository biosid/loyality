namespace RapidSoft.VTB24.ArmSecurity
{
    using System;
    using System.Collections.Generic;

    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public static class ArmSecurity
    {
        private static Func<IUserService> userServiceCreator;

        public static Func<IUserService> UserServiceCreator
        {
            get
            {
                return userServiceCreator ?? (() => new UserService());
            }

            set
            {
                userServiceCreator = value;
            }
        }

        public static bool CheckPermissionByAccountName(string accountName, string permission)
        {
            var principal = GetUserPrincipal(accountName);

            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            return principal.HasPermission(permission);
        }

        public static bool CheckPermissionsByAccountName(string accountName, IEnumerable<string> permissions)
        {
            var principal = GetUserPrincipal(accountName);

            if (principal == null)
            {
                return false;
            }

            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            return principal.HasPermissions(permissions);
        }

        public static bool CheckObjectPermissionByAccountName(
            string accountName, string permissionName, string partnerId)
        {
            var principal = GetUserPrincipal(accountName);

            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            return principal.HasPermissionForPartner(permissionName, partnerId);
        }

        public static bool CheckObjectPermissionsByAccountName(
            string accountName, IEnumerable<string> permissions, string partnerId)
        {
            var principal = GetUserPrincipal(accountName);

            if (!principal.Identity.IsAuthenticated)
            {
                return false;
            }

            return principal.HasPermissionsForPartner(permissions, partnerId);
        }

        private static IVtb24Principal GetUserPrincipal(string accountName)
        {
            var userService = UserServiceCreator();

            var retVal = userService.GetUserPrincipalByName(accountName);

            return retVal;
        }
    }
}
