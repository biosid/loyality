using Vtb24.Site.Services.Infrastructure;

namespace ScheduledJobs.FeedbackByEmail
{
    static class Settings
    {
        public static int CheckIntervalInMinutes
        {
            get
            {
                return AppSettingsHelper.Int("feedback_by_email_check_job_interval_in_minutes", 1);
            }
        }

        public static int SendIntervalInMinutes
        {
            get
            {
                return AppSettingsHelper.Int("feedback_by_email_send_job_interval_in_minutes", 1);
            }
        }

        public static string MailHost
        {
            get
            {
                return AppSettingsHelper.String("mailbox_host", "loyalty-win0.rapidsoft.local");
            }
        }

        public static int MailPort
        {
            get
            {
                return AppSettingsHelper.Int("mailbox_imap_port", 143);
            }
        }

        public static string MailUser
        {
            get
            {
                return AppSettingsHelper.String("feedback_by_email_mailbox", "reply.feedback@loyalty-win0.rapidsoft.local");
            }
        }

        public static string MailPassword
        {
            get
            {
                return AppSettingsHelper.String("feedback_by_email_mailbox_password", "mail");
            }
        }

        public static string UserId {
            get
            {
                return AppSettingsHelper.String("arm_security_domain_user", "vtbSystemUser");
            } 
        }

        public static string MailTo
        {
            get
            {
                return AppSettingsHelper.String("feedback_by_email_recipients", "sscherbakov@rapidsoft.ru");
            }
        }
    }
}