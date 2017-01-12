using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports
{
    public class PixelReportItem
    {
        [Key]
        public Guid Id { get; set; }

        #region детали строки отчета

        public DateTime Timestamp { get; set; }

        public string IpAddress { get; set; }

        public string Agent { get; set; }

        #endregion

        #region ссылки

        public Guid PixelReportId { get; set; }

        [ForeignKey("PixelReportId")]
        [Required]
        public PixelReport Report { get; set; }

        #endregion
    }
}
