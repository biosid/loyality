using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.ArmSecurity.Tests
{
    using System.Collections;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;

    [TestClass]
    public class ActiveDirectoryProviderTests
    {
        private readonly ActiveDirectoryProvider activeDirectoryProvider = new ActiveDirectoryProvider();

        [TestMethod]
        public void ShouldGetUsers()
        {
            var userResult1 = this.activeDirectoryProvider.SearchUser("vtbSystemUser");

            this.WriteProp(userResult1);

            var userResult2 = this.activeDirectoryProvider.SearchUser("vtbAdmin");

            this.WriteProp(userResult2);

            var userResult3 = this.activeDirectoryProvider.SearchUser("vtbTest");

            this.WriteProp(userResult3);
        }

        [TestMethod]
        public void ShouldGetForGroups()
        {
            var groupResults = this.activeDirectoryProvider.SearchAllUserGroups();

            this.WriteProp(groupResults);
        }

        private void WriteProp(IEnumerable<SearchResult> searchResults)
        {
            foreach (var searchResult in searchResults)
            {
                this.WriteProp(searchResult);
                Console.WriteLine("=======");
            }
        }

        private void WriteProp(SearchResult searchResult)
        {
            foreach (var item in searchResult.Properties.Cast<DictionaryEntry>())
            {
                Console.WriteLine(item.Key);
                foreach (var obj in (item.Value as ResultPropertyValueCollection))
                {
                    Console.Write("\t");
                    Console.WriteLine(obj == null ? "null" : obj.ToString());
                }
            }
        }
    }
}
