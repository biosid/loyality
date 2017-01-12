using System;

namespace Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models
{
    public class EtlFile
    {
        public const long SIZE_LIMIT = 4 * 1024 * 1024; // 4MB

        public string Name { get; set; }

        public DateTime Timestamp { get; set; }

        public long? RowCount { get; set; }

        public long? Size { get; set; }

        public bool SizeExceeded
        {
            get { return Size.HasValue && Size.Value > SIZE_LIMIT; }
        }
    }
}
