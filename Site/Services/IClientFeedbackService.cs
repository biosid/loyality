using Vtb24.Site.Services.ClientFeedback.Models.Inputs;

namespace Vtb24.Site.Services
{
    public interface IClientFeedbackService
    {
        void SendFeedback(SendFeedbackOptions options);

        void SendBecomeAPartnerRequest(BecomeAPartnerRequest options);
    }
}
