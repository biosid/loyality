namespace RapidSoft.VTB24.ArmSecurity
{
    using System;
    using System.DirectoryServices;
    using System.Linq;

    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    public class ActiveDirectoryProvider : IActiveDirectoryProvider
    {
        private const string SearchUserFilterFormate = "(&(ObjectClass=user)(sAMAccountName={0}))";
        private const string SearchAllUserGroup = "(&(ObjectClass=group))";

        private readonly DirectoryEntry directoryEntry;

        public ActiveDirectoryProvider(string path, string username, string password)
        {
            this.directoryEntry = new DirectoryEntry(path, username, password);
        }

        public ActiveDirectoryProvider()
        {
            var config = ActiveDirectoryConfigSection.Current;

            if (config == null || config.Connection == null)
            {
                throw new Exception("Не заданы настройки подключения к Active Directory");
            }

            var connection = config.Connection;
            this.directoryEntry = new DirectoryEntry(connection.Path, connection.UserName, connection.Password);
        }

        public SearchResult SearchUser(string userName)
        {
            var filter = string.Format(SearchUserFilterFormate, userName);
            var userSearcher = new DirectorySearcher(this.directoryEntry)
                                   {
                                       SearchScope = SearchScope.Subtree,
                                       Filter = filter
                                   };
            var retVal = userSearcher.FindOne();
            return retVal;
        }

        public SearchResult[] SearchAllUserGroups()
        {
            var groupSearcher = new DirectorySearcher(this.directoryEntry)
                                    {
                                        SearchScope = SearchScope.Subtree,
                                        Filter = SearchAllUserGroup
                                    };
            var groups = groupSearcher.FindAll();

            var retVal = groups.Cast<SearchResult>().ToArray();
            return retVal;
        }
    }
}
