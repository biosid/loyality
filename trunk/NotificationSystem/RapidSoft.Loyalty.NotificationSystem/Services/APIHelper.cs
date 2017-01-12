using System;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public class ApiHelper
    {
        public static ThreadTypes ConvertToThreadType(FeedbackTypes type)
        {
            switch (type)
            {
                case FeedbackTypes.Issue:
                    return ThreadTypes.Issue;
                case FeedbackTypes.Suggestion:
                    return ThreadTypes.Suggestion;
                case FeedbackTypes.OrderIncident:
                    return ThreadTypes.OrderIncident;
            }

            throw new InvalidOperationException(string.Format("FeedbackType {0} unsupported", type));
        }

        public static FeedbackTypes ConvertToFeedbackType(ThreadTypes type)
        {
            switch (type)
            {
                case ThreadTypes.Issue:
                    return FeedbackTypes.Issue;
                case ThreadTypes.Suggestion:
                    return FeedbackTypes.Suggestion;
                case ThreadTypes.OrderIncident:
                    return FeedbackTypes.OrderIncident;
            }

            throw new InvalidOperationException(string.Format("ThreadType {0} unsupported", type));
        }
    }
}