using System;

namespace Vtb24.Arms.Security.Models.Users
{
    public class UserPointsQueryModel
    {
        // ReSharper disable InconsistentNaming

        public string login { get; set; }

        public DateTime? from { get; set; }

        public DateTime? to { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming
    }
}
