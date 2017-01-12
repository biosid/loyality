using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess.Models
{
    public class EtlCounter
    {
        [Key, Column(Order = 0)]
        public Guid EtlPackageId { get; set; }

        [Key, Column(Order = 1)]
        public Guid EtlSessionId { get; set; }

        [Key, Column(Order = 2)]
        [Required]
        public string EntityName { get; set; }

        [Key, Column(Order = 3)]
        [Required]
        public string CounterName { get; set; }

        public long CounterValue { get; set; }

        public DateTime LogDateTime { get; set; }

        public DateTime LogUtcDateTime { get; set; }
    }
}
