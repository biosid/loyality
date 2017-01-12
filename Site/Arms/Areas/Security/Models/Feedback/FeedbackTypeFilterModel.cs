using System.ComponentModel;

namespace Vtb24.Arms.Security.Models.Feedback
{
    public enum FeedbackTypeFilterModel
    {
        [Description("Тип обращения: все")]
        // ReSharper disable once InconsistentNaming
        all = 0,

        [Description("Только предложения")]
        // ReSharper disable once InconsistentNaming
        suggestion = 1,

        [Description("Только претензии")]
        // ReSharper disable once InconsistentNaming
        issue = 2,

        [Description("Только вопросы по заказам")]
        // ReSharper disable once InconsistentNaming
        orderincident = 3
    }
}