using Vtb24.Site.Security.Models;

namespace Vtb24.Site.Security.SecurityService.Models.Outputs
{
    public class PhoneNumberChangeHistory
    {
        public User User { get; set; }

        public PhoneNumberChange[] History { get; set; }

        public int TotalCount { get; set; }

        public SecurityPagingSettings SecurityPagingSettings { get; set; }
    }
}