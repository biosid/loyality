using System;
using Vtb24.Arms.AdminServices.GiftShopManagement.ImportTasks.Models;

namespace Vtb24.Arms.AdminServices.GiftShopManagement.Delivery.Models
{
    public class DeliveryRatesImportTask
    {
        public string Id { get; set; }

        public int CountFail { get; set; }

        public int CountSuccess { get; set; }

        public DateTime CreationDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }

        public DateTime? StartDateTime { get; set; }

        public string FileUrl { get; set; }

        public string UserId { get; set; }

        public int PartnerId { get; set; }

        public ImportTaskStatus Status { get; set; }
    }
}
