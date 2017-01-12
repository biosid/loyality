namespace Vtb24.Site.Services
{
    public interface IFeedbackService
    {
        void Send(string type, string name, string emailFrom, string body);
    }
}
