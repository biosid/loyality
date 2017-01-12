using System;
using System.IO;
using Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models;
using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Delivery
{
    public class PartnerMatrixImportTaskModel
    {
        public string Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string PartnerName { get; set; }

        public int PartnerId { get; set; }

        public string SourceUrl { get; set; }

        public ImportTaskStatuses Status { get; set; }

        public long SuccessCount { get; set; }

        public long FailCount { get; set; }

        public string UserId { get; set; }

        public static PartnerMatrixImportTaskModel Map(DeliveryRatesImportTask original)
        {
            return new PartnerMatrixImportTaskModel
            {
                Id = original.Id,
                Created = original.CreationDateTime,
                Start = original.StartDateTime,
                End = original.EndDateTime,
                PartnerId = original.PartnerId,
                SourceUrl = Path.GetFileName(original.FileUrl) ?? original.FileUrl,
                Status = original.Status.Map(),
                SuccessCount = original.CountSuccess,
                FailCount = original.CountFail,
                UserId = original.UserId
            };
        }
    }
}
