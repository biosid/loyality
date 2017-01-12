using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Rapidsoft.VTB24.Reports.Statistics.Models.PixelReports;

namespace Rapidsoft.VTB24.Reports.Statistics.Entities.PixelReports
{
    public class PixelReport
    {
        [Key]
        public Guid Id { get; set; }

        #region детали запроса

        [Required]
        [StringLength(1024)]
        public string Pixel { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        #endregion

        #region детали выполнения

        public DateTime CreateTimestamp { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? FinishTimestamp { get; set; }

        public ReportStatus Status { get; set; }

        #endregion

        #region ссылки

        public ICollection<PixelReportItem> Items { get; set; }

        #endregion
    }
}
