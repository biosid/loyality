using System;
using System.Collections.Generic;
using System.Linq;
using Rapidsoft.VTB24.Reports.Etl.EtlReports.Models;

namespace Rapidsoft.VTB24.Reports.Etl.EtlReports
{
    public class EtlReports : IEtlReports
    {
        public EtlReports(IEtlFiles etlFiles)
        {
            _etlFiles = etlFiles;
        }

        private readonly IEtlFiles _etlFiles;

        public Report<NotificationReportItem> LoyaltyReport(ReportRequest request)
        {
            var itemsBuilder = CreateLoyaltyItemsBuilder(request.Type);

            var timestamp = DateTime.Now;

            var items = itemsBuilder.Build(timestamp, request.FromDate, request.ToDate, true);

            return new Report<NotificationReportItem>
            {
                Request = request,
                Summary = CreateSummary(timestamp, items),
                Items = items
            };
        }

        public Report<RequestWithReplyReportItem> BankLoyaltyReport(ReportRequest request)
        {
            var itemsBuilder = CreateBankLoyaltyItemsBuilder(request.Type);

            var timestamp = DateTime.Now;

            var items = itemsBuilder.Build(timestamp, request.FromDate, request.ToDate, request.SkipRowCountDiscrepancyCheck);

            return new Report<RequestWithReplyReportItem>
            {
                Request = request,
                Summary = CreateSummary(timestamp, items, request.SkipRowCountDiscrepancyCheck),
                Items = items
            };
        }

        public Report<RequestWithReplyReportItem> LoyaltyBankReport(ReportRequest request)
        {
            var itemsBuilder = CreateLoyaltyBankItemsBuilder(request.Type);

            var timestamp = DateTime.Now;

            var items = itemsBuilder.Build(timestamp, request.FromDate, request.ToDate, request.SkipRowCountDiscrepancyCheck);

            return new Report<RequestWithReplyReportItem>
            {
                Request = request,
                Summary = CreateSummary(timestamp, items, request.SkipRowCountDiscrepancyCheck),
                Items = items
            };
        }

        public Report<RequestWithTwoRepliesReportItem> LoyaltyBankLoyaltyReport(ReportRequest request)
        {
            var itemsBuilder = CreateLoyaltyBankLoyaltyItemsBuilder(request.Type);

            var timestamp = DateTime.Now;

            var items = itemsBuilder.Build(timestamp, request.FromDate, request.ToDate, request.SkipRowCountDiscrepancyCheck);

            return new Report<RequestWithTwoRepliesReportItem>
            {
                Request = request,
                Summary = CreateSummary(timestamp, items, request.SkipRowCountDiscrepancyCheck),
                Items = items
            };
        }

        private const string LOYALTY_CLIENT_UPDATES_PACKAGE_ID = "777be283-09f1-4528-bd59-3ba5622fc3ad";

        private IEtlReportItemsBuilder<NotificationReportItem> CreateLoyaltyItemsBuilder(InteractionType type)
        {
            Guid etlPackageId;

            switch (type)
            {
                case InteractionType.LoyaltyClientUpdates:
                    etlPackageId = Guid.Parse(LOYALTY_CLIENT_UPDATES_PACKAGE_ID);
                    break;

                default:
                    throw new ArgumentException("type");
            }

            return new LoyaltyReportItemsBuilder(_etlFiles, etlPackageId);
        }

