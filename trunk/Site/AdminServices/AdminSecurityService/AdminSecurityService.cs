using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Exceptions;
using Vtb24.Arms.AdminServices.AdminSecurityService.Helpers;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models.Internal;
using Vtb24.Arms.AdminServices.AdminSecurityService.Models.Output;
using Vtb24.Arms.AdminServices.Infrastructure.AdminSecurityConfiguration;
using Vtb24.Arms.AdminServices.Models;

namespace Vtb24.Arms.AdminServices.AdminSecurityService
{
    public class AdminSecurityService : IAdminSecurityService
    {
        #region Интерфейс для запросов прав текущего пользователя

        public bool IsAuthenticated
        {
            get { return ClaimsPrincipal.Current.Identity.IsAuthenticated; }
        }

        public string CurrentUser
        {
            get { return ClaimsPrincipal.Current.Identity.Name; }
        }

        public IPermissionsSource CurrentPermissions
        {
            get
            {
                return IsAuthenticated
                           ? GetEffectiveUserPermissionsByLogin(CurrentUser)
                           : EmptyPermissions;
            }
        }

        #endregion

        #region Интерфейс для проверки учётных данных пользователей

        public bool Login(string login, string password)
        {
            return ValidateUser(login, password) && VerifyUserLoginPermissions(login);
        }

        public bool VerifyCurrentUser()
        {
            return !IsAuthenticated || VerifyUserLoginPermissions(CurrentUser);
        }

        #endregion

        #region Интрефейс для управления учётными записями

        public PermissionNode[] PermissionNodes { get { return Nodes;  } }

        public string[] GetGroupNames()
        {
            AssertManagePermissions();

            return GetAllGroupDisplayNames().ToArray();
        }

        public string[] GetUserNames()
        {
            AssertManagePermissions();

            return GetAllUserLogins().ToArray();
        }

        public AdminGroup GetGroupByName(string name)
        {
            AssertManagePermissions();

            var group = FindGroupByDisplayName(name);

            return group != null ? ToAdminGroup(group, GetAllUserLogins()) : null;
        }

        public AdminUser GetUserByLogin(string login)
        {
            AssertManagePermissions();

            var user = FindUserByLogin(login);

            return user != null ? ToAdminUser(user) : null;
        }

        public IPermissionsSource GetPermissionsByLogin(string login)
        {
            AssertManagePermissions();

            var user = FindUserByLogin(login);

            return user != null ? GetUserPermissions(user) : EmptyPermissions;
        }

        public IPermissionsSource[] GetInheritedPermissionsByLogin(string login)
        {
            AssertManagePermissions();

            var user = FindUserByLogin(login);

            return FindGroupsByMember(user).Select(GetGroupPermissions).Cast<IPermissionsSource>().ToArray();
        }

        public GetGroupsResult GetGroups(PagingSettings paging)
        {
            AssertManagePermissions();

            var allUserLogins = GetAllUserLogins().ToArray();

            var allGroups = FindAllGroups().OrderBy(GetGroupDisplayName).ToArray();

            var groups = allGroups.Skip(paging.Skip)
                                  .Take(paging.Take)
                                  .Select(g => ToAdminGroup(g, allUserLogins))
                                  .ToArray();

            return new GetGroupsResult(groups, allGroups.Length, paging);
        }

        public GetUsersResult GetUsers(PagingSettings paging)
        {
            AssertManagePermissions();

            var allUsers = FindAllUsers().OrderBy(u => u.AccountName()).ToArray();

            var users = allUsers.Skip(paging.Skip)
                                .Take(paging.Take)
                                .Select(ToAdminUser)
                                .ToArray();

            return new GetUsersResult(users, allUsers.Length, paging);
        }

        public void CreateGroup(AdminGroup group)
        {
            AssertManagePermissions();
            AssertGroupDoesNotExists(group.Name);

            var groupEntry = CreateGroupEntry(group.Name);
            //WritePermissions(groupEntry, group.Permissions);
            SetGroupMembers(groupEntry, group.Users);

            CommitEntry(groupEntry);
        }

