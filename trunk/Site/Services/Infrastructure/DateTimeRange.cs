using System;

namespace Vtb24.Site.Services.Infrastructure
{
    public class DateTimeRange
    {
        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public bool HasValues
        {
            get { return Start.HasValue || End.HasValue; }
        }

        public bool HasBothValues
        {
            get { return Start.HasValue && End.HasValue; }
        }

        public DateTimeRange()
        {
        }

        public DateTimeRange(DateTime? start, DateTime? end)
        {
            Start = start;
            End = end;
        }

        public DateTime GetActualStartValue()
        {
            if (!Start.HasValue)
            {
                throw new InvalidCastException("DateTimeRange.Start cant be null!");
            }
            return Start.Value;
        }

        public DateTime GetActualEndValue()
        {
            if (!End.HasValue)
            {
                throw new InvalidCastException("DateTimeRange.End cant be null!");
            }
            return End.Value;           
        }

        public DateTimeRange AddMonths(int months)
        {
            var range = new DateTimeRange();
            if (Start.HasValue)
            {
                range.Start = Start.Value.AddMonths(months);
            }
            if (End.HasValue)
            {
                range.End = End.Value.AddMonths(months);
            }
            return range;
        }

        public bool Includes(DateTime date)
        {
            return (date >= Start) && (date <= End);
        }

        public bool Includes(DateTimeRange range)
        {
            return (Start <= range.Start) && (range.End <= End);
        }

        public static DateTimeRange GetMonthRange(DateTime date)
        {
            var start = new DateTime(date.Year, date.Month, 1);
            var end = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
            
            return new DateTimeRange(start, end);
        }
    }
}