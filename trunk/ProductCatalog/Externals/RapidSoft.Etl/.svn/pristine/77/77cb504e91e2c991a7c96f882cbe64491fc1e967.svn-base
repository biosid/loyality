using System;
using System.Configuration;
using System.Resources;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using RapidSoft.Etl.Logging;
using RapidSoft.Etl.Logging.DBScripts;
using RapidSoft.Etl.Runtime.Agents;
using RapidSoft.Etl.Runtime.Agents.Sql;

namespace RapidSoft.Etl.Runtime.Tests.Agents.Sql
{
	[TestClass]
	public class SqlEtlAgentTests
    {
        #region Initialization

        private static readonly string _connectionStringName = "EtlTestDB";
        private static readonly TimeSpan _sessionTimeSpan = new TimeSpan(10, 0, 0, 0);

        [TestInitialize]
        public void Initialize()
        {
            ScriptHelper.ExecuteScript
            (
                _connectionStringName,
                new ResourceManager("RapidSoft.Etl.Logging.Properties.Resources", typeof(IEtlLogger).Assembly).GetString("EtlSqlScript")
            );

            ScriptHelper.ExecuteScript
            (
                _connectionStringName,
                Properties.Resources.EtlTablesData
            );
        }

        #endregion

        #region Tests

        [TestMethod]
		public void CanGetEtlPackages()
		{
            var provider = CreateAgent();
            var packages = provider.GetEtlPackages();
            Assert.IsNotNull(packages);
            Assert.IsTrue(packages.Length > 0);
            Assert.IsNotNull(packages[0]);
        }

        [TestMethod]
        public void CanGetEtlSessionResults()
        {
            var agent = CreateAgent();
            var logParser = agent.GetEtlLogParser();

            var query = new EtlSessionQuery
            {
                FromDateTime = DateTime.Now.Subtract(_sessionTimeSpan),
                ToDateTime = DateTime.Now,
            };
            var sessions = logParser.GetEtlSessions(query);
            Assert.IsNotNull(sessions);
            Assert.IsTrue(sessions.Length > 0);
        }

        [TestMethod]
        public void CannotGetEtlSessionResultsByAbsentId()
        {
            var agent = CreateAgent();
            var logParser = agent.GetEtlLogParser();

            var query = new EtlSessionQuery
            {
                FromDateTime = DateTime.Now.Subtract(_sessionTimeSpan),
                ToDateTime = DateTime.Now,
                EtlPackageIds = 
                { 
                    Guid.Empty.ToString() 
                },
            };
            var sessions = logParser.GetEtlSessions(query);
            Assert.IsNotNull(sessions);
            Assert.AreEqual(0, sessions.Length);
        }

        [TestMethod]
        public void CanGetEtlSessionResultById()
        {
            var agent = CreateAgent();
            var logParser = agent.GetEtlLogParser();

            var query = new EtlSessionQuery
            {
                FromDateTime = DateTime.Now.Subtract(_sessionTimeSpan),
                ToDateTime = DateTime.Now,
            };
            var sessions = logParser.GetEtlSessions(query);
            Assert.IsNotNull(sessions);
            Assert.IsTrue(sessions.Length > 0);

            var packageId = sessions[0].EtlPackageId;
            var sessionId = sessions[0].EtlSessionId;
            var session = logParser.GetEtlSession(packageId, sessionId);
            Assert.IsNotNull(session);
        }

        [TestMethod]
        public void CannotGetEtlSessionResultByAbsentId()
        {
            var agent = CreateAgent();
            var logParser = agent.GetEtlLogParser();

            var session = logParser.GetEtlSession("aaa", "bbb");
            Assert.IsNull(session);
        }

        [TestMethod]
        public void CanGenEtlSessionResultsWork()
        {
            var agent = CreateAgent();
            var logParser = agent.GetEtlLogParser();

            var query = new EtlSessionQuery
            {
                FromDateTime = DateTime.Now.Subtract(_sessionTimeSpan),
                ToDateTime = DateTime.Now,
                EtlPackageIds = 
                { 
                    Guid.Empty.ToString() 
                },
                Variables = 
                { 
                    new EtlVariableFilter("aaa", "bbb"), 
                    new EtlVariableFilter("ccc", "ddd") 
                },
                MaxSessionCount = 10,
            };
            var sessions = logParser.GetEtlSessions(query);
            Assert.IsNotNull(sessions);
            Assert.AreEqual(0, sessions.Length);
        }

        private static SqlEtlAgent CreateAgent()
        {
            var settings = ConfigurationManager.ConnectionStrings[_connectionStringName];

            var source = new EtlAgentInfo
            {
                ConnectionString = settings.ConnectionString
            };
            
            var provider = new SqlEtlAgent(source);
            return provider;
        }

        #endregion
    }
}