        public void UpdateGroup(AdminGroup group)
        {
            AssertManagePermissions();
            AssertGroupEditOfSelf(group);

            var groupEntry = GetGroupEntry(group.Name);
            WritePermissions(groupEntry, group.Permissions);
            SetGroupMembers(groupEntry, group.Users);

            CommitEntry(groupEntry);
        }

        public void DeleteGroup(string name)
        {
            AssertManagePermissions();
            AssertGroupDeleteOfSelf(name);

            try
            {
                DeleteEntry(GetGroupEntry(name));
            }
            catch (AdminSecurityGroupNotFoundException)
            {
            }
        }

        public void CreateUser(AdminUser user, string password)
        {
            AssertManagePermissions();
            AssertUserDoesNotExists(user.Login);

            var userEntry = CreateUserEntry(user.Login);
            CommitEntry(userEntry);

            try
            {
                SetUserPassword(userEntry, password);
                AddGroupMember(FindCommonGroup().GetDirectoryEntry(), userEntry.DistinguishedName());
                UpdateUserMembership(userEntry, user.Groups ?? Enumerable.Empty<string>());
                WritePermissions(userEntry, user.Permissions);
                ActivateUser(userEntry);
            }
            catch (AdminSecurityServiceException)
            {
                DeleteEntryNoThrow(userEntry);
                throw;
            }
        }

        public void UpdateUser(AdminUser user)
        {
            AssertManagePermissions();
            AssertSelfEditIsValid(user);

            var userEntry = GetUserEntry(user.Login);

            UpdateUserMembership(userEntry, user.Groups ?? Enumerable.Empty<string>());
            WritePermissions(userEntry, user.Permissions);
            CommitEntry(userEntry);
        }

        public void DeleteUser(string login)
        {
            AssertManagePermissions();
            AssertSelfDeleteAttempt(login);

            try
            {
                DeleteEntry(GetUserEntry(login));
            }
            catch (AdminSecurityUserNotFoundException)
            {
            }
        }

        public void ResetUserPassword(string login, string password)
        {
            AssertManagePermissions();

            var userEntry = GetUserEntry(login);

            SetUserPassword(userEntry, password);
        }

        #endregion

        #region Инициализация

        public AdminSecurityService()
        {
            // читаем конфигурацию для подключения к AD
            _domainPath = ConfigurationManager.AppSettings[DOMAIN_PATH_CONFIG_NAME];

            var domainPortString = ConfigurationManager.AppSettings[DOMAIN_PORT_CONFIG_NAME];
            _domainPort = string.IsNullOrEmpty(domainPortString)
                              ? DOMAIN_DEFAULT_PORT
                              : int.Parse(domainPortString);

            var user = ConfigurationManager.AppSettings[DOMAIN_USER_CONFIG_NAME];
            var password = ConfigurationManager.AppSettings[DOMAIN_PASSWORD_CONFIG_NAME];

            // проверяем конфигурацию
            if (string.IsNullOrWhiteSpace(_domainPath))
            {
                throw new AdminSecurityGeneralException("путь к доменному контроллеру не задан в конфигурации");
            }

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(password))
            {
                throw new AdminSecurityGeneralException(
                    "учётные данные доменного администратора не заданы в конфигурации");
            }

            _domain = new DirectoryEntry(_domainPath + ":" + _domainPort, user, password,
                                         AuthenticationTypes.Signing |
                                         AuthenticationTypes.Sealing |
                                         AuthenticationTypes.Secure);

            _commonGroupDistinguishedName = FindCommonGroup().DistinguishedName();
        }

        private const string DOMAIN_PATH_CONFIG_NAME = "arm_security_domain_path";
        private const string DOMAIN_PORT_CONFIG_NAME = "arm_security_domain_port";
        private const string DOMAIN_USER_CONFIG_NAME = "arm_security_domain_user";
        private const string DOMAIN_PASSWORD_CONFIG_NAME = "arm_security_domain_password";
        private const int DOMAIN_DEFAULT_PORT = 389;

