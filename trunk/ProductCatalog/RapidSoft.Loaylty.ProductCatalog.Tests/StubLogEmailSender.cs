namespace RapidSoft.Loaylty.ProductCatalog.Tests
{
    using System.Diagnostics;

    using Etl.LogSender;

    public class StubLogEmailSender : ILogEmailSender
    {
        public void SendMail(string subject, string[] recipients, string mailBody)
        {
            Trace.WriteLine(string.Format("StubLogEmailSender:Email message subject:{0} recipients:{1} body:{2}", subject, string.Join(",", recipients), mailBody));
        }
    }
}