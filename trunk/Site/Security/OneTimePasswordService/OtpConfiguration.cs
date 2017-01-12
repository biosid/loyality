using System.Collections.Generic;
using Vtb24.Site.Security.Infrastructure;

namespace Vtb24.Site.Security.OneTimePasswordService
{
    public class OtpConfiguration
    {
        private int? _length;
        private int? _attempts;
        private int? _smsTimeout;
        private int? _emailTimeout;
        private int? _smsRenewAttempts;
        private int? _emailRenewAttempts;
        private string _emailFrom;
        private string _smtpHost;
        private int? _smtpPort;
        private string _smtpUser;
        private string _smtpPassword;
        private Dictionary<string, int> _sendSimilarTimeout;
        private Dictionary<string, int> _sendThreshold;
        private Dictionary<string, int> _sendThresholdInterval;

        public int Length
        {
            get { return _length ?? (_length = AppSettingsHelper.Int("otp_length", 6)).Value; }
            set { _length = value; }
        }

        public int AllowedAttempts
        {
            get { return _attempts ?? (_attempts = AppSettingsHelper.Int("otp_allowed_attempts", 3)).Value; }
            set { _attempts = value; }
        }

        public int SmsTimeoutSeconds
        {
            get { return _smsTimeout ?? (_smsTimeout = AppSettingsHelper.Int("otp_via_sms_timeout_seconds", 300)).Value; }
            set { _smsTimeout = value; }
        }

        public int EmailTimeoutSeconds
        {
            get { return _emailTimeout ?? (_emailTimeout = AppSettingsHelper.Int("otp_via_email_timeout_seconds", 900)).Value; }
            set { _emailTimeout = value; }
        }

        public int SmsAllowedRenewAttempts
        {
            get { return _smsRenewAttempts ?? (_smsRenewAttempts = AppSettingsHelper.Int("otp_via_sms_renew_allowed_attempts", 3)).Value; }
            set { _smsRenewAttempts = value; }
        }

        public int EmailAllowedRenewAttempts
        {
            get { return _emailRenewAttempts ?? (_emailRenewAttempts = AppSettingsHelper.Int("otp_via_email_renew_allowed_attempts", 5)).Value; }
            set { _emailRenewAttempts = value; }
        }

        public string EmailFrom
        {
            get { return _emailFrom ?? (_emailFrom = AppSettingsHelper.String("opt_via_email_from", "noreply@bonus.vtb24.ru")); }
            set { _emailFrom = value; }
        }

        public string SmtpHost
        {
            get { return _smtpHost ?? (_smtpHost = AppSettingsHelper.String("mailbox_host", "localhost")); }
            set { _smtpHost = value; }
        }

        public int SmtpPort
        {
            get { return _smtpPort ?? (_smtpPort = AppSettingsHelper.Int("mailbox_smtp_port", 25)).Value; }
            set { _smtpPort = value; }
        }

        public string SmtpUser
        {
            get { return _smtpUser ?? (_smtpUser = AppSettingsHelper.String("otp_via_email_smtp_user", "loyalty@vtb24.loyalty")); }
            set { _smtpUser = value; }
        }

        public string SmtpPassword
        {
            get { return _smtpPassword ?? (_smtpPassword = AppSettingsHelper.String("otp_via_email_smtp_password", "")); }
            set { _smtpPassword = value; }
        }

        public int GetSimilarTimeoutSeconds(string type)
        {
            if (_sendSimilarTimeout == null)
            {
                _sendSimilarTimeout = new Dictionary<string, int>();
            }

            var defaultValue = AppSettingsHelper.Int("otp_send_similar_timeout_seconds_default", 300);

            if (string.IsNullOrEmpty(type))
            {
                return defaultValue;
            }

            if (!_sendSimilarTimeout.ContainsKey(type))
            {
                _sendSimilarTimeout[type] = AppSettingsHelper.Int(string.Format("otp_send_similar_timeout_seconds_{0}", type), defaultValue);
            }

            return _sendSimilarTimeout[type];
        }

        public int GetSimilarThresholdIntervalInSeconds(string type)
        {
            if (_sendThresholdInterval == null)
            {
                _sendThresholdInterval = new Dictionary<string, int>();
            }

            var defaultValue = AppSettingsHelper.Int("otp_send_similar_threshold_interval_seconds_default", 3600);

            if (string.IsNullOrEmpty(type))
            {
                return defaultValue;
            }

            if (!_sendThresholdInterval.ContainsKey(type))
            {
                _sendThresholdInterval[type] = AppSettingsHelper.Int(string.Format("otp_send_similar_threshold_interval_seconds_{0}", type), defaultValue);
            }

            return _sendThresholdInterval[type];            
        }

        public int GetSimilarThresholdAttempts(string type)
        {
            if (_sendThreshold == null)
            {
                _sendThreshold = new Dictionary<string, int>();
            }

            var defaultValue = AppSettingsHelper.Int("otp_send_similar_threshold_attempts_default", 3);

            if (string.IsNullOrEmpty(type))
            {
                return defaultValue;
            }

            if (!_sendThreshold.ContainsKey(type))
            {
                _sendThreshold[type] = AppSettingsHelper.Int(string.Format("otp_send_similar_threshold_attempts_{0}", type), defaultValue);
            }

            return _sendThreshold[type];
        }
    }
}