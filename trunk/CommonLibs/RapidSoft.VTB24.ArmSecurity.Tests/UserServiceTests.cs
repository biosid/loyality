using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.ArmSecurity.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.DirectoryServices;
    using System.Linq;
    using System.Reflection;

    using Moq;

    using RapidSoft.VTB24.ArmSecurity.Entities;
    using RapidSoft.VTB24.ArmSecurity.Interfaces;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed. Suppression is OK here.")]
    [TestClass]
    public class UserServiceTests
    {
        internal const string VtbPermissionPrefix = "vtb";

        private const string TestUser = "Test";

        private const string PermView = "View";

        private const string PermEdit = "Edit";

        private const string PermDel = "Delete";

        private const string Space = " ";

        private const string Group1 = "Group1";
        private const string Group2 = "Group2";
        private const string Group3 = "Group3";

        [TestMethod]
        public void ShouldHasPermissionByUser()
        {
            var mock = new Mock<IActiveDirectoryProvider>();

            var dic = new Dictionary<string, object[]>
                          {
                              { VtbPermissionPrefix + PermView, new object[] { Space } },
                              { VtbPermissionPrefix + PermEdit, new object[] { Space } },
                              {
                                  SearchResultExtensions.AccountNameAttrName,
                                  new object[] { TestUser }
                              }
                          };

            var searchResult = this.BuildSearchResult(dic);

            mock.Setup(x => x.SearchUser(TestUser)).Returns(searchResult);
            var userService = new UserService(mock.Object);
            var user = userService.GetUserPrincipalByName(TestUser);

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Identity);
            Assert.AreEqual(TestUser, user.Identity.Name);

            Assert.IsTrue(user.HasPermission(PermView));
            Assert.IsTrue(user.HasPermission(PermEdit));
            Assert.IsFalse(user.HasPermission(PermDel));
        }

        [TestMethod]
        public void ShouldHasPermissionByUserGroup()
        {
            var mock = new Mock<IActiveDirectoryProvider>();

            var dicUser = new Dictionary<string, object[]>
                              {
                                  {
                                      SearchResultExtensions.AccountNameAttrName,
                                      new object[] { TestUser }
                                  },
                                  {
                                      SearchResultExtensions.MemberOfAttrName,
                                      new object[] { Group1, Group2 }
                                  }
                              };
            var userSearchResult = this.BuildSearchResult(dicUser);

            var group1SearchResult = this.BuildSearchResultForGroup(Group1, (string)null, VtbPermissionPrefix + PermView);
            var group2SearchResult = this.BuildSearchResultForGroup(Group2, (string)null, VtbPermissionPrefix + PermEdit);
            var group3SearchResult = this.BuildSearchResultForGroup(Group3, (string)null, VtbPermissionPrefix + PermDel);

            mock.Setup(x => x.SearchUser(TestUser)).Returns(userSearchResult);
            mock.Setup(x => x.SearchAllUserGroups())
                .Returns(new[] { group1SearchResult, group2SearchResult, group3SearchResult });

            var userService = new UserService(mock.Object);
            var user = userService.GetUserPrincipalByName(TestUser);

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Identity);
            Assert.AreEqual(TestUser, user.Identity.Name);

            Assert.IsTrue(user.HasPermission(PermView));
            Assert.IsTrue(user.HasPermission(PermEdit));
            Assert.IsFalse(user.HasPermission(PermDel));

            Assert.IsTrue(user.HasPermissions(new List<string> { PermView, PermEdit }));
            Assert.IsFalse(user.HasPermissions(new List<string> { PermView, PermEdit, PermDel }));
        }

        [TestMethod]
        public void ShouldHasPermissionForPartnerId()
        {
            var mock = new Mock<IActiveDirectoryProvider>();

            var dicUser = new Dictionary<string, object[]>
                              {
                                  {
                                      SearchResultExtensions.AccountNameAttrName,
                                      new object[] { TestUser }
                                  },
                                  { VtbPermissionPrefix + PermView, new object[] { Space } },
                                  {
                                      Vtb24Principal.SearchResultWrapper.PartnerIdsAttrName,
                                      new object[] { "1" }
                                  },
                                  {
                                      SearchResultExtensions.MemberOfAttrName,
                                      new object[] { Group1, Group3 }
                                  }
                              };
            var userSearchResult = this.BuildSearchResult(dicUser);

            var group1SearchResult = this.BuildSearchResultForGroup(Group1, "2", VtbPermissionPrefix + PermView);
            var group3SearchResult = this.BuildSearchResultForGroup(Group3, "*", VtbPermissionPrefix + PermDel);

            mock.Setup(x => x.SearchUser(TestUser)).Returns(userSearchResult);
            mock.Setup(x => x.SearchAllUserGroups()).Returns(new[] { group1SearchResult, group3SearchResult });

            var userService = new UserService(mock.Object);
            var user = userService.GetUserPrincipalByName(TestUser);

            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Identity);
            Assert.AreEqual(TestUser, user.Identity.Name);

            Assert.IsTrue(user.HasPermission(PermView));
            Assert.IsFalse(user.HasPermission(PermEdit));
            Assert.IsTrue(user.HasPermission(PermDel));
            Assert.IsTrue(user.HasPermissionForPartner(PermView, "1"));
            Assert.IsTrue(user.HasPermissionForPartner(PermView, "2"));
            Assert.IsTrue(user.HasPermissionForPartner(PermView, "3"), "Так как права объединяются, а в группе 3 для списка ид партнеров \"*\", это дает право на любого партнера");
            Assert.IsTrue(user.HasPermissionForPartner(PermDel, "1"));
            Assert.IsTrue(user.HasPermissionForPartner(PermDel, "5"));

            Assert.IsTrue(user.HasPermissionsForPartner(new List<string> { PermView, PermDel }, "1"));
            Assert.IsTrue(user.HasPermissionsForPartner(new List<string> { PermDel }, "5"));
            Assert.IsFalse(user.HasPermissionsForPartner(new List<string> { PermEdit }, "1"));
        }

        private SearchResult BuildSearchResultForGroup(
            string distinguishedName, string partnerId, params string[] permissions)
        {
            return this.BuildSearchResultForGroup(distinguishedName, new[] { partnerId }, permissions);
        }

        private SearchResult BuildSearchResultForGroup(string distinguishedName, IEnumerable<string> partnerIds, params string[] permissions)
        {
            var converted = permissions.Select(x => new KeyValuePair<string, object[]>(x, new object[] { Space }));

            var list = new List<KeyValuePair<string, object[]>>
                           {
                               new KeyValuePair<string, object[]>(
                                   SearchResultExtensions.DistinguishedNameAttrName,
                                   new object[] { distinguishedName })
                           };

            if (partnerIds != null)
            {
                var str = string.Join(", ", partnerIds);
                list.Add(
                    new KeyValuePair<string, object[]>(
                        Vtb24Principal.SearchResultWrapper.PartnerIdsAttrName, new object[] { str }));
            }

            var dic = converted.Union(list);

            var retVal = this.BuildSearchResult(dic);

            return retVal;
        }

        private SearchResult BuildSearchResult(IEnumerable<KeyValuePair<string, object[]>> resultCollection)
        {
            var type = typeof(SearchResult);

            var retVal = (SearchResult)Activator.CreateInstance(
                type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { null, null }, null);

            var collection = this.BuildResultPropertyCollection(resultCollection);

            type.GetField("properties", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(retVal, collection);

            return retVal;
        }

        private ResultPropertyCollection BuildResultPropertyCollection(IEnumerable<KeyValuePair<string, object[]>> result)
        {
            var inner = result.ToDictionary(
                x => x.Key, y => this.BuildResultPropertyValueCollection(y.Value));

            var type = typeof(ResultPropertyCollection);

            var retVal = (ResultPropertyCollection)Activator.CreateInstance(
                type, BindingFlags.NonPublic | BindingFlags.Instance, null, null, null);

            var methodInfo = type.GetMethod("Add", BindingFlags.Instance | BindingFlags.NonPublic);

            foreach (var resultPropertyValueCollection in inner)
            {
                methodInfo.Invoke(
                    retVal, new object[] { resultPropertyValueCollection.Key, resultPropertyValueCollection.Value });
            }

            return retVal;
        }

        private ResultPropertyValueCollection BuildResultPropertyValueCollection(object[] objects)
        {
            var type = typeof(ResultPropertyValueCollection);
            var retVal = (ResultPropertyValueCollection)Activator.CreateInstance(
                type, BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { objects }, null);

            return retVal;
        }
    }
}
