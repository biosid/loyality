using System;

namespace Vtb24.Site.Services.Processing.Models
{
    public class ProcessingOperationInfo
    {
        public long Id { get; set; }

        public string Desc { get; set; }

        public string PartnerId { get; set; }

        public string PartnerName { get; set; }

        public DateTime? PosTime { get; set; }

        public DateTime ProcessingTime { get; set; }

        public string PosId { get; set; }

        public decimal Sum { get; set; }

        public ProcessingOperationType Type { get; set; }

        public ProcessingCheque Cheque { get; set; }

        public int? BankAccrualType { get; set; }

        public string ImagePath { get; set; }
    }
}
