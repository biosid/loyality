using System;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models
{
    public class ProductImportTask
    {
        public int Id { get; set; }
        
        public int CountFail { get; set; }

        public int CountSuccess { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? StartDateTime { get; set; }

        public DateTime CreationDateTime { get; set; }

        public string FileUrl { get; set; }

        public string UserId { get; set; }

        public int SupplierId { get; set; }

        public ImportTaskStatus Status { get; set; }
    }
}
