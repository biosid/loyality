using System;

using RapidSoft.Extensions;
using RapidSoft.Loaylty.Logging;
using RapidSoft.Loaylty.Logging.Wcf;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API;
using Rapidsoft.Loyalty.NotificationSystem.API.Entities;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.Services
{
    /// <summary>
    ///     Реализация сервиса работы с сообщениями клиента.
    /// </summary>
    [LoggingBehavior]
    public class ClientFeedbackService : SupportService, IClientFeedbackService
    {
        private readonly IThreadBuilder _threadBuilder;

        private readonly ILog _log = LogManager.GetLogger(typeof(ClientFeedbackService));

        public ClientFeedbackService() : this(null)
        {
        }

        public ClientFeedbackService(IThreadBuilder threadBuilder = null)
        {
            _threadBuilder = threadBuilder ?? new ThreadBuilder();
        }

        #region IClientFeedbackService Members

        public SendFeedbackResult Send(SendFeedbackParameters parameters)
        {
            try
            {
                parameters.ThrowIfNull("parameters is null");
                parameters.ClientFullName.ThrowIfNull("ClientFullName is null");                
                parameters.MessageBody.ThrowIfNull("MessageBody is null");

                var clientType = string.IsNullOrWhiteSpace(parameters.ClientId)
                    ? ThreadClientTypes.Guest
                    : ThreadClientTypes.Client;

                if (clientType == ThreadClientTypes.Guest)
                {
                    parameters.ClientEmail.ThrowIfNull("ClientEmail is null");    
                }

                var messageType = string.IsNullOrWhiteSpace(parameters.ClientId)
                    ? MessageTypes.GuestMessage
                    : MessageTypes.ClientMessage;
                
                var thread = _threadBuilder.CreateThread(
                    parameters.MessageTitle,
                    parameters.ClientEmail, 
                    parameters.ClientFullName, 
                    parameters.ClientId, 
                    clientType, 
                    parameters.ClientFullName, 
                    parameters.ClientFullName, 
                    null, 
                    null, 
                    parameters.Type,
                    messageType,
                    messageType,
                    parameters.MetaData);

                var topicMessage = _threadBuilder.CreateThreadMessage(
                    thread.Id, 
                    messageType, 
                    parameters.MessageBody, 
                    parameters.ClientId, 
                    parameters.ClientFullName, 
                    parameters.ClientEmail, 
                    parameters.Attachments, 
                    true);

                thread.TopicMessage = topicMessage;

                return new SendFeedbackResult
                {
                    Thread = thread
                };
            }
            catch (Exception e)
            {
                _log.Error("Error", e);
                return ServiceOperationResult.BuildErrorResult<SendFeedbackResult>(e);
            }
        }

        #endregion
    }
}