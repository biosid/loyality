namespace Quartz.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Quartz.Impl;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented",
        Justification = "Для тестов можно опустить.")]
    [TestClass]
    public class GroupDisallowConcurrentExecutionAttributeTests
    {
        public static Dictionary<DateTime, int> Log = new Dictionary<DateTime, int>(10);

        private readonly string jobGroup = "TestJobGroup";

        #region Additional test attributes

        // You can use the following additional attributes as you write your tests:

        ////Use TestInitialize to run code before running each test 
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{        
        //}

        ////Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{        
        //}

        #endregion

        //[TestMethod]
        [DeploymentItem("..\\Libs\\Common.Logging.Log4Net1211.dll")]
        public void ShouldNotConcurrentExecuteByGroup()
        {
            // Assert.Inconclusive("Очень долгий тес для проверки аттрибута GroupDisallowConcurrentExecution");

            var shedulerFactory = new StdSchedulerFactory();
            var scheduler = shedulerFactory.GetScheduler();
            for (int i = 1; i <= 5; i++)
            {
                var guid = Guid.NewGuid().ToString();

                var job = JobBuilder.Create<TestJob>()
                    .WithIdentity(guid, jobGroup)
                    .RequestRecovery(false)
                    .Build();

                job.JobDataMap.Put("ID", i);

                var trigger = TriggerBuilder.Create()
                    .WithIdentity(guid, jobGroup)
                    .StartNow()
                    .Build();

                scheduler.ScheduleJob(job, trigger);

                Thread.Sleep(100);
            }

            shedulerFactory = new StdSchedulerFactory();
            scheduler = shedulerFactory.GetScheduler();

            scheduler.Start();

            while (Log.Count < 10)
            {
                Thread.Sleep(2000);
            }

            var begin = Log.First();
            Assert.AreEqual(1, begin.Value, "Первым должен выполнится job c ID = 1");
            var end = Log.Skip(1).First();
            Assert.AreEqual(1, end.Value, "Второй запись должен быть тоже job c ID = 1, так как запрет на конкуренцию по группе");

            begin = Log.Skip(2).First();
            Assert.AreEqual(2, begin.Value, "Первым должен выполнится job c ID = 2");
            end = Log.Skip(3).First();
            Assert.AreEqual(2, end.Value, "Второй запись должен быть тоже job c ID = 2, так как запрет на конкуренцию по группе");

            begin = Log.Skip(4).First();
            Assert.AreEqual(3, begin.Value, "Первым должен выполнится job c ID = 3");
            end = Log.Skip(5).First();
            Assert.AreEqual(3, end.Value, "Второй запись должен быть тоже job c ID = 3, так как запрет на конкуренцию по группе");

            begin = Log.Skip(6).First();
            Assert.AreEqual(4, begin.Value, "Первым должен выполнится job c ID = 4");
            end = Log.Skip(7).First();
            Assert.AreEqual(4, end.Value, "Второй запись должен быть тоже job c ID = 4, так как запрет на конкуренцию по группе");

            begin = Log.Skip(8).First();
            Assert.AreEqual(5, begin.Value, "Первым должен выполнится job c ID = 5");
            end = Log.Skip(9).First();
            Assert.AreEqual(5, end.Value, "Второй запись должен быть тоже job c ID = 5, так как запрет на конкуренцию по группе");
        }
    }

    [GroupDisallowConcurrentExecution]
    public class TestJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            int id = dataMap.GetIntValue("ID");
            GroupDisallowConcurrentExecutionAttributeTests.Log.Add(DateTime.Now, id);
            Thread.Sleep(1000);
            GroupDisallowConcurrentExecutionAttributeTests.Log.Add(DateTime.Now, id);
        }
    }
}
