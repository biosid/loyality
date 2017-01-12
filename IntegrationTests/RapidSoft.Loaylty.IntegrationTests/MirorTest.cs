using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RapidSoft.Loaylty.IntegrationTests
{
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Threading;

    [TestClass]
    public class MirorTest
    {
        [Ignore]
        [TestMethod]
        public void ShouldContinueWriteDataToMirorServerTest()
        {
            var insertSessionSql = string.Format(@"insert into t1(col1) values('col1val')");

            var insertSessionCommand = new SqlCommand(insertSessionSql);

            while (true)
            {
                try
                {
                    using (var conn = new SqlConnection("Data Source=rphqbuild1;Failover Partner=loyalty-win0;Initial Catalog=test;user id=LoyaltyDB;password=LoyaltyDB"))
                    {
                        conn.Open();
                        insertSessionCommand.Connection = conn;
                        insertSessionCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception exception)
                {
                    Trace.WriteLine(exception);
                }
               
                Thread.Sleep(500);
            }
        }        
    }
}