        private readonly string _domainPath;
        private readonly int _domainPort;
        private readonly DirectoryEntry _domain;
        private readonly string _commonGroupDistinguishedName;

        #endregion

        #region Поиск объектов в Active Directory

        private IEnumerable<SearchResult> FindAllUsers()
        {
            return FindGroupMembers(FindCommonGroup());
        }

        private IEnumerable<SearchResult> FindAllGroups()
        {
            return FindAll(GetAllGroupsFilter());
        }

        private SearchResult FindGroupByDisplayName(string displayName)
        {
            return FindOne(GetGroupByDisplayNameFilter(displayName));
        }

        private SearchResult FindGroupByDistinguishedName(string distinguishedName)
        {
            return FindOne(GetGroupByDnFilter(distinguishedName));
        }

        private SearchResult FindUserByLogin(string login)
        {
            return FindOne(GetUserByLoginFilter(login));
        }

        private SearchResult FindUserByDistinguishedName(string distinguishedName)
        {
            return FindOne(GetUserByDnFilter(distinguishedName));
        }

        private IEnumerable<SearchResult> FindGroupMembers(SearchResult group)
        {
            return group.Members().Select(FindUserByDistinguishedName).Where(u => u != null);
        }

        private IEnumerable<SearchResult> FindGroupsByMember(SearchResult user)
        {
            return user.Membership().Select(FindGroupByDistinguishedName).Where(g => g != null);
        }

        private IEnumerable<SearchResult> FindUserWithGroups(string login)
        {
            var user = FindUserByLogin(login);
            return user != null
                       ? new[] { user }.Concat(FindGroupsByMember(user))
                       : Enumerable.Empty<SearchResult>();
        }

        private SearchResult FindCommonGroup()
        {
            var group = FindOne(GetCommonGroupFilter());
            if (group == null)
                throw new AdminSecurityGeneralException("общая группа пользователей не найдена");

            return group;
        }

        private SearchResult FindUsersContainer()
        {
            var container = FindOne(GetUsersContainerFilter());
            if (container == null)
                throw new AdminSecurityGeneralException("контейнер с пользователями не найден");

            return container;
        }

        private SearchResult FindOne(string filter)
        {
            try
            {
                var searcher = new DirectorySearcher
                {
                    SearchRoot = _domain,
                    SearchScope = SearchScope.Subtree,
                    Filter = filter
                };

                searcher.PropertiesToLoad.AddRange(AttributesNames);

                return searcher.FindOne();
            }
            catch (Exception e)
            {
                throw new AdminSecurityDirectoryServiceException("Поиск объекта по фильтру", e);
            }
        }

        private IEnumerable<SearchResult> FindAll(string filter)
        {
            try
            {
                var searcher = new DirectorySearcher
                {
                    SearchRoot = _domain,
                    SearchScope = SearchScope.Subtree,
                    Filter = filter
                };

                searcher.PropertiesToLoad.AddRange(AttributesNames);

                return searcher.FindAll().Cast<SearchResult>();
            }
            catch (Exception e)
            {
                throw new AdminSecurityDirectoryServiceException("Поиск объектов по фильтру", e);
            }
        }

        private const string DOMAIN_COMMON_GROUP_NAME = "AllVtbUsers";
        private const string DOMAIN_GROUP_PREFIX = "vtb";

        private const string FILTER_FOR_USERS_CONTAINER = "(cn=Users)";
        private const string FILTER_FOR_ALL_GROUPS = "(&(objectclass=group)(samaccountname=" + DOMAIN_GROUP_PREFIX + "*))";
        private const string FILTER_FOR_COMMON_GROUP = "(&(objectclass=group)(samaccountname=" + DOMAIN_COMMON_GROUP_NAME + "))";
        private const string FILTER_FOR_USER_BY_LOGIN = "(&(objectclass=user)(memberof={0})(samaccountname={1}))";
        private const string FILTER_FOR_USER_BY_DN = "(&(objectclass=user)(memberof={0})(distinguishedname={1}))";
        private const string FILTER_FOR_GROUP_BY_DISPLAY_NAME = "(&(objectclass=group)(samaccountname=" + DOMAIN_GROUP_PREFIX + "{0}))";
        private const string FILTER_FOR_GROUP_BY_DN = "(&(objectclass=group)(distinguishedname={0})(samaccountname=" + DOMAIN_GROUP_PREFIX + "*))";

