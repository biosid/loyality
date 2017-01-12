using System.ComponentModel;

namespace Rapidsoft.VTB24.Reports.Statistics.Models.ProductViewEvents
{
    public enum ViewEventsDayStatus
    {
        [Description("Не готов")]
        New = 1,

        [Description("Создаётся")]
        InProgress = 2,

        [Description("Готов")]
        Ready = 3,

        [Description("Ошибка при создании")]
        Error = 4
    }
}
