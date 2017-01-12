using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;
    using System.Diagnostics;

    using Quartz;

    [TestClass]
    public class QuartzCronTest
    {
        //[Ignore]
        [TestMethod]
        public void ShouldPrintCronNextTimeTest()
        {
            var cronExpression = new[]
                                 {
                                     "0 0/1 * * * ?",
                                     //"0 0 22 * * ?",
                                     //"0 0 0 * * ?",
                                     //"0 0 9,18 * * ?",
                                     //"0 0 0 5 5 ?",
                                     //"0 0 * * * ?"
                                 };
            Show(cronExpression);
        }

        private static void Show(string[] cronExpression)
        {
            foreach (var exp in cronExpression)
            {
                ShowOne(exp);    
            }            
        }

        private static void ShowOne(string cronExpression)
        {
            Trace.WriteLine(string.Format("*** {0} ***", cronExpression));
            var exp = new CronExpression(cronExpression);
            DateTimeOffset date = new DateTimeOffset(DateTime.Now);
            for (int i = 0; i < 5; i++)
            {
                var utc = exp.GetNextValidTimeAfter(date);
                if (utc.HasValue)
                {
                    var dateTimeOffset = utc.Value.ToLocalTime();
                    date = dateTimeOffset;
                    Trace.WriteLine(dateTimeOffset);
                }
            }
            Trace.WriteLine(string.Format("******", cronExpression));
        }
    }
}