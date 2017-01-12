using System;
using System.Text.RegularExpressions;
using Vtb24.Site.Security.SecurityService.Models.Outputs;

namespace Vtb24.Arms.Security.Models.Users
{
    public class PhoneNumberHistoryItemModel
    {
        public DateTime ChangeTime { get; set; }

        public string OldPhoneNumber { get; set; }

        public string NewPhoneNumber { get; set; }

        public string ChangedBy { get; set; }

        public static PhoneNumberHistoryItemModel Map(PhoneNumberChange original)
        {
            return new PhoneNumberHistoryItemModel
            {
                ChangeTime = original.ChangeTime,
                OldPhoneNumber = FormatPhone(original.OldPhoneNumber),
                NewPhoneNumber = FormatPhone(original.NewPhoneNumber),
                ChangedBy = original.ChangedBy
            };
        }

        private static string FormatPhone(string phone)
        {
            return string.IsNullOrWhiteSpace(phone)
                       ? string.Empty
                       : Regex.Replace(phone, @"[^\d]", "");
        }
    }
}