        private static string GetGroupDisplayName(SearchResult group)
        {
            return Regex.Replace(group.AccountName(), "^" + DOMAIN_GROUP_PREFIX, "");
        }

        private static string GetUsersContainerFilter()
        {
            return FILTER_FOR_USERS_CONTAINER;
        }

        private static string GetAllGroupsFilter()
        {
            return FILTER_FOR_ALL_GROUPS;
        }

        private static string GetCommonGroupFilter()
        {
            return FILTER_FOR_COMMON_GROUP;
        }

        private string GetUserByLoginFilter(string login)
        {
            return string.Format(FILTER_FOR_USER_BY_LOGIN, _commonGroupDistinguishedName, login);
        }

        private static string GetGroupByDisplayNameFilter(string name)
        {
            return string.Format(FILTER_FOR_GROUP_BY_DISPLAY_NAME, name);
        }

        private string GetUserByDnFilter(string dn)
        {
            return string.Format(FILTER_FOR_USER_BY_DN, _commonGroupDistinguishedName, dn);
        }

        private static string GetGroupByDnFilter(string dn)
        {
            return string.Format(FILTER_FOR_GROUP_BY_DN, dn);
        }

        #endregion

        #region Управление учётными записями

        private void AssertManagePermissions()
        {
            CurrentPermissions.AssertAllGranted(PermissionKeys.AdminSecurity_All);
        }

        private void AssertGroupDoesNotExists(string displayName)
        {
            if (FindGroupByDisplayName(displayName) != null)
                throw new AdminSecurityGroupExistsException(displayName);
        }

        private void AssertUserDoesNotExists(string login)
        {
            if (FindUserByLogin(login) != null)
                throw new AdminSecurityUserExistsException(login);
        }

