namespace RapidSoft.VTB24.BankConnector.Quartz_Jobs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mail;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;

    using Quartz;

    using RapidSoft.Loaylty.Logging;

    public class SendInteractionReportsJob : IInterruptableJob
    {
        private static readonly ReportSettings[] ReportsSettings = new[]
        {
            new ReportSettings("LoyaltyRegistrations", "Регистрация клиентов в Системе лояльности"),
            new ReportSettings("BankRegistrations", "Регистрация клиентов на стороне Банка"),
            new ReportSettings("Activations", "Активация клиентов в Системе лояльности"),
            new ReportSettings("Detachments", "Отключение клиентов от Системы лояльности"),
            new ReportSettings("BankClientUpdates", "Изменение анкетных данных клиентов Банком"),
            new ReportSettings("Accruals", "Начисление бонусов на бонусные счета клиентов"),
            new ReportSettings("PromoActions", "Формирование кампаний"),
            new ReportSettings("Audiences", "Формирование списка участников целевых кампаний"),
            new ReportSettings("Messages", "Формирование персональных сообщений"),
            new ReportSettings("Orders", "Отправка реестра совершенных заказов"),
            new ReportSettings("LoyaltyClientUpdates", "Оповещение об обновлении анкетных данных клиентов"), 
            new ReportSettings("LoginUpdates", "Изменение номера мобильного телефона клиента Банком"),
            new ReportSettings("PasswordResets", "Сброс пароля клиента"),
            new ReportSettings("BankOffers", "Получение персональных банковских предложений")
        };

        private readonly ILog log = LogManager.GetLogger(typeof(SendInteractionReportsJob));

        private readonly Func<QuerySettings, IReportClient> makeReportClient;
        private readonly Func<SendSettings, ISendMailClient> makeSendMailClient;

        public SendInteractionReportsJob()
            : this(null, null)
        {
        }

        public SendInteractionReportsJob(
            Func<QuerySettings, IReportClient> makeReportClient,
            Func<SendSettings, ISendMailClient> makeSendMailClient)
        {
            this.makeReportClient = makeReportClient ?? (settings => new ReportClient(settings));
            this.makeSendMailClient = makeSendMailClient ?? (settings => new SendMailClient(settings));
        }

        public interface IReportClient : IDisposable
        {
            Task<string> GetReportAsync(string type);
        }

        public interface ISendMailClient : IDisposable
        {
            void Send(string subject, string reportHtml);
        }

        public void Execute(IJobExecutionContext context)
        {
            var querySettings = GetQuerySettings(context.MergedJobDataMap);

            var sendSettings = GetSendSettings(context.MergedJobDataMap);

            Execute(querySettings, sendSettings);
        }

        public void Interrupt()
        {
            Thread.CurrentThread.Abort();
        }

        public void Execute(QuerySettings querySettings, SendSettings sendSettings)
        {
            try
            {
                var reports = GetReports(querySettings);

                SendReports(sendSettings, reports);
            }
            catch (Exception e)
            {
                log.Error("Ошибка при отправке отчетов по файлообмену", e);
            }
        }

        private static QuerySettings GetQuerySettings(JobDataMap jobDataMap)
        {
            var nowDate = DateTime.Now.Date;
            var daysNumber = jobDataMap.GetIntValue("reports_days_number");
            var login = jobDataMap.GetString("reports_http_login");
            var pwd = jobDataMap.GetString("reports_http_password");

            return new QuerySettings(
                jobDataMap.GetString("reports_base_url"),
                string.IsNullOrEmpty(login) ? null : new NetworkCredential(login, pwd),
                nowDate.AddTicks((-1 - daysNumber) * TimeSpan.TicksPerDay),
                nowDate.AddTicks(-1 * TimeSpan.TicksPerDay));
        }

        private static SendSettings GetSendSettings(JobDataMap jobDataMap)
        {
            return new SendSettings(
                jobDataMap.GetString("smtp_host"),
                jobDataMap.GetIntValue("smtp_port"),
                jobDataMap.GetBooleanValue("smtp_enable_ssl"),
                jobDataMap.GetString("smtp_login"),
                jobDataMap.GetString("smtp_password"),
                jobDataMap.GetString("reports_send_from"),
                jobDataMap.GetString("reports_send_to"));
        }

        private IEnumerable<Report> GetReports(QuerySettings querySettings)
        {
            using (var client = makeReportClient(querySettings))
            {
                var requests = ReportsSettings
                    .Select(settings => new
                    {
                        settings,
                        task = client.GetReportAsync(settings.Type)
                    })
                    .ToArray();

                var reports =
                    requests.Select(request => new Report(request.settings, querySettings, request.task.Result))
                            .Where(report => report.Html != null)
                            .ToArray();

                return reports;
            }
        }

        private void SendReports(SendSettings settings, IEnumerable<Report> reports)
        {
            using (var client = makeSendMailClient(settings))
            {
                foreach (var report in reports)
                {
                    var subject = string.Format(
                        "Отчет по файлообмену \"{0}\" c {1} по {2}",
                        report.Settings.Title,
                        report.QuerySettings.FromDate.ToString("dd.MM.yyyy"),
                        report.QuerySettings.ToDate.ToString("dd.MM.yyyy"));

                    client.Send(subject, report.Html);
                }
            }
        }

        public class QuerySettings
        {
            public QuerySettings(string url, ICredentials credentials, DateTime from, DateTime to)
            {
                BaseUrl = url;
                Credentials = credentials;
                FromDate = from;
                ToDate = to;
            }

            public string BaseUrl { get; private set; }

            public ICredentials Credentials { get; private set; }

            public DateTime FromDate { get; private set; }

            public DateTime ToDate { get; private set; }
        }

        public class SendSettings
        {
            public SendSettings(
                string host, int port, bool enableSsl, string login, string password, string from, string to)
            {
                SmtpHost = host;
                SmtpPort = port;
                SmtpEnableSsl = enableSsl;
                SmtpLogin = login;
                SmtpPassword = password;
                SendFrom = from;
                SendTo = to;
            }

            public string SmtpHost { get; private set; }

            public int SmtpPort { get; private set; }

            public bool SmtpEnableSsl { get; private set; }

            public string SmtpLogin { get; private set; }

            public string SmtpPassword { get; private set; }

            public string SendFrom { get; private set; }

            public string SendTo { get; private set; }
        }

        private class ReportSettings
        {
            public ReportSettings(string type, string title)
            {
                Type = type;
                Title = title;
            }

            public string Type { get; private set; }

            public string Title { get; private set; }
        }

        private class Report
        {
            public Report(ReportSettings settings, QuerySettings querySettings, string html)
            {
                Settings = settings;
                QuerySettings = querySettings;
                Html = html;
            }

            public ReportSettings Settings { get; private set; }

            public QuerySettings QuerySettings { get; private set; }

            public string Html { get; private set; }
        }

        private class ReportClient : IReportClient
        {
            private readonly ILog log = LogManager.GetLogger(typeof(ReportClient));

            private readonly QuerySettings settings;
            private readonly HttpClient client;
            private readonly WebRequestHandler requestHandler;

            public ReportClient(QuerySettings settings)
            {
                this.settings = settings;

                requestHandler = new WebRequestHandler
                {
                    Credentials = settings.Credentials,
                    ServerCertificateValidationCallback = (sender, cert, chain, errors) => true
                };

                client = new HttpClient(requestHandler, true);
            }

            public void Dispose()
            {
                client.Dispose();
            }

            public Task<string> GetReportAsync(string type)
            {
                var url = string.Format(
                    "{0}?fromdate={1}&todate={2}&type={3}&print=true",
                    settings.BaseUrl,
                    HttpUtility.UrlEncode(settings.FromDate.ToString("dd.MM.yyyy")),
                    HttpUtility.UrlEncode(settings.ToDate.ToString("dd.MM.yyyy")),
                    HttpUtility.UrlEncode(type));

                return client.GetAsync(url).ContinueWith<string>(GetReportString, type);
            }

            private string GetReportString(Task<HttpResponseMessage> task, object state)
            {
                var type = (string)state;

                try
                {
                    using (var response = task.Result)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            log.ErrorFormat(
                                "Не удалось получить отчет типа \"{0}\" за период {1} - {2} (HTTP status = {3})",
                                type,
                                settings.FromDate,
                                settings.ToDate,
                                response.StatusCode);

                            return null;
                        }

                        return response.Content.ReadAsStringAsync().Result;
                    }
                }
                catch (Exception e)
                {
                    log.Error(
                        string.Format(
                            "Ошибка при получении отчета типа \"{0}\" за период {1} - {2}",
                            type,
                            settings.FromDate,
                            settings.ToDate),
                        e);

                    return null;
                }
            }
        }

        private class SendMailClient : SmtpClient, ISendMailClient
        {
            private readonly ILog log = LogManager.GetLogger(typeof(SendMailClient));

            private readonly SendSettings settings;

            public SendMailClient(SendSettings settings)
            {
                this.settings = settings;

                Host = settings.SmtpHost;
                Port = settings.SmtpPort;
                EnableSsl = settings.SmtpEnableSsl;
                Credentials = new NetworkCredential(settings.SmtpLogin, settings.SmtpPassword);
            }

            public void Send(string subject, string reportHtml)
            {
                try
                {
                    var message = new MailMessage
                    {
                        From = new MailAddress(settings.SendFrom),
                        Subject = subject,
                        IsBodyHtml = true,
                        Body = reportHtml
                    };

                    var sendToAddresses = settings.SendTo.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var address in sendToAddresses)
                    {
                        message.To.Add(address);
                    }

                    Send(message);
                }
                catch (Exception e)
                {
                    log.Error("Не удалось отправить отчет \"" + subject + "\"", e);
                }
            }
        }
    }
}
