using System;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.Configuration;
using Rapidsoft.Loyalty.NotificationSystem.Email;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    public class EmailSender : SupportService, IEmailSender
    {
        private readonly ISender _emailSender;

        private readonly ILog _log = LogManager.GetLogger(typeof(EmailSender));

        public EmailSender()
        {
            _emailSender = new Sender();

            if (ServiceConfiguration.UseStub)
            {
                _emailSender = new SenderStub();
            }
        }

        #region IEmailSender Members

        public SendEmailResult Send(SendEmailParameters parameters)
        {
            if (string.IsNullOrEmpty(parameters.EmailFrom))
            {
                return ServiceOperationResult.BuildInvalidParam<SendEmailResult>("Поле EmailFrom не заполнено.");
            }

            if (string.IsNullOrEmpty(parameters.EmailTo))
            {
                return ServiceOperationResult.BuildInvalidParam<SendEmailResult>("Поле EmailTo не заполнено.");
            }

            if (string.IsNullOrEmpty(parameters.Body))
            {
                return ServiceOperationResult.BuildInvalidParam<SendEmailResult>("Поле Body не заполнено.");
            }

            if (string.IsNullOrEmpty(parameters.Subject))
            {
                return ServiceOperationResult.BuildInvalidParam<SendEmailResult>("Поле Subject не заполнено.");
            }

            try
            {
                var emailsTo = parameters.EmailTo.Split(';');

                _emailSender.SendEmail(emailsTo, parameters.EmailFrom, parameters.Subject, parameters.Body);

                return new SendEmailResult
                {
                    ResultCode = 0
                };
            }
            catch (Exception ex)
            {
                _log.Error("Ошибка при отправлении письма", ex);
                return ServiceOperationResult.BuildErrorResult<SendEmailResult>(ex);
            }
        }

        #endregion
    }
}