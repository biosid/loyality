using System;
using LINQtoCSV;

namespace Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports
{
    public class PixelReportCsvItem
    {
        [CsvColumn(FieldIndex = 1, Name = "Дата/время", OutputFormat = "dd.MM.yyyy HH:mm:ss")]
        public DateTime Timestamp { get; set; }

        [CsvColumn(FieldIndex = 2, Name = "IP-адрес")]
        public string IpAddress { get; set; }

        [CsvColumn(FieldIndex = 3, Name = "Агент")]
        public string Agent { get; set; }
    }
}
