namespace RapidSoft.Extensions
{
	using System;

	public static class TimeSpanExtensions
	{
		public static DateTime ToDataTime(this TimeSpan timeSpan)
		{
			return new DateTime().Add(timeSpan);
		}

		public static DateTime? ToDataTime(this TimeSpan? timeSpan)
		{
			return !timeSpan.HasValue ? (DateTime?)null : timeSpan.Value.ToDataTime();
		}
	}
}
