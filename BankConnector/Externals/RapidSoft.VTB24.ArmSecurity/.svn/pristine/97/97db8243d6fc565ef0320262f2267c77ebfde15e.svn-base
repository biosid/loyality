namespace RapidSoft.VTB24.ArmSecurity
{
    using System.Linq;

    using RapidSoft.VTB24.ArmSecurity.Entities;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public class UserService : IUserService
    {
        private readonly IActiveDirectoryProvider adProvider;

        public UserService(IActiveDirectoryProvider adProvider = null)
        {
            this.adProvider = adProvider ?? new ActiveDirectoryProvider();
        }

        public IVtb24Principal GetUserPrincipalByName(string name)
        {
            var userSearchResult = this.adProvider.SearchUser(name);

            var identity = new UserIdentity(userSearchResult);

            if (!identity.IsAuthenticated)
            {
                return new Vtb24Principal(identity, null);
            }

            var searchGroups = this.adProvider.SearchAllUserGroups();

            var memberOf = userSearchResult.GetValues<string>(SearchResultExtensions.MemberOfAttrName);

            var userGroups = searchGroups.Where(x => x.IsHasValue(SearchResultExtensions.DistinguishedNameAttrName, memberOf));

            var withUser = new[] { userSearchResult }.Union(userGroups);

            var principal = new Vtb24Principal(identity, withUser);

            return principal;
        }
    }
}