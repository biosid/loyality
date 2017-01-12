namespace RapidSoft.VTB24.ArmSecurity.Entities
{
    using System.DirectoryServices;
    using System.Linq;
    using System.Security.Principal;

    public class UserIdentity : IIdentity
    {
        public UserIdentity(SearchResult adUser)
        {
            if (adUser != null && adUser.Properties != null)
            {
                var name = adUser.GetValues<string>(SearchResultExtensions.AccountNameAttrName).SingleOrDefault();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    this.Name = name;
                    this.AuthenticationType = "ActiveDirectory";
                    this.IsAuthenticated = true;
                    return;
                }
            }

            this.Name = null;
            this.AuthenticationType = null;
            this.IsAuthenticated = false;
        }

        public string Name { get; private set; }

        public string AuthenticationType { get; private set; }

        // NOTE: Здесь не происходит аутентификации в полном смысле.
        public bool IsAuthenticated { get; private set; }
    }
}