using System;
using Rapidsoft.VTB24.Reports.Etl.EtlFiles.Models;

namespace Rapidsoft.VTB24.Reports.Etl
{
    public interface IEtlFiles
    {
        EtlFile[] GetLoyaltyNotificationFiles(Guid packageId, DateTime fromDate, DateTime toDate);

        Tuple<EtlFile, EtlFile>[] GetBankRequestsWithLoyaltyReplies(Guid packageId, DateTime fromDate, DateTime toDate, string responseFileNamePostfix);

        Tuple<EtlFile, EtlFile>[] GetLoyaltyRequestsWithBankReplies(Guid requestPackageId, Guid replyPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix);

        Tuple<EtlFile, EtlFile, EtlFile>[] GetLoyaltyRequestsWithBankAndLoyaltyReplies(Guid requestPackageId, Guid repliesPackageId, DateTime fromDate, DateTime toDate, string replyFileNamePostfix1, string replyFileNamePostfix2);
    }
}
