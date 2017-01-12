using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NSubstitute;
using RapidSoft.VTB24.BankConnector.Quartz_Jobs;

namespace RapidSoft.VTB24.BankConnector.Tests.ProcessorTest
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SendWeeklyInteractionReportsTest
    {
        const string REPORT_BASE_URL = "<report_url>";
        const string SMTP_HOST = "<smtp_host>";
        const string SMTP_LOGIN = "<smtp_login>";
        const string SMTP_PASSWORD = "<smtp_password>";
        const string SEND_FROM = "<send_from>";
        const string SEND_TO = "<send_to>";

        [TestMethod]
        public void ShouldSend()
        {
            var reportClient = Substitute.For<SendInteractionReportsJob.IReportClient>();
            reportClient.GetReportAsync(Arg.Any<string>())
                        .Returns(x => Task.Factory.StartNew(() => "report: " + (string) x[0]));
            var countGet = 0;
            reportClient.When(x => x.GetReportAsync(Arg.Any<string>()))
                .Do(x => ++countGet);

            var sendMailClient = Substitute.For<SendInteractionReportsJob.ISendMailClient>();
            var countSend = 0;
            sendMailClient.When(x => x.Send(Arg.Any<string>(), Arg.Any<string>()))
                          .Do(x => ++countSend);

            var job = new SendInteractionReportsJob(settings => reportClient, settings => sendMailClient);

            var nowDate = DateTime.Now.Date;

            var querySettings = new SendInteractionReportsJob.QuerySettings(
                REPORT_BASE_URL,
                new NetworkCredential("USER", "PWD"),
                nowDate.AddTicks(-8 * TimeSpan.TicksPerDay),
                nowDate.AddTicks(-1 * TimeSpan.TicksPerDay));

            var sendSettings = new SendInteractionReportsJob.SendSettings(
                SMTP_HOST, 25, false, SMTP_LOGIN, SMTP_PASSWORD, SEND_FROM, SEND_TO);

            job.Execute(querySettings, sendSettings);

            Assert.IsTrue(countGet > 0);
            Assert.IsTrue(countSend > 0);
            Assert.IsTrue(countGet == countSend);
        }
    }
}
