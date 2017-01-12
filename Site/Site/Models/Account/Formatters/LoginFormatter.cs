using System.Text.RegularExpressions;

namespace Vtb24.Site.Models.Account.Formatters
{
    public static class LoginFormatter
    {
         public static string FormatPhone(string phone)
         {
             return Regex.Replace(phone ?? "", @"[^\d]", "");
         }
    }
}