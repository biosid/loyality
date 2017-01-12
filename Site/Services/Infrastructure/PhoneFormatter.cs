using System.Text.RegularExpressions;

namespace Vtb24.Site.Services.Infrastructure
{
    public static class PhoneFormatter
    {
        public static string FormatPhoneNumber(string strippedPhoneNumber)
        {
            if (string.IsNullOrWhiteSpace(strippedPhoneNumber) || strippedPhoneNumber.Length != 11)
            {
                return strippedPhoneNumber;
            }

            return string.Format(
                "+{0} ({1}) {2}-{3}",   // 7XXXYYYZZZZ
                strippedPhoneNumber.Substring(0, 1),  // 7
                strippedPhoneNumber.Substring(1, 3),  // XXX
                strippedPhoneNumber.Substring(4, 3),  // YYY
                strippedPhoneNumber.Substring(7, 4)    // ZZZZ
            );
        }

        public static string StripPhoneNumber(string formattedPhoneNumber)
        {
            return Regex.Replace(formattedPhoneNumber ?? "", @"[^\d]", "");
        }

    }
}