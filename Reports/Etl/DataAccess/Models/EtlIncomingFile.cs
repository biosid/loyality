using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess.Models
{
    public class EtlIncomingFile
    {
        [Key]
        [Column("SeqId")]
        public int Id { get; set; }

        public Guid EtlPackageId { get; set; }

        public Guid EtlSessionId { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
