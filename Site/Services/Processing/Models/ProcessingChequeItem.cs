using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vtb24.Site.Services.Processing.Models
{
    public class ProcessingChequeItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public decimal Sum { get; set; }
    }
}