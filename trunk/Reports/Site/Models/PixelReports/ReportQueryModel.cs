using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Rapidsoft.VTB24.Reports.Site.Models.Shared;

namespace Rapidsoft.VTB24.Reports.Site.Models.PixelReports
{
    public class ReportQueryModel
    {
        [Required(ErrorMessage = "укажите дату")]
        [Display(Name = "Начальная дата")]
        public DateModel FromDate { get; set; }

        [Required(ErrorMessage = "укажите дату")]
        [Display(Name = "Конечная дата")]
        public DateModel ToDate { get; set; }

        [Required(ErrorMessage = "укажите пиксель")]
        [Display(Name = "Пиксель")]
        public string Pixel { get; set; }

        public SelectListItem[] Pixels { get; set; }
    }
}
