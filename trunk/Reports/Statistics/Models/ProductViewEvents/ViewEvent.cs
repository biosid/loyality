using System;
using LINQtoCSV;

namespace Rapidsoft.VTB24.Reports.Statistics.Models.ProductViewEvents
{
    public class ViewEvent
    {
        [CsvColumn(FieldIndex = 1)]
        public DateTime Timestamp { get; set; }

        [CsvColumn(FieldIndex = 2)]
        public string ProductId { get; set; }

        [CsvColumn(FieldIndex = 3)]
        public string Login { get; set; }

        [CsvColumn(FieldIndex = 4)]
        public string Session { get; set; }
    }
}
