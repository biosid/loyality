using System.ServiceModel;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.API
{
    /// <summary>
    /// Интерфейс сервиса для обратной связи.
    /// </summary>
    [ServiceContract]
    public interface IClientFeedbackService : ISupportService
    {
        [OperationContract]
        SendFeedbackResult Send(SendFeedbackParameters parameters);
    }
}
