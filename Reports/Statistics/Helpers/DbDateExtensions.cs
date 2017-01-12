using System;

namespace Rapidsoft.VTB24.Reports.Statistics.Helpers
{
    internal static class DbDateExtensions
    {
        public static DateTime FromDbDate(this DateTime dbDate)
        {
            return DateTime.SpecifyKind(dbDate, DateTimeKind.Utc);
        }

        public static DateTime? FromDbDate(this DateTime? dbDate)
        {
            return dbDate.HasValue
                       ? DateTime.SpecifyKind(dbDate.Value, DateTimeKind.Utc)
                       : (DateTime?)null;
        }
    }
}
