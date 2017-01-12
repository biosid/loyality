namespace Rapidsoft.Loyalty.NotificationSystem.Email
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;

    public class SenderStub : ISender
    {
        #region ISender Members

        public void SendEmail(IList<string> to, string @from, string subject, string body)
        {
            throw new NotImplementedException("SenderStub");
        }

        public void SendEmail(MailMessage message)
        {
            throw new NotImplementedException("SenderStub");
        }

        #endregion
    }
}