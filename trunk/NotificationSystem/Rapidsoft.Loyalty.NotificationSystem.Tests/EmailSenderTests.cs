using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rapidsoft.Loyalty.NotificationSystem.API.InputParameters;
using Rapidsoft.Loyalty.NotificationSystem.API.OutputResults;
using Rapidsoft.Loyalty.NotificationSystem.Services;

namespace Rapidsoft.Loyalty.NotificationSystem.Tests.Services
{
    [TestClass]
    public class EmailSenderTests
    {
        [TestMethod]
        public void ShouldSend()
        {
            var sender = new EmailSender();

            var result =
                sender.Send(new SendEmailParameters()
                    {
                        Body = "test",
                        EmailFrom = "zds-@mail.ru",
                        EmailTo = "dmitriy.zakharov@rapidsoft.ru",
                        Subject = "test"
                    });

            Assert.IsTrue(result.Success);
        }

        [TestMethod]
        public void ShouldNotSend()
        {
            var sender = new EmailSender();

            var result =
                sender.Send(new SendEmailParameters()
                {
                    Body = null,
                    EmailFrom = "zds-@mail.ru",
                    EmailTo = "dmitriy.zakharov@rapidsoft.ru",
                    Subject = "test"
                });

            Assert.IsFalse(result.Success);
            Assert.AreEqual(ResultCodes.INVALID_PARAM, result.ResultCode);
        }
    }
}