        private const string BANK_REGISTRATIONS_ETL_PACKAGE_ID = "64e1e608-c27f-43bb-88fa-4865e7178109";
        private const string ACTIVATIONS_ETL_PACKAGE_ID = "e23d8af7-5916-4907-9d99-d69ed8e7d542";
        private const string BANK_CLIENT_UPDATES_ETL_PACKAGE_ID = "0ce83a24-44ac-4d1e-81c2-80d2d7a3b124";
        private const string AUDIENCES_ETL_PACKAGE_ID = "a50fdfdb-92bd-4b27-a9cb-4f80be7c5295";
        private const string MESSAGES_ETL_PACKAGE_ID = "d83d02df-98e0-4714-b06a-9f967930d051";
        private const string ACCRUALS_ETL_PACKAGE_ID = "c137a6c9-059e-4323-bbd5-000fc754457a";
        private const string LOGIN_UPDATES_ETL_PACKAGE_ID = "9f101eda-53e3-40ba-9ece-9106a093ecb1";
        private const string PASSWORD_RESETS_ETL_PACKAGE_ID = "9c12e0c1-9b0b-45a8-ae07-3bc490756f37";
        private const string BANK_OFFERS_ETL_PACKAGE_ID = "3e505ba0-59a6-4093-8286-55501c61ae09";

