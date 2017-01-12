using Vtb24.Site.Security.Infrastructure;

namespace Vtb24.Site.Security.SecurityService
{
    public class SecurityConfiguration
    {
        private int? _passwordAttempts;
        private int? _lockout;
        private int? _tmpLength;
        private bool? _disableBankSms;
        private int? _pwdChangeOtpSmsLimit;
        private bool? _enableOtpViaEmail;

        public int AllowedPasswordAttempts
        {
            get { return _passwordAttempts ?? (_passwordAttempts = AppSettingsHelper.Int("security_allowed_password_attempts", 5)).Value; }
            set { _passwordAttempts = value; }
        }

        public int LockoutIntervalInMinutes
        {
            get { return _lockout ?? (_lockout = AppSettingsHelper.Int("security_lockout_interval_minutes", 30)).Value; }
            set { _lockout = value; }
        }

        public int TempPasswordLength
        {
            get { return _tmpLength ?? (_tmpLength = AppSettingsHelper.Int("security_temp_password_length", 8)).Value; }
            set { _tmpLength = value; }
        }

        public bool DisableBankSms
        {
            get { return _disableBankSms ?? (_disableBankSms = AppSettingsHelper.Bool("security_disable_bank_sms", false)).Value; }
            set { _disableBankSms = value; }
        }

        public int PasswordChangeOtpSmsLimit
        {
            get { return _pwdChangeOtpSmsLimit ?? (_pwdChangeOtpSmsLimit = AppSettingsHelper.Int("security_password_change_otp_sms_limit", -1)).Value; }
            set { _pwdChangeOtpSmsLimit = value; }
        }

        public bool EnableOtpViaEmail
        {
            get { return _enableOtpViaEmail ?? (_enableOtpViaEmail = AppSettingsHelper.Bool("security_otp_via_email_enabled", false)).Value; }
            set { _enableOtpViaEmail = value; }
        }
    }
}