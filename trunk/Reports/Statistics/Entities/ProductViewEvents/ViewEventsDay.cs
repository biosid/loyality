using System;
using System.ComponentModel.DataAnnotations;
using Rapidsoft.VTB24.Reports.Statistics.Models.ProductViewEvents;

namespace Rapidsoft.VTB24.Reports.Statistics.Entities.ProductViewEvents
{
    public class ViewEventsDay
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime? StartTimestamp { get; set; }

        public DateTime? FinishTimestamp { get; set; }

        public ViewEventsDayStatus Status { get; set; }

        public int Count { get; set; }

        public int KeysCount { get; set; }
    }
}
