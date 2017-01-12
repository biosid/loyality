namespace Vtb24.Arms.Actions.Models
{
    public class ActionHistoryModel
    {
        // ReSharper disable InconsistentNaming

        public string query { get; set; }

        public int? page { get; set; }

        // ReSharper restore InconsistentNaming

        public long Id { get; set; }

        public string Name { get; set; }

        public HistoryRecordModel[] HistoryRecords { get; set; }

        public int TotalPages { get; set; }
    }
}
