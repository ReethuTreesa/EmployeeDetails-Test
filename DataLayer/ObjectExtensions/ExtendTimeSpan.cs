using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendTimeSpan
	{
		public static TimeSpan AddHours(this TimeSpan span, double hours)
		{
			return span.Add(TimeSpan.FromHours(hours));
		}

		public static TimeSpan AddMinutes(this TimeSpan span, double minutes)
		{
			return span.Add(TimeSpan.FromMinutes(minutes));
		}

		public static TimeSpan AddSeconds(this TimeSpan span, double seconds)
		{
			return span.Add(TimeSpan.FromSeconds(seconds));
		}

		public static TimeSpan AddMilliseconds(this TimeSpan span, double milliseconds)
		{
			return span.Add(TimeSpan.FromMilliseconds(milliseconds));
		}

		public static TimeSpan AddHours(this TimeSpan span, int hours)
		{
			return span.AddHours((double)hours);
		}

		public static TimeSpan AddMinutes(this TimeSpan span, int minutes)
		{
			return span.AddMinutes((double)minutes);
		}

		public static TimeSpan AddSeconds(this TimeSpan span, int seconds)
		{
			return span.AddSeconds((double)seconds);
		}

		public static TimeSpan AddMilliseconds(this TimeSpan span, int milliseconds)
		{
			return span.AddMilliseconds((double)milliseconds);
		}
	}
}