        private void AssertSelfEditIsValid(AdminUser user)
        {
            if (user.Login != CurrentUser)
                return;

            if (user.Permissions.IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            var groups = user.Groups.Select(FindGroupByDisplayName).Where(g => g != null);
            if (GetEffectivePermissions(groups).IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            throw new AdminSecuritySelfEditException();
        }

        private void AssertSelfDeleteAttempt(string login)
        {
            if (login != CurrentUser)
                return;

            throw new AdminSecuritySelfDeleteException();
        }

        private void AssertGroupEditOfSelf(AdminGroup group)
        {
            var user = FindUserByLogin(CurrentUser);
            var groups = FindGroupsByMember(user).ToArray();

            // если пользователь не входит в данную группу, то ок
            if (!groups.Select(GetGroupDisplayName).Contains(group.Name))
                return;

            // если пользователь не удален из данной группы и группа дает доступ к безопасности, то ок
            if (group.Users.Contains(CurrentUser) && group.Permissions.IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            // если пользователю напрямую дан доступ к безопасности, то ок
            if (GetUserPermissions(user).IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            // если пользователю дан доступ к безопасности через другую группу, то ок
            var otherGroups = groups.Where(g => GetGroupDisplayName(g) != group.Name);
            if (GetEffectivePermissions(otherGroups).IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            throw new AdminSecuritySelfEditException();
        }

        private void AssertGroupDeleteOfSelf(string displayName)
        {
            var user = FindUserByLogin(CurrentUser);
            var groups = FindGroupsByMember(user).ToArray();

            // если пользователь не входит в данную группу, то ок
            if (!groups.Select(GetGroupDisplayName).Contains(displayName))
                return;

            // если пользователю напрямую дан доступ к безопасности, то ок
            if (GetUserPermissions(user).IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            // если пользователю дан доступ к безопасности через другую группу, то ок
            var otherGroups = groups.Where(g => GetGroupDisplayName(g) != displayName);
            if (GetEffectivePermissions(otherGroups).IsGranted(PermissionKeys.AdminSecurity_All))
                return;

            throw new AdminSecuritySelfEditException();
        }

        private DirectoryEntry GetUsersEntry()
        {
            return FindUsersContainer().GetDirectoryEntry();
        }

        private DirectoryEntry CreateGroupEntry(string displayName)
        {
            var usersEntry = GetUsersEntry();
            var name = DOMAIN_GROUP_PREFIX + displayName;

            try
            {
                var groupEntry = usersEntry.Children.Add("CN=" + name, "group");
                groupEntry.Properties["samaccountname"].Add(name);
                return groupEntry;
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Создание группы", e);
            }
        }

        private DirectoryEntry CreateUserEntry(string login)
        {
            var usersEntry = GetUsersEntry();

            try
            {
                var userEntry = usersEntry.Children.Add("CN=" + login, "user");
                userEntry.Properties["samaccountname"].Add(login);
                return userEntry;
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Создание пользователя", e);
            }
        }

        private DirectoryEntry GetGroupEntry(string displayName)
        {
            var group = FindGroupByDisplayName(displayName);
            if (group == null)
                throw new AdminSecurityGroupNotFoundException(displayName);

            return group.GetDirectoryEntry();
        }

        private DirectoryEntry GetUserEntry(string login)
        {
            var user = FindUserByLogin(login);
            if (user == null)
                throw new AdminSecurityUserNotFoundException(login);

            return user.GetDirectoryEntry();
        }

        private void SetGroupMembers(DirectoryEntry group, IEnumerable<string> userLogins)
        {
            if (group.Properties.Contains("member"))
                group.Properties["member"].Clear();

            foreach (var user in userLogins.Select(FindUserByLogin).Where(u => u != null))
            {
                group.Properties["member"].Add(user.DistinguishedName());
            }
        }

        private void UpdateUserMembership(DirectoryEntry userEntry, IEnumerable<string> groupDisplayNames)
        {
            var userDistinguishedName = userEntry.DistinguishedName();

            // находим все группы, в которые сейчас входит пользователь
            var currentGroupsDns = userEntry.Membership();
            var currentGroups = currentGroupsDns.Select(FindGroupByDistinguishedName).Where(g => g != null).ToArray();

            // находим все группы, в которые пользователь должен входить,
            var groups = groupDisplayNames.Select(FindGroupByDisplayName).Where(g => g != null).ToArray();
            var groupsDns = groups.Select(g => g.DistinguishedName()).ToArray();

            // включаем пользователя в группы
            foreach (var group in groups.Where(g => !currentGroupsDns.Contains(g.DistinguishedName())))
            {
                AddGroupMember(group.GetDirectoryEntry(), userDistinguishedName);
            }

            // исключаем пользователя из групп
            foreach (var group in currentGroups.Where(g => !groupsDns.Contains(g.DistinguishedName())))
            {
                RemoveGroupMember(group.GetDirectoryEntry(), userDistinguishedName);
            }
        }

        private static void AddGroupMember(DirectoryEntry group, object memberDistinguishedName)
        {
            try
            {
                group.Properties["member"].Add(memberDistinguishedName);
                group.CommitChanges();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Добавление пользователя в группу", e);
            }
        }

        private static void RemoveGroupMember(DirectoryEntry group, object memberDistinguishedName)
        {
            try
            {
                group.Properties["member"].Remove(memberDistinguishedName);
                group.CommitChanges();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Удаление пользователя из группы", e);
            }
        }

        private void SetUserPassword(DirectoryEntry userEntry, string password)
        {
            const long ADS_OPTION_PASSWORD_PORTNUMBER = 6;
            const long ADS_OPTION_PASSWORD_METHOD = 7;
            const int ADS_PASSWORD_ENCODE_CLEAR = 1;

            try
            {
                userEntry.Invoke("SetOption", ADS_OPTION_PASSWORD_PORTNUMBER, _domainPort);
                userEntry.Invoke("SetOption", ADS_OPTION_PASSWORD_METHOD, ADS_PASSWORD_ENCODE_CLEAR);
                userEntry.Invoke("SetPassword", password);
                userEntry.CommitChanges();
            }
            catch (Exception e)
            {
                throw new AdminSecuritySetPasswordException(e);
            }
        }

        private static void ActivateUser(DirectoryEntry userEntry)
        {
            const int ADS_UF_ACCOUNTDISABLE = 0x0002;
            const int ADS_UF_DONT_EXPIRE_PASSWORD = 0x10000;

            try
            {
                var uacValue = (int) userEntry.Properties["useraccountcontrol"].Value;
                uacValue |= ADS_UF_DONT_EXPIRE_PASSWORD;
                uacValue &= ~ADS_UF_ACCOUNTDISABLE;
                userEntry.Properties["useraccountcontrol"].Value = uacValue;
                userEntry.CommitChanges();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Активация пользователя", e);
            }
        }

        private void DeleteEntry(DirectoryEntry entry)
        {
            try
            {
                var usersEntry = GetUsersEntry();
                usersEntry.Children.Remove(entry);
                usersEntry.CommitChanges();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Удаление объекта AD", e);
            }
        }

        private void DeleteEntryNoThrow(DirectoryEntry entry)
        {
            try
            {
                var usersEntry = GetUsersEntry();
                usersEntry.Children.Remove(entry);
                usersEntry.CommitChanges();
            }
            catch (DirectoryServicesCOMException)
            {
                //TODO: залогировать
            }
        }

        private static void CommitEntry(DirectoryEntry entry)
        {
            try
            {
                entry.CommitChanges();
            }
            catch (DirectoryServicesCOMException e)
            {
                throw new AdminSecurityDirectoryServiceException("Сохранение объекта AD", e);
            }
        }

        private AdminGroup ToAdminGroup(SearchResult group, IEnumerable<string> allUserLogins)
        {
            var loginsList = allUserLogins.ToArray();

            return new AdminGroup
            {
                Name = GetGroupDisplayName(group),
                Users = FindGroupMembers(group).Select(s => s.AccountName())
                                               .Where(loginsList.Contains)
                                               .ToArray(),
                Permissions = GetGroupPermissions(group)
            };
        }

        private AdminUser ToAdminUser(SearchResult user)
        {
            var groups = FindGroupsByMember(user).ToArray();

            return new AdminUser
            {
                Login = user.AccountName(),
                WhenCreated = user.WhenCreated(),
                Groups = groups.Select(GetGroupDisplayName).ToArray(),
                Permissions = GetUserPermissions(user),
                InheritedPermissions = groups.Select(GetGroupPermissions).Cast<IPermissionsSource>().ToArray()
            };
        }

        private IEnumerable<string> GetAllGroupDisplayNames()
        {
            return FindAllGroups().Select(GetGroupDisplayName);
        }

        private IEnumerable<string> GetAllUserLogins()
        {
            return FindAllUsers().Select(u => u.AccountName());
        }

        #endregion

        #region Проверка учётных данных пользователей

        private bool ValidateUser(string login, string password)
        {
            if (string.IsNullOrWhiteSpace(_domainPath))
                return false;

            using (var entry = new DirectoryEntry(_domainPath + ":" + _domainPort, login, password))
            {
                try
                {
                    return entry.NativeObject != null;
                }
                catch (DirectoryServicesCOMException)
                {
                    return false;
                }
            }
        }

        private static readonly PermissionKeys[] LoginPermissions = new[]
        {
            PermissionKeys.Catalog_Login,
            PermissionKeys.Actions_Login,
            PermissionKeys.Site_Login,
            PermissionKeys.Security_Login,
            PermissionKeys.AdminSecurity_All
        };

        private bool VerifyUserLoginPermissions(string login)
        {
            var permissions = GetEffectiveUserPermissionsByLogin(login);
            return permissions.IsAnyGranted(LoginPermissions);
        }

        #endregion

        #region Чтение/запись прав

        private static AdPermissionsSource GetGroupPermissions(SearchResult group)
        {
            return new AdPermissionsSource(GetGroupDisplayName(group), group, AttributeNamesByKey);
        }

        private static AdPermissionsSource GetUserPermissions(SearchResult user)
        {
            return new AdPermissionsSource(user.AccountName(), user, AttributeNamesByKey);
        }

        private EffectivePermissionsSource GetEffectiveUserPermissionsByLogin(string login)
        {
            var sources = FindUserWithGroups(login).Select(obj => new AdPermissionsSource(obj, AttributeNamesByKey))
                                                   .Cast<IPermissionsSource>()
                                                   .ToArray();

            return new EffectivePermissionsSource(login, sources);
        }

        private static EffectivePermissionsSource GetEffectivePermissions(IEnumerable<SearchResult> objects)
        {
            var sources = objects.Select(obj => new AdPermissionsSource(obj, AttributeNamesByKey))
                                 .Cast<IPermissionsSource>()
                                 .ToArray();
            return new EffectivePermissionsSource(sources);
        }

        private static void WritePermissions(DirectoryEntry entry, IPermissionsSource source)
        {
            foreach (var attrName in AttributeNamesByKey.Values.Where(n => entry.Properties.Contains(n)))
            {
                entry.Properties[attrName].Clear();
            }
            foreach (var item in source.Enumerate())
            {
                var value = item.Value.Length > 0
                                ? string.Join(",", item.Value)
                                : " ";
                entry.Properties[AttributeNamesByKey[item.Key]].Value = value;
            }
        }

        #endregion

        #region Чтение конфигурации

        private static readonly string[] AttributesNames;

        private static readonly Dictionary<PermissionKeys, string> AttributeNamesByKey;

        private static readonly PermissionNode[] Nodes;

        private static readonly IPermissionsSource EmptyPermissions;

        static AdminSecurityService()
        {
            var section = (AdminSecurityConfigSection) ConfigurationManager.GetSection("admin_security");

            AttributeNamesByKey = section.PermissionsCollection
                                         .Cast<PermissionElement>()
                                         .ToDictionary(element => element.Key, element => element.AdAttributeName);

            AttributesNames = AttributeNamesByKey.Values
                                                 .Concat(new[]
                                                 {
                                                     "samaccountname",
                                                     "member",
                                                     "memberof",
                                                     "distinguishedname",
                                                     "whencreated"
                                                 })
                                                 .ToArray();

            Nodes = ReadPermissionNodes(section.NodesCollection).ToArray();

            EmptyPermissions = new DictionaryPermissionsSource(new Dictionary<PermissionKeys, string[]>());
        }

        private static IEnumerable<PermissionNode> ReadPermissionNodes(IEnumerable collection)
        {
            return collection.Cast<PermissionNodeElement>()
                             .Select(element => new PermissionNode
                             {
                                 Description = element.Description,
                                 Type = element.Type,
                                 ListDescription = element.ListDescription,
                                 ListWildcardDescription = element.ListWildcardDescription,
                                 TargetKey = element.TargetKey,
                                 AdditionalValues = ReadAdditionalPermissionValues(element.AdditionalItemsCollection),
                                 Children = ReadPermissionNodes(element.ChildrenCollection).ToArray()
                             });
        }

        private static Dictionary<PermissionKeys, string[]> ReadAdditionalPermissionValues(IEnumerable collection)
        {
            return collection.Cast<PermissionItemElement>()
                             .Select(item => new KeyValuePair<PermissionKeys, string[]>(
                                                 item.Key,
                                                 item.ValuesCollection
                                                     .Cast<PermissionValueElement>()
                                                     .Select(v => v.Value)
                                                     .ToArray()))
                             .ToDictionary(item => item.Key, item => item.Value);
        }

        #endregion
    }
}
