using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Services.Processing.Models
{
    public class ProcessingCheque
    {
        public string Id { get; set; }

        public decimal Sum { get; set; }

        public ProcessingChequeItem[] Items { get; set; }
    }
}