        private IEtlReportItemsBuilder<RequestWithReplyReportItem> CreateBankLoyaltyItemsBuilder(InteractionType type)
        {
            Guid etlPackageId;

            switch (type)
            {
                case InteractionType.BankRegistrations:
                    etlPackageId = Guid.Parse(BANK_REGISTRATIONS_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Activations:
                    etlPackageId = Guid.Parse(ACTIVATIONS_ETL_PACKAGE_ID);
                    break;

                case InteractionType.BankClientUpdates:
                    etlPackageId = Guid.Parse(BANK_CLIENT_UPDATES_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Accruals:
                    etlPackageId = Guid.Parse(ACCRUALS_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Audiences:
                    etlPackageId = Guid.Parse(AUDIENCES_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Messages:
                    etlPackageId = Guid.Parse(MESSAGES_ETL_PACKAGE_ID);
                    break;

                case InteractionType.LoginUpdates:
                    etlPackageId = Guid.Parse(LOGIN_UPDATES_ETL_PACKAGE_ID);
                    break;

                case InteractionType.PasswordResets:
                    etlPackageId = Guid.Parse(PASSWORD_RESETS_ETL_PACKAGE_ID);
                    break;

                case InteractionType.BankOffers:
                    etlPackageId = Guid.Parse(BANK_OFFERS_ETL_PACKAGE_ID);
                    break;

                default:
                    throw new ArgumentException("type");
            }

            return new BankLoyaltyReportItemsBuilder(_etlFiles, etlPackageId);
        }

        private const string PROMO_ACTIONS_1_ETL_PACKAGE_ID = "7772648e-708f-43d6-8153-c4caa3e2fb05";
        private const string PROMO_ACTIONS_2_ETL_PACKAGE_ID = "77500805-8169-4ae6-87a5-7fcae0fbf398";

        private IEtlReportItemsBuilder<RequestWithReplyReportItem> CreateLoyaltyBankItemsBuilder(InteractionType type)
        {
            Guid requestEtlPackageId;
            Guid replyEtlPackageId;

            switch (type)
            {
                case InteractionType.PromoActions:
                    requestEtlPackageId = Guid.Parse(PROMO_ACTIONS_1_ETL_PACKAGE_ID);
                    replyEtlPackageId = Guid.Parse(PROMO_ACTIONS_2_ETL_PACKAGE_ID);
                    break;

                default:
                    throw new ArgumentException("type");
            }

            return new LoyaltyBankReportItemsBuilder(_etlFiles, requestEtlPackageId, replyEtlPackageId);
        }

        private const string LOYALTY_REGISTRATIONS_1_ETL_PACKAGE_ID = "9517516a-ee80-4963-b326-7ef56efd9691";
        private const string LOYALTY_REGISTRATIONS_2_ETL_PACKAGE_ID = "1560ecaf-7420-4c34-8eea-d6caee64acbc";
        private const string DETACHMENTS_1_ETL_PACKAGE_ID = "a8fbdf89-c705-4a35-a706-a23dd5abadb0";
        private const string DETACHMENTS_2_ETL_PACKAGE_ID = "f2c56eab-2b60-4af2-8712-90f533ee095e";
        private const string ORDERS_1_ETL_PACKAGE_ID = "6dd71e6c-0fd6-4f27-bc3e-abb94921b7cf";
        private const string ORDERS_2_ETL_PACKAGE_ID = "b8c07e39-37e9-45f1-8a79-2a99d86b9641";

        private IEtlReportItemsBuilder<RequestWithTwoRepliesReportItem> CreateLoyaltyBankLoyaltyItemsBuilder(InteractionType type)
        {
            Guid requestEtlPackageId;
            Guid repliesEtlPackageId;

            switch (type)
            {
                case InteractionType.LoyaltyRegistrations:
                    requestEtlPackageId = Guid.Parse(LOYALTY_REGISTRATIONS_1_ETL_PACKAGE_ID);
                    repliesEtlPackageId = Guid.Parse(LOYALTY_REGISTRATIONS_2_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Detachments:
                    requestEtlPackageId = Guid.Parse(DETACHMENTS_1_ETL_PACKAGE_ID);
                    repliesEtlPackageId = Guid.Parse(DETACHMENTS_2_ETL_PACKAGE_ID);
                    break;

                case InteractionType.Orders:
                    requestEtlPackageId = Guid.Parse(ORDERS_1_ETL_PACKAGE_ID);
                    repliesEtlPackageId = Guid.Parse(ORDERS_2_ETL_PACKAGE_ID);
                    break;

                default:
                    throw new ArgumentException("type");
            }

            return new LoyaltyBankLoyaltyReportItemsBuilder(_etlFiles, requestEtlPackageId, repliesEtlPackageId);
        }

        private static ReportSummary CreateSummary(DateTime timestamp, IEnumerable<NotificationReportItem> items)
        {
            return new ReportSummary
            {
                Timestamp = timestamp,
                ShowReplyDelayed = false,
                ShowRowCountDiscrepancy = false,
                ShowSizeExceeded = true,
                HasAnySizeExceeded = items.Any(item => item.Notification.SizeExceeded)
            };
        }

        private static ReportSummary CreateSummary(DateTime timestamp, RequestWithReplyReportItem[] items, bool skipRowCountDiscrepancyCheck)
        {
            return new ReportSummary
            {
                Timestamp = timestamp,
                ShowReplyDelayed = true,
                ShowRowCountDiscrepancy = !skipRowCountDiscrepancyCheck,
                ShowSizeExceeded = true,
                HasAnyReplyDelayed = items.Any(item => item.IsReplyDelayed),
                HasAnyRowCountDiscrepancy = skipRowCountDiscrepancyCheck && items.Any(item => item.Reply != null && item.Reply.RowCount != item.Request.RowCount),
                HasAnySizeExceeded = items.Any(item => item.Request.SizeExceeded || (item.Reply != null && item.Reply.SizeExceeded))
            };
        }

        private static ReportSummary CreateSummary(DateTime timestamp, RequestWithTwoRepliesReportItem[] items, bool skipRowCountDiscrepancyCheck)
        {
            return new ReportSummary
            {
                Timestamp = timestamp,
                ShowReplyDelayed = true,
                ShowRowCountDiscrepancy = !skipRowCountDiscrepancyCheck,
                ShowSizeExceeded = true,
                HasAnyReplyDelayed = items.Any(item => item.IsBankReplyDelayed || item.IsLoyaltyReplyDelayed),
                HasAnyRowCountDiscrepancy = skipRowCountDiscrepancyCheck && items.Any(item => item.HasBankRowCountDiscrepancy || item.HasLoyaltyRowCountDiscrepancy),
                HasAnySizeExceeded = items.Any(item => item.Request.SizeExceeded ||
                                                       (item.BankReply != null && item.BankReply.SizeExceeded) ||
                                                       (item.LoyaltyReply != null && item.LoyaltyReply.SizeExceeded))
            };
        }
    }
}
