using System;
using System.ComponentModel.DataAnnotations;

namespace Rapidsoft.VTB24.Reports.Etl.DataAccess.Models
{
    public class EtlOutcomingFile
    {
        [Key]
        public long Id { get; set; }

        public Guid EtlPackageId { get; set; }

        public Guid EtlSessionId { get; set; }

        [Required]
        public string FileName { get; set; }
    }
}
