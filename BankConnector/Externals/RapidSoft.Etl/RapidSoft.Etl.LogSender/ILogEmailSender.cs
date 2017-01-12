namespace RapidSoft.Etl.LogSender
{
    public interface ILogEmailSender
    {
        void SendMail(string subject, string[] recipients, string mailBody);
    }
}