namespace RapidSoft.VTB24.BankConnector.Tests
{
    using System;
    using System.Data;
    using System.Data.SqlClient;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Quartz;
    using Quartz.Core;
    using Quartz.Impl;
    using Quartz.Plugin.Xml;

    using RapidSoft.Etl.Logging;
    using RapidSoft.Etl.Runtime;
    using RapidSoft.Etl.Runtime.Agents;
    using RapidSoft.Etl.Runtime.Agents.Sql;
    using RapidSoft.VTB24.BankConnector.EtlExecutionWrapper;
    using RapidSoft.VTB24.BankConnector.Infrastructure.Configuration;
    using RapidSoft.VTB24.BankConnector.Quartz_Jobs;

    public static class TestHelper
    {
        public static bool IsSuccess(string etlPackageId, string etlSessionId)
        {
            const string SQL = @"
SELECT COUNT(*)
  FROM {0}.[EtlMessages]
WHERE [EtlPackageId] = @EtlPackageId
  AND [EtlSessionId] = @EtlSessionId
  AND [Text] LIKE 'Session % finished with status ""Succeeded""'";

            using (var con = new SqlConnection(ConfigHelper.ConnectionString))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = string.Format(SQL, "etl");
                comm.CommandType = CommandType.Text;

                comm.Parameters.AddWithValue("@EtlPackageId", etlPackageId);
                comm.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

                var result = comm.ExecuteScalar();

                return Convert.ToInt32(result) > 0;
            }
        }

        public static int CountSendedPromoAction(string etlSessionId)
        {
            const string SQL = @"
SELECT COUNT(*) FROM [dbo].[PromoAction]
WHERE @EtlSessionId = [EtlSessionId]";

            using (var con = new SqlConnection(ConfigHelper.ConnectionString))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = SQL;
                comm.CommandType = CommandType.Text;

                comm.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

                var result = comm.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }

        public static int CountReceivedPromoAction(string etlSessionId)
        {
            const string SQL = @"
SELECT COUNT(*) FROM [dbo].[PromoActionResponse]
WHERE @EtlSessionId = [EtlSessionId]";

            using (var con = new SqlConnection(ConfigHelper.ConnectionString))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = SQL;
                comm.CommandType = CommandType.Text;

                comm.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

                var result = comm.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }

        public static int CountReceivedOrders(string etlSessionId)
        {
            const string SQL = @"
SELECT COUNT(*) FROM [dbo].[OrderPaymentResponse]
WHERE @EtlSessionId = [EtlSessionId]";

            using (var con = new SqlConnection(ConfigHelper.ConnectionString))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = SQL;
                comm.CommandType = CommandType.Text;

                comm.Parameters.AddWithValue("@EtlSessionId", etlSessionId);

                var result = comm.ExecuteScalar();

                return Convert.ToInt32(result);
            }
        }

        internal static void AssertSuccess(WrapperBase job)
        {
            if (!job.IsSuccess())
            {
                Assert.Fail(job.GetMessagesText());
            }
        }

        internal static void AssertJobResult(WrapperBase job, int? errorsCount = null, int? warnCount = null)
        {
            if (errorsCount.HasValue)
            {
                var messages = job.GetMessages(EtlMessageType.Error);
                Assert.IsNotNull(messages);
                Assert.AreEqual(errorsCount, messages.Length, string.Format("Не совпадает количество ошибок {0}", job.GetMessagesText(messages)));
            }

            if (warnCount.HasValue)
            {
                var messages = job.GetMessages(EtlMessageType.Warning);
                Assert.IsNotNull(messages);
                Assert.AreEqual(warnCount, messages.Length, string.Format("Не совпадает количество предупреждений {0}", job.GetMessagesText(messages)));                
            }           
        }

        public static EtlSession CreateWaitingEtlSession(string etlPackageId)
        {
            var agentInfo = new EtlAgentInfo
            {
                ConnectionString = ConfigHelper.ConnectionString,
                SchemaName = ConfigHelper.SchemaName,
            };

            var agent = new SqlEtlAgent(agentInfo);
            return agent.CreateWaitingEtlSession(etlPackageId);
        }

        public static WrapperBase GetWrapper(string packageId, EtlVariableAssignment[] assigments = null)
        {
            var executionContext = new StubJobExecutionContext();

            executionContext.MergedJobDataMap.Add(EtlVariableKeys.PackageId, packageId);

            var executionParameters = JobExecutionParametersBuilder.BuildExecutionParameters(executionContext, assigments);

            var wrapper = new WrapperBase(executionParameters);

            return wrapper;
        }
    }
}