using System;
using System.Configuration;
using System.Linq;
using System.Web;
using Vtb24.Site.Security.Models;
using Vtb24.Site.Services.ClientFeedback.Models;
using Vtb24.Site.Services.ClientFeedback.Models.Exceptions;
using Vtb24.Site.Services.ClientFeedback.Models.Inputs;
using Vtb24.Site.Services.ClientFeedbackServiceEndpoint;
using Vtb24.Site.Services.EmailSenderService;
using Vtb24.Site.Services.Infrastructure;
using Vtb24.Site.Services.Profile.Models;

namespace Vtb24.Site.Services.ClientFeedback
{
    public class ClientFeedbackService : IClientFeedbackService, IDisposable
    {
        public ClientFeedbackService(ClientPrincipal principal, IClientService client)
        {
            _principal = principal;
            _client = client;
        }

        private readonly ClientPrincipal _principal;
        private readonly IClientService _client;

        public void SendFeedback(SendFeedbackOptions options)
        {
            if (options.Type == FeedbackType.Unsubscribe)
            {
                SendUnsubscribeRequest(options);
            }
            else
            {
                CallFeedbackService(options);
            }
        }

        public void SendBecomeAPartnerRequest(BecomeAPartnerRequest options)
        {
            const string template = @"
            <p><b>Город</b>: {0}</p>
            <p><b>Наименование юридического лица</b>: {1}</p>
            <p><b>Контактное лицо</b>: {2}</p>
            <p><b>Телефон</b>: {3}</p> 
            <p><b>E-mail</b>: {4}</p>
            <p><b>Отрасль компании</b>: {5}</p>
            <p><b>Группа товара</b>: {6}</p>
            <p><b>Адрес сайта</b>: {7}
            <p><b>Количество торговых точек</b>: {8}</p>
            <p><b>Комментарии</b>:<br/><br> {9}</p>
            ";

            var email = AppSettingsHelper.String("feedback_become_a_partner_recipients", "vtb-team@rapidsoft.ru");
            var subject = string.Format(
                "Новая заявка «Cтать партнером программы» от {0}",
                options.LegalEntity.Length > 50 ? options.LegalEntity.Substring(0, 50) + "..." : options.LegalEntity
           );
            var text = string.Format(
                template,
                HttpUtility.HtmlEncode(options.Location),
                HttpUtility.HtmlEncode(options.LegalEntity),
                HttpUtility.HtmlEncode(options.Contact),
                HttpUtility.HtmlEncode(options.PhoneNumber),
                HttpUtility.HtmlEncode(options.Email),
                HttpUtility.HtmlEncode(options.Branch),
                HttpUtility.HtmlEncode(options.GroupOfGoods),
                HttpUtility.HtmlEncode(options.Site),
                HttpUtility.HtmlEncode(options.PosCount),
                HttpUtility.HtmlEncode(options.Comment ?? "<пусто>").Replace("\n", "<br>")
             );

            using (var service = new EmailSenderClient())
            {
                var sendParameters = new SendEmailParameters
                {
                    EmailTo = email,
                    EmailFrom = "noreply@bonus.vtb24.ru",
                    Subject = subject,
                    Body = text
                };

                var result = service.Send(sendParameters);
                if (!result.Success)
                {
                    throw new ClientFeedbackException(result.ResultCode, result.ResultDescription);
                }
            }
        }

        private void CallFeedbackService(SendFeedbackOptions options)
        {
            NormalizeOptions(options);

            using (var service = new ClientFeedbackServiceClient())
            {
                var parameters = new SendFeedbackParameters
                {
                    MessageTitle = options.Title,
                    Type = MapType(options.Type),
                    ClientId = _principal.ClientId,
                    ClientFullName = options.ClientFullName,
                    ClientEmail = options.ClientEmail,
                    MessageBody = options.Text,
                    MetaData = options.MetaData
                };
                var response = service.Send(parameters);

                if (!response.Success)
                {
                    throw new ClientFeedbackException(response.ResultCode, response.ResultDescription);
                }
            }
        }

        private void SendUnsubscribeRequest(SendFeedbackOptions options)
        {
            if (!_principal.IsAuthenticated)
            {
                throw new InvalidOperationException("Неаутентифицированные пользователи не могут отсправлять заявки на отключение от программы");
            }

            var section = (FeedbackConfigSection) ConfigurationManager.GetSection("feedback");
            var feedbackType = section.TypesCollection.OfType<FeedbackTypeElement>().Single(e => e.Name == "Unsubscribe");

            var text = string.Format("Id: {0}. Телефон: {1}. {2}", _principal.ClientId, _principal.PhoneNumber, options.Text);

            using (var service = new EmailSenderClient())
            {
                var sendParameters = new SendEmailParameters
                {
                    EmailTo = feedbackType.Email,
                    EmailFrom = options.ClientEmail,
                    Subject = string.Format(feedbackType.Template, options.ClientFullName),
                    Body = HttpUtility.HtmlEncode(text)
                };

                var result = service.Send(sendParameters);
                if (!result.Success)
                {
                    throw new ClientFeedbackException(result.ResultCode, result.ResultDescription);
                }
            }
        }

        private void NormalizeOptions(SendFeedbackOptions options)
        {
            if (!_principal.IsAuthenticated)
            {
                return;
            }

            var profile = _client.GetProfile();

            if (profile.Status != ClientStatus.Activated)
            {
                return;
            }

            options.ClientFullName = profile.GetFullName();
        }

        private static FeedbackTypes MapType(FeedbackType type)
        {
            switch (type)
            {
                case FeedbackType.Issue:
                    return FeedbackTypes.Issue;
                case FeedbackType.Suggestion:
                    return FeedbackTypes.Suggestion;
                case FeedbackType.OrderIncident:
                    return FeedbackTypes.OrderIncident;
                default:
                    throw new InvalidOperationException("Неверный тип обращения для обратной связи");
            }
        }

        public void Dispose()
        {
            // do nothing
        }
    }
}