using System;
using System.IO;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models;
using Vtb24.Arms.Catalog.Models.Shared;

namespace Vtb24.Arms.Catalog.Models.Gifts
{
    public class GiftsImportHistoryRowModel
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string SourceUrl { get; set; }

        public ImportTaskStatuses Status { get; set; }

        public int SuccessCount { get; set; }

        public int FailCount { get; set; }

        public string UserId { get; set; }

        public static GiftsImportHistoryRowModel Map(ProductImportTask original)
        {
            return new GiftsImportHistoryRowModel
            {
                Id = original.Id,
                Created = original.CreationDateTime,
                Start = original.StartDateTime,
                End = original.EndDateTime,
                SourceUrl = Path.GetFileName(original.FileUrl) ?? original.FileUrl,
                Status = original.Status.Map(),
                SuccessCount = original.CountSuccess,
                FailCount = original.CountFail,
                UserId = original.UserId
            };
        }
    }
}
