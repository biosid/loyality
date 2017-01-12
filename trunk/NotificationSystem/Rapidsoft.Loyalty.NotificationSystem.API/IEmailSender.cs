using System.ServiceModel;
using RapidSoft.Loaylty.Monitoring;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;

namespace Rapidsoft.Loyalty.NotificationSystem.API
{
    [ServiceContract]
    public interface IEmailSender : ISupportService
    {
        [OperationContract]
        SendEmailResult Send(SendEmailParameters parameters);
    }
}