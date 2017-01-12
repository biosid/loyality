namespace RapidSoft.VTB24.ArmSecurity
{
    using System;
    using System.Collections.Generic;
    using System.DirectoryServices;
    using System.Linq;

    internal static class SearchResultExtensions
    {
        public static readonly string AccountNameAttrName = "sAMAccountName";

        public static readonly string MemberOfAttrName = "memberOf";

        public static readonly string DistinguishedNameAttrName = "distinguishedname";

        public static bool IsHasValue<T>(this SearchResult searchResult, string attributeName, IEnumerable<T> value)
        {
            var lower = attributeName.ToLower();

            if (!searchResult.Properties.Contains(lower))
            {
                return false;
            }

            var resultPropertyValueCollection = searchResult.Properties[lower];

            return resultPropertyValueCollection.Cast<T>().Intersect(value).Any();
        }

        public static IEnumerable<T> GetValues<T>(this SearchResult searchResult, string attributeName)
        {
            var properties = searchResult.Properties;
            var retVal = properties[attributeName.ToLower()].Cast<T>();
            return retVal;
        }
    }
}