using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess.Models
{
    public class EtlOutcomingMail
    {
        [Key]
        public long Id { get; set; }

        public Guid EtlPackageId { get; set; }

        public Guid EtlSessionId { get; set; }

        [Column("InsertedDate")]
        public DateTime Timestamp { get; set; }
    }
}
