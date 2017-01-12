using System.ComponentModel;

namespace Vtb24.Site.Models.Feedback
{
    public enum FeedbackType
    {
        [Description("Предложение")]
        Suggestion,

        [Description("Претензия")]
        Issue,

        [Description("Отключение от программы")]
        Unsubscribe,

        [Description("Вопрос по оформленному заказу")]
        OrderIncident
    }

    internal static class FeedbackTypeExtensions
    {
        public static Services.ClientFeedback.Models.FeedbackType? Map(this FeedbackType original)
        {
            switch (original)
            {
                case FeedbackType.Suggestion:
                    return Services.ClientFeedback.Models.FeedbackType.Suggestion;
                case FeedbackType.Issue:
                    return Services.ClientFeedback.Models.FeedbackType.Issue;
                case FeedbackType.Unsubscribe:
                    return Services.ClientFeedback.Models.FeedbackType.Unsubscribe;
                    case FeedbackType.OrderIncident:
                    return Services.ClientFeedback.Models.FeedbackType.OrderIncident;
            }

            return null;
        }
    }
}
