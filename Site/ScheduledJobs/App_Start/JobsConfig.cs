using System;
using System.Collections.Generic;
using Topshelf.Quartz;

namespace ScheduledJobs.App_Start
{
    public struct JobsConfig
    {
        public static IEnumerable<Action<QuartzConfigurator>> ListJobs()
        {
            // обработка ответов операторов ДКО на обращения по обратной связи
            yield return FeedbackByEmail.Jobs.CheckMailJob.Schedule;
            // рассылка уведомлений о новых сообщениях
            yield return FeedbackByEmail.Jobs.SendFeedbackUpdatesJob.Schedule;
        }  
    }
}