namespace RapidSoft.VTB24.BankConnector.Tests.StubServices
{
    using System.Collections.Generic;
    using System.Security.Principal;

    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public class ArmSecurityStub : IUserService
    {
        private class IdentityStub : IIdentity
        {
            public string Name { get; private set; }

            public string AuthenticationType
            {
                get
                {
                    return "Stub";
                }
            }

            public bool IsAuthenticated
            {
                get
                {
                    return true;
                }
            }

            public IdentityStub(string name)
            {
                this.Name = name;
            }
        }

        private class Vtb24PrincipalStub : IVtb24Principal
        {
            public IIdentity Identity { get; private set; }

            public bool HasPermission(string permission)
            {
                return true;
            }

            public bool HasPermissions(IEnumerable<string> permissions)
            {
                return true;
            }

            public bool HasPermissionForPartner(string permission, string partnerId)
            {
                return true;
            }

            public bool HasPermissionsForPartner(IEnumerable<string> permissions, string partnerId)
            {
                return true;
            }

            public Vtb24PrincipalStub(string name)
            {
                this.Identity = new IdentityStub(name);
            }
        }

        public IVtb24Principal GetUserPrincipalByName(string name)
        {
            return new Vtb24PrincipalStub(name);
        }
    }
}
