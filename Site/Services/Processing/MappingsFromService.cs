using System;
using Vtb24.Site.Services.Processing.Models;
using Vtb24.Site.Services.ProcessingService;
using System.Linq;

namespace Vtb24.Site.Services.Processing
{
    internal static class MappingsFromService
    {
        public static ProcessingOperationInfo ToProcessingOperationInfo(GetOperationHistoryResponseTypeOperationInfo original)
        {
            if (original == null)
            {
                return null;
            }

            var info = new ProcessingOperationInfo
            {
                Id = original.OperationId,
                Desc = original.OperationDesc,
                PartnerId = original.PartnerId,
                PartnerName = original.PartnerName,
                PosTime = original.TransactionPosDateTime,
                ProcessingTime = original.TransactionProcDateTime,
                Sum = original.OperationSum,
                Type = ToProcessingOperationType(original.OperationTypeCode),
                PosId = original.PosId,
                Cheque = original.Cheque != null ? ToProcessingCheque(original.Cheque) : null,
            };

            if (info.Type == ProcessingOperationType.Deposit && !string.IsNullOrEmpty(original.TransactionExternalId))
            {
                var splitText = original.TransactionExternalId.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
                
                var transactionType = 0;
                if (splitText.Length == 2)
                {
                    var accrualTypeString = splitText[1];
                    int.TryParse(accrualTypeString, out transactionType);
                }

                info.BankAccrualType = transactionType;
            }

            return info;
        }

        public static ProcessingOperationType ToProcessingOperationType(string original)
        {
            switch (original)
            {
                case "W":
                    return ProcessingOperationType.Withdraw;
                case "D":
                    return ProcessingOperationType.Deposit;
                default:
                    return ProcessingOperationType.Unknown;
            }
        }

        public static ProcessingCheque ToProcessingCheque(Cheque original)
        {
            if (original == null)
            {
                return null;
            }

            return new ProcessingCheque
            {
                Id = original.ChequeNumber,
                Sum = original.ChequeSum,
                Items = original.ChequeItem != null
                            ? original.ChequeItem.Select(ToProcessingChequeItem).ToArray()
                            : new ProcessingChequeItem[0]
            };
        }

        public static ProcessingChequeItem ToProcessingChequeItem(ChequeChequeItem original)
        {
            if (original == null)
            {
                return null;
            }

            return new ProcessingChequeItem
            {
                Id = original.ArticleId,
                Amount = original.Amount,
                Name = original.ArticleName,
                Sum = original.ItemSum
            };
        }
    }
}
