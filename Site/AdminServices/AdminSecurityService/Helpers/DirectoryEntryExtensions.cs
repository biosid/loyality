using System.DirectoryServices;
using System.Linq;

namespace Vtb24.Arms.AdminServices.AdminSecurityService.Helpers
{
    internal static class DirectoryEntryExtensions
    {
        public static string DistinguishedName(this DirectoryEntry entry)
        {
            return (string) entry.Properties["distinguishedname"].Value;
        }

        public static string[] Membership(this DirectoryEntry entry)
        {
            return entry.Properties["memberof"].Cast<string>().ToArray();
        }
    }
}
