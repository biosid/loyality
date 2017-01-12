using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using S22.Mail;
using ScheduledJobs.FeedbackByEmail;
using Vtb24.Arms.AdminServices;

namespace ScheduledJobs.Tests
{
    [TestClass]
    public class ReplyHandlerRunner
    {
        [TestMethod]
        public void Run()
        {
            var moq = new Mock<IAdminFeedbackService>();
            var handler = new ReplyMessageHandler(moq.Object);
            using (var stream = File.Open("TestData\\multiview_email.eml", FileMode.Open))
            {
                var mail = MailExtension.Load(stream);
                handler.Execute(mail);
            }
        }
    }
}
