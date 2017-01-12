using System;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.DataAccess;
using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlFiles
{
    public class EtlFiles : IEtlFiles
    {
        public EtlFile[] GetLoyaltyNotificationFiles(Guid packageId, DateTime fromDate, DateTime toDate)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddTicks(TimeSpan.TicksPerDay);

            using (var ctx = new EtlDbContext())
            {
                return ctx.GetRequestFilesItems(packageId, fromDate, toDate).ToArray();
            }
        }

        public Tuple<EtlFile, EtlFile>[] GetBankRequestsWithLoyaltyReplies(Guid packageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddTicks(TimeSpan.TicksPerDay);

            using (var ctx = new EtlDbContext())
            {
                return ctx.GetRequestAndReplyFilesItems(packageId, fromDate, toDate, replyFileNamePostfix).ToArray();
            }
        }

        public Tuple<EtlFile, EtlFile>[] GetLoyaltyRequestsWithBankReplies(Guid requestPackageId, Guid replyPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddTicks(TimeSpan.TicksPerDay);

            using (var ctx = new EtlDbContext())
            {
                return ctx.GetRequestAndReplyFilesItems(requestPackageId, replyPackageId, fromDate, toDate, replyFileNamePostfix).ToArray();
            }
        }

        public Tuple<EtlFile, EtlFile, EtlFile>[] GetLoyaltyRequestsWithBankAndLoyaltyReplies(Guid requestPackageId, Guid repliesPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix1, string replyFileNamePostfix2)
        {
            fromDate = fromDate.Date;
            toDate = toDate.Date.AddTicks(TimeSpan.TicksPerDay);

            using (var ctx = new EtlDbContext())
            {
                return ctx.GetRequestAndTwoRepliesFilesItems(requestPackageId, repliesPackageId, fromDate, toDate, replyFileNamePostfix1, replyFileNamePostfix2).ToArray();
            }
        }
    }
}
