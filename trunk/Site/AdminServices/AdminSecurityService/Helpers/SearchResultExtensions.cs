using System;
using System.DirectoryServices;
using System.Linq;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Helpers
{
    internal static class SearchResultExtensions
    {
        public static string AccountName(this SearchResult searchResult)
        {
            return searchResult.Properties["samaccountname"].Cast<string>().First();
        }

        public static string DistinguishedName(this SearchResult searchResult)
        {
            return searchResult.Properties["distinguishedname"].Cast<string>().First();
        }

        public static DateTime WhenCreated(this SearchResult searchResult)
        {
            return searchResult.Properties["whencreated"].Cast<DateTime>().First();
        }

        public static string[] Members(this SearchResult searchResult)
        {
            return searchResult.Properties["member"].Cast<string>().ToArray();
        }

        public static string[] Membership(this SearchResult entry)
        {
            return entry.Properties["memberof"].Cast<string>().ToArray();
        }
    }
}
