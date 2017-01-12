using System.ComponentModel.DataAnnotations;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;
using Rapidsoft.VTB24.Reports.Site.Models.Shared;

namespace Rapidsoft.VTB24.Reports.Site.Models.InteractionReports
{
    public class ReportQueryModel
    {
        [Required(ErrorMessage = "укажите дату")]
        [Display(Name = "Начальная дата")]
        public DateModel FromDate { get; set; }

        [Required(ErrorMessage = "укажите дату")]
        [Display(Name = "Конечная дата")]
        public DateModel ToDate { get; set; }

        [Required(ErrorMessage = "укажите тип")]
        [Display(Name = "Тип взаимодействия")]
        public InteractionType Type { get; set; }

        public bool Print { get; set; }
    }
}