using System.ComponentModel;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public enum FeedbackTypeModel
    {
        [Description("Предложение")]
        Suggestion,

        [Description("Претензия")]
        Issue,

        [Description("Вопрос по оформленному заказу")]
        OrderIncident
    }
}