using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtendDateTimes
	{
		/// <summary>
		/// 
		/// </summary>
		public enum ScheduleMode 
		{
			/// <summary>
			/// The hourly
			/// </summary>
			Hourly,
			/// <summary>
			/// The daily
			/// </summary>
			Daily,
			/// <summary>
			/// The weekly
			/// </summary>
			Weekly,
			/// <summary>
			/// The first day of month
			/// </summary>
			FirstDayOfMonth,
			/// <summary>
			/// The NTH weekday
			/// </summary>
			NthWeekday,
			/// <summary>
			/// The last day of month
			/// </summary>
			LastDayOfMonth,
			/// <summary>
			/// The day of month
			/// </summary>
			DayOfMonth,
			/// <summary>
			/// The specific interval
			/// </summary>
			SpecificInterval
		};

		#region Date comparison

		/// <summary>
		/// 
		/// </summary>
		public enum DateCompareState 
		{
			/// <summary>
			/// The equal
			/// </summary>
			Equal   = 0,
			/// <summary>
			/// The earlier
			/// </summary>
			Earlier = 1,
			/// <summary>
			/// The later
			/// </summary>
			Later   = 2 
		};

		/// <summary>
		/// 
		/// </summary>
		[Flags]
		public enum DatePartFlags 
		{
			/// <summary>
			/// The ticks
			/// </summary>
			Ticks       = 0,
			/// <summary>
			/// The year
			/// </summary>
			Year        = 1,
			/// <summary>
			/// The month
			/// </summary>
			Month       = 2,
			/// <summary>
			/// The day
			/// </summary>
			Day         = 4,
			/// <summary>
			/// The hour
			/// </summary>
			Hour        = 8,
			/// <summary>
			/// The minute
			/// </summary>
			Minute      = 16,
			/// <summary>
			/// The second
			/// </summary>
			Second      = 32,
			/// <summary>
			/// The millisecond
			/// </summary>
			Millisecond = 64 
		};

		/// <summary>
		/// Determines if the current datetime object is partially equal to the specified datetime 
		/// object. (Comparison can be any combination of datetime properties - year, month, day, 
		/// etc). Calling convention is:
		/// 
		/// bool equal = myDate.PartiallyEqual(DateTime.Now, DatePartFlags.Hour | DatePartFlags.Day);
		/// 
		/// The example above checks to see is the hour and the day are equal between myDate and 
		/// DateTime.Now
		/// </summary>
		/// <param name="then">This object.</param>
		/// <param name="now">The object being compared.</param>
		/// <param name="flags">Flags that indicate which part(s) of the datetime objects will be compared.</param>
		/// <returns>True if the indicated components are equal. Otherwise, false.</returns>
		public static bool PartiallyEqual(this DateTime then, DateTime now, DatePartFlags flags = DatePartFlags.Ticks)
		{
			bool isEqual = false;
			if (flags == DatePartFlags.Ticks)
			{
				isEqual = (now == then);
			}
			else
			{
				StringBuilder compareStr = new StringBuilder();
				compareStr.Append(flags.HasFlag(DatePartFlags.Year)       ? "yyyy" : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Month)      ? "MM"   : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Day)        ? "dd"   : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Hour)       ? "HH"   : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Minute)     ? "mm"   : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Second)     ? "ss"   : "");
				compareStr.Append(flags.HasFlag(DatePartFlags.Millisecond)? "fff"  : "");
				isEqual = now.ConvertToInt64(compareStr.ToString()) == then.ConvertToInt64(compareStr.ToString());
			}
			return isEqual;
		}

		/// <summary>
		/// Compare two datetime objects, but ignore the seconds and milliseconds parts.
		/// </summary>
		/// <param name="thisDate">The date to compare with</param>
		/// <param name="thatDate">The date to compare against</param>
		/// <returns></returns>
		public static DateCompareState CompareDatesExtern(DateTime thisDate, DateTime thatDate, bool includeTime=false, DatePartFlags timeFlags= DatePartFlags.Hour|DatePartFlags.Minute)
		{
			return thisDate.CompareDates(thatDate, includeTime, timeFlags);
		}

		/// <summary>
		/// Compare two datetime objects, but ignore the seconds and milliseconds parts.
		/// </summary>
		/// <param name="thisDate">The date to compare with</param>
		/// <param name="thatDate">The date to compare against</param>
		/// <returns></returns>
		public static DateCompareState CompareDates(this DateTime thisDate, DateTime thatDate, bool includeTime=false, DatePartFlags timeFlags= DatePartFlags.Hour|DatePartFlags.Minute)
		{
			DateCompareState state = DateCompareState.Equal;

			TimeSpan thisTime = (includeTime) ? new TimeSpan(timeFlags.HasFlag(DatePartFlags.Hour)           ? thisDate.Hour        : 0,
																timeFlags.HasFlag(DatePartFlags.Minute)      ? thisDate.Minute      : 0,
																timeFlags.HasFlag(DatePartFlags.Second)      ? thisDate.Second      : 0,
																timeFlags.HasFlag(DatePartFlags.Millisecond) ? thatDate.Millisecond : 0)
												: new TimeSpan(0);
			TimeSpan thatTime = (includeTime) ? new TimeSpan(timeFlags.HasFlag(DatePartFlags.Hour)           ? thatDate.Hour        : 0,
																timeFlags.HasFlag(DatePartFlags.Minute)      ? thatDate.Minute      : 0,
																timeFlags.HasFlag(DatePartFlags.Second)      ? thatDate.Second      : 0,
																timeFlags.HasFlag(DatePartFlags.Millisecond) ? thatDate.Millisecond : 0)
												: new TimeSpan(0);

			thisDate = thisDate.SetTime(thisTime);
			thatDate = thatDate.SetTime(thatTime);
			
			long thisValue = thisDate.Ticks;
			long thatValue = thatDate.Ticks;

			if (thisValue < thatValue)
			{
				state = DateCompareState.Earlier;
			}
			else
			{
				if (thisValue > thatValue)
				{
					state = DateCompareState.Later;
				}
			}

#if DEBUG
			// If you want to watch the comparison results in the output window during 
			// a debugging session, uncomment these lines
			//string traceFormat = "CompareDates({0}, {1}) - result = {2}";
			//System.Diagnostics.Trace.WriteLine(string.Format(traceFormat, thisDate.ToString(), thatDate.ToString(), state.ToString()));
#endif

			return state;
		}

		/// <summary>
		/// Returns the highest of either this date, or the specified date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="thatDate">The that date.</param>
		/// <returns></returns>
		public static DateTime Highest(this DateTime date, DateTime thatDate)
		{
			//DateTime result = (date < thatDate) ? thatDate : date;
			DateTime result = new DateTime(Math.Max(date.Ticks, thatDate.Ticks));
			return result;
		}

		/// <summary>
		/// Returns the lowest of either this date, or the specified date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="thatDate">The that date.</param>
		/// <returns></returns>
		public static DateTime Lowest(this DateTime date, DateTime thatDate)
		{
			//DateTime result = (date > thatDate) ? thatDate : date;
			DateTime result = new DateTime(Math.Min(date.Ticks, thatDate.Ticks));
			return result;
		}

		/// <summary>
		/// Returns the difference between this date and the specified date, with respect to the 
		/// specified date part.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="thatDate">The that date.</param>
		/// <param name="datePart">The date part to compare.</param>
		/// <returns></returns>
		public static int DateDiff(this DateTime date, DateTime thatDate, DatePartFlags datePart)
		{
			int result = 0;
			switch (datePart)
			{
				case DatePartFlags.Day:
					result = (int)((date.Date - thatDate.Date).TotalDays);
					break;
				case DatePartFlags.Month:
					result = Math.Abs(((date.Year * 12) + date.Month) - ((thatDate.Year * 12) + thatDate.Month));
					break;
				case DatePartFlags.Year:
					result = Math.Abs(date.Year - thatDate.Year);
					break;
			}
			return result;
		}

		/// <summary>
		/// Determines whether this date is between the two specified dates.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		/// <param name="inclusive">if set to <c>true</c> the code considers the specified start and end dates.</param>
		/// <returns></returns>
		public static bool IsBetween(this DateTime date, DateTime start, DateTime end, bool inclusive)
		{
			bool result = false;
			DateTime d1 = start.Lowest(end);
			DateTime d2 = start.Highest(end);
			if (inclusive)
			{
				result = (date >= d1 && date <= d2);
			}
			else
			{
				result = (date == d1 && date == d2);
			}
			return result;
		}

		#endregion

		#region Fiscal dates

		/// <summary>
		/// The fiscal year start month
		/// </summary>
		private static int fiscalYearStartMonth = 10;

		/// <summary>
		/// Gets or sets the fiscal year start month.
		/// </summary>
		/// <value>
		/// The fiscal year start month.
		/// </value>
		public static int FiscalYearStartMonth 
		{ 
			get { return ExtendDateTimes.fiscalYearStartMonth; }
			set { ExtendDateTimes.fiscalYearStartMonth = value; }
		}

		/// <summary>
		/// Converts thisDate to a fiscal date based on the fiscal year's starting month
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="fiscalStartMonth">The fiscal start month.</param>
		/// <returns></returns>
		public static DateTime ToFiscalDate(this DateTime thisDate, int fiscalStartMonth)
		{
			ExtendDateTimes.fiscalYearStartMonth = fiscalStartMonth;
			return thisDate.ToFiscalDate();
		}

		/// <summary>
		/// Converts thisDate to a CalendarDate based on the specified fiscal year starting month
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="fiscalStartMonth">The fiscal start month.</param>
		/// <returns></returns>
		public static DateTime FromFiscalDate(this DateTime thisDate, int fiscalStartMonth)
		{
			ExtendDateTimes.fiscalYearStartMonth = fiscalStartMonth;
			return thisDate.FromFiscalDate();
		}

		/// <summary>
		/// Returns the fiscal date representation of this date using the 
		/// ExtendDateTimes.fiscalYearStartMonth proprty to determin the start of the fiscal year.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <returns></returns>
		public static DateTime ToFiscalDate(this DateTime thisDate)
		{
			DateTime newDate = new DateTime(0); 
			try
			{
				newDate = thisDate.AddMonths(13 - ExtendDateTimes.fiscalYearStartMonth);
			}
			catch (Exception)
			{
			}
			return newDate;
		}

		/// <summary>
		/// Returns the calendar date representation of this date using the 
		/// ExtendDateTimes.fiscalYearStartMonth proprty to determin the start of the fiscal year.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <returns></returns>
		public static DateTime FromFiscalDate(this DateTime thisDate)
		{
			DateTime newDate = new DateTime(0);
			try
			{
				newDate = thisDate.AddMonths(-(13 - ExtendDateTimes.fiscalYearStartMonth));
			}
			catch (Exception)
			{
			}
			return newDate;
		}

		#endregion Fiscal dates

		#region Set date components

		public static int LastDayOfMonth(this DateTime thisDate)
		{
			int lastDay = thisDate.AddMonths(1).AddDays(-1).Day;
			return lastDay;
		}

		public static DateTime SetDay(this DateTime thisDate, int year, int month, int day, bool zeroTime = false)
		{
			// sanity checks
			if (year < 0 || year > DateTime.MaxValue.Year)
			{
				throw new InvalidOperationException("Year is invalid.");
			}
			if (month < 1 || month > 12)
			{
				throw new InvalidOperationException("Month must be from 1 to 12.");
			}
			int lastDay = new DateTime(year, month, 1).LastDayOfMonth();
			if (day < 1 || day > lastDay)
			{
				throw new InvalidOperationException(string.Format("Day must be from 1 to {0}.", lastDay));
			}

			DateTime newDate = thisDate;
			newDate = newDate.SetYear(year);
			newDate = newDate.SetMonth(month);
			newDate = newDate.SetDay(day);
			if (zeroTime)
			{
				newDate = newDate.Date;
			}
			return newDate;
		}

		/// <summary>
		/// Sets the day for thisDate
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="day">The day.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetDay(this DateTime thisDate, int day)
		{
			int lastDay = thisDate.DaysInMonth();
			if (day < 1 || day > lastDay)
			{
				throw new ArgumentException(string.Format("The specified day ({0}) is an invalid value.", day));
			}
			return (new DateTime(thisDate.Year, thisDate.Month, day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the month for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="month">The month.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetMonth(this DateTime thisDate, int month)
		{
			if (month < 1 || month > 12)
			{
				throw new ArgumentException(string.Format("The specified month ({0}) is an invalid value.", month));
			}
			return (new DateTime(thisDate.Year, month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the year for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="year">The year.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetYear(this DateTime thisDate, int year)
		{
			if (year < 1 || year > DateTime.MaxValue.Year)
			{
				throw new ArgumentException(string.Format("The specified year ({0}) is an invalid value.", year));
			}
			return (new DateTime(year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the hour for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="hour">The hour.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetHour(this DateTime thisDate, int hour)
		{
			if (hour < 0 || hour > 23)
			{
				throw new ArgumentException(string.Format("The specified hour ({0}) is an invalid value.", hour));
			}
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, hour, thisDate.Minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the minute for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="minute">The minute.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetMinute(this DateTime thisDate, int minute)
		{
			if (minute < 0 || minute > 59)
			{
				throw new ArgumentException(string.Format("The specified minute ({0}) is an invalid value.", minute));
			}
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, minute, thisDate.Second, thisDate.Millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the second for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="second">The second.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetSecond(this DateTime thisDate, int second)
		{
			if (second < 0 || second > 59)
			{
				throw new ArgumentException(string.Format("The specified minute ({0}) is an invalid value.", second));
			}
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, second, thisDate.Millisecond, thisDate.Kind));
		}

		
		/// <summary>
		/// Sets the millisecond for thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="millisecond">The millisecond.</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetMillisecond(this DateTime thisDate, int millisecond)
		{
			if (millisecond < 0 || millisecond > 999)
			{
				throw new ArgumentException(string.Format("The specified millisecond ({0}) is an invalid value.", millisecond));
			}
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, thisDate.Second, millisecond, thisDate.Kind));
		}

		/// <summary>
		/// Sets the time to 0:0:0.0
		/// </summary>
		/// <param name="thisDate">This date</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime ZeroSeconds(this DateTime thisDate)
		{
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, thisDate.Hour, thisDate.Minute, 0, 0, thisDate.Kind));
		}

		/// <summary>
		/// Sets the time to 0:0:0.0
		/// </summary>
		/// <param name="thisDate">This date</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime ZeroTime(this DateTime thisDate)
		{
			//return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, 0, 0, 0, 0, thisDate.Kind));
			return thisDate.Date;
		}

		/// <summary>
		/// Sets the time to the specified time span value (days are ignored).
		/// </summary>
		/// <param name="thisDate">This date</param>
		/// <returns>A new datetime object.</returns>
		public static DateTime SetTime(this DateTime thisDate, TimeSpan span)
		{
			if (span == null)
			{
				throw new ArgumentNullException("Span is null");
			}
			return (new DateTime(thisDate.Year, thisDate.Month, thisDate.Day, span.Hours, span.Minutes, span.Seconds, span.Milliseconds, thisDate.Kind));
		}

		/// <summary>
		/// Set the time of component to the specified values. Seconds and milliseconds default to 0, and any 
		/// value is less than 0, that part of the time component is not set. An exception is thrown for any 
		/// value that exceeds the maximum possible value.
		/// </summary>
		/// <param name="thisDate"></param>
		/// <param name="hours"></param>
		/// <param name="mins"></param>
		/// <param name="secs"></param>
		/// <param name="millisecs"></param>
		/// <exception cref="InvalidOperationException"
		/// <returns>A new datetime object with the same date and new time.</returns>
		public static DateTime SetTime(this DateTime thisDate, int hours, int mins, int secs=0, int millisecs=0)
		{
			if (hours > 23)
			{
				throw new InvalidOperationException("Hours must be < 24.");
			}
			if (mins > 59)
			{
				throw new InvalidOperationException("Minutes must be < 60.");
			}
			if (secs > 59)
			{
				throw new InvalidOperationException("Seconds must be < 60.");
			}
			if (millisecs > 999)
			{
				throw new InvalidOperationException("Milliseconds must be < 1000.");
			}
			DateTime newDate = thisDate;
			newDate = (hours     < 0) ? newDate : newDate.SetHour(hours);
			newDate = (mins      < 0) ? newDate : newDate.SetMinute(mins);
			newDate = (secs      < 0) ? newDate : newDate.SetSecond(secs);
			newDate = (millisecs < 0) ? newDate : newDate.SetMillisecond(millisecs);
			return newDate;
		}

		/// <summary>
		/// Removes the time and sets the day to 1.
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		public static DateTime Normalize(this DateTime date)
		{
			return (date.SetDay(1).Date);
		}

		public static DateTime GetNextDayOfWeek(this DateTime thisDate, DayOfWeek dayOfWeek)
		{
			DateTime newDate = thisDate;
			do
			{
				newDate = newDate.AddDays(1);
			} while (newDate.DayOfWeek != dayOfWeek);
			return newDate;
		}

		#endregion Set date components

		#region Helper methods

		/// <summary>
		/// Calculates the number of days in the month of thisDate.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <returns></returns>
		public static int DaysInMonth(this DateTime thisDate)
		{
			return CultureInfo.InvariantCulture.Calendar.GetDaysInMonth(thisDate.Year, thisDate.Month);
		}

		/// <summary>
		/// Determines if the current date represents the last day of its month.
		/// </summary>
		/// <param name="thisDate">The date</param>
		/// <returns>True if the date is the last day of the month.</returns>
		public static bool IsLastDayOfMonth(this DateTime thisDate)
		{
			return (thisDate.Day == thisDate.DaysInMonth());
		}

		/// <summary>
		/// Determines if the current date represents the last day of its month.
		/// </summary>
		/// <param name="thisDate">The date</param>
		/// <returns>True if the date is the last day of the month.</returns>
		public static bool IsWeekend(this DateTime thisDate)
		{
			return (thisDate.DayOfWeek == DayOfWeek.Saturday || thisDate.DayOfWeek == DayOfWeek.Sunday);
		}

		/// <summary>
		/// Determines whether thisDate is a leap year.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <returns></returns>
		public static bool IsLeapYear(this DateTime thisDate)
		{
			return CultureInfo.InvariantCulture.Calendar.IsLeapYear(thisDate.Year);
		}

		/// <summary>
		/// Gets the future date time.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="processTime">The process time.</param>
		/// <param name="mode">The mode.</param>
		/// <param name="dayOfWeek">The day of week - this is only necessary when getting the nth weekday of a month.</param>
		/// <param name="ordinal">The ordinal - this is only necessary when getting the nth weekday of a month.</param>
		/// <returns></returns>
		public static DateTime GetFutureDateTime(this DateTime thisDate, TimeSpan processTime, ScheduleMode mode, DayOfWeek dayOfWeek = DayOfWeek.Sunday, int ordinal = -1)
		{
			DateTime result = thisDate;

			// if the processTime object is null, throw an exception - You could 
			// alternately react to a null processTime by creating one with an 
			// appropriate default value, but I figured this would really serve no 
			// purpose in the grand scheme of things, so I didn't do it here.
			if (processTime == null)
			{
				throw new Exception("Parameter 2 (processTime) cannot be null");
			}

			switch(mode)
			{
				// This mode returns the next even hour of the current datetime.
				case ScheduleMode.Hourly:
					{
						if (result.Minute < processTime.Minutes)
						{
							result = result.AddMinutes(processTime.Minutes - result.Minute);
						}
						else
						{
							// subtract the minutes, seconds, and milliseconds - this allows 
							// us to determine what the last hour was
							result = result.Subtract(new TimeSpan(0, 0, result.Minute, result.Second, result.Millisecond));
							// add an hour and the number of minutes 
							result = result.Add(new TimeSpan(0, 1, processTime.Minutes, 0, 0));
						}
					}
					break;
 
				case ScheduleMode.Daily:
					{
						// subtract the hour, minutes, seconds, and milliseconds (essentially, makes it midnight of the current day)
						result = result.Subtract(new TimeSpan(0,result.Hour,result.Minute,result.Second,result.Millisecond));
						// add a day, and the number of hours:minutes after midnight
						result = result.Add(new TimeSpan(1,processTime.Hours,processTime.Minutes,0,0));
					}
					break;
 
				case ScheduleMode.Weekly:
					{
						int daysToAdd = 7;
						if (result.DayOfWeek != dayOfWeek)
						{
							int dayNumber = (int)dayOfWeek;
							daysToAdd = ((int)(result.DayOfWeek) < dayNumber) ? (int)(result.DayOfWeek) - dayNumber : 7 - dayNumber;
						}
						result = result.AddDays(daysToAdd);
						// get rid of the seconds/milliseconds
						result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
						// add the specified time of day
						result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
					}
					break;

				case ScheduleMode.NthWeekday:
					{
						ordinal = Math.Max(ordinal, 0);
						result  = result.GetDateByOrdinalDay(dayOfWeek, ordinal);
						if (result < thisDate)
						{
							result = result.AddMonths(1).GetDateByOrdinalDay(dayOfWeek, ordinal);
						}
						// get rid of the seconds/milliseconds
						result = result.Subtract(new TimeSpan(0,result.Hour,result.Minute,result.Second,result.Millisecond));
						// add the specified time of day
						result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
					}
					break;

				case ScheduleMode.FirstDayOfMonth:
					{
						// determine how many days in the month
						int daysThisMonth = DaysInMonth(result);
						// for ease of typing
						int today = result.Day;
						// if today is the first day of the month
						if (today == 1)
						{
							// simply add the number of days in the month
							result = result.AddDays(daysThisMonth);
						}
						else
						{
							// otherwise, add the remaining days in the month
							result = result.AddDays((daysThisMonth - today) + 1);
						}
						// get rid of the seconds/milliseconds
						result = result.Subtract(new TimeSpan(0,result.Hour,result.Minute,result.Second,result.Millisecond));
						// add the specified time of day
						result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
					}
					break;

				case ScheduleMode.LastDayOfMonth:
					{
						// determine how many days in the month
						int daysThisMonth = DaysInMonth(result);
						// for ease of typing
						int today = result.Day;
						// if this is the last day of the month
						if (today == daysThisMonth)
						{
							// add the number of days for the next month
							int daysNextMonth = DaysInMonth(result.AddDays(1));
							result = result.AddDays(daysNextMonth);
						}
						else
						{
							// otherwise, add the remaining days for this month
							result = result.AddDays(daysThisMonth - today);
						}
						// get rid of the seconds/milliseconds
						result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
						// add the specified time of day
						result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
					}
					break;

				// The processTime.Day property indicates what day of the month to 
				// schedule, and the hour/minute properties indicate the time of the day 
				case ScheduleMode.DayOfMonth:
					{
						// account for leap year
						// assume we don't have a leap day 
						int leapDay = 0;
						// if it's february, and a leap year and the day is 29
						if (result.Month == 2 && !IsLeapYear(result) && processTime.Days == 29)
						{
							// we have a leap day
							leapDay = 1;
						}

						// If the current day is earlier in the month than the desired day,
						// calculate how many days there are between the two
						int daysToAdd = 0;
						// if the current day is earlier than the desired day
						if (result.Day < processTime.Days)
						{
							// add the difference (less the leap day)
							daysToAdd = processTime.Days - result.Day - leapDay;
						}
						else
						{
							// otherwise, add the days not yet consumed (less the leap day)
							daysToAdd = (DaysInMonth(result) - result.Day) + processTime.Days - leapDay;
						}
						// add the calculated days
						result = result.AddDays(daysToAdd);
						// get rid of the seconds/milliseconds
						result = result.Subtract(new TimeSpan(0, result.Hour, result.Minute, result.Second, result.Millisecond));
						// add the specified time of day
						result = result.Add(new TimeSpan(0, processTime.Hours, processTime.Minutes, 0, 0));
					}
					break;

				case ScheduleMode.SpecificInterval:
					{
						// if we're past the 30-second mark, add a minute to the current time
						if (result.Second >= 30)
						{
							result = result.AddSeconds(60 - result.Second);
						}

						// since we don't care about seconds or milliseconds, zero these items out
						result = result.Subtract(new TimeSpan(0, 0, 0, result.Second, result.Millisecond));
						// now, we add the process time
						result = result.Add(processTime);
					}
					break;
			}
			// and subtract the seconds and milliseconds, just in case they were specified in the 
			// process time 
			result = result.Subtract(new TimeSpan(0, 0, 0, result.Second, result.Millisecond));
			return result;
		}

		/// <summary>
		/// Convert the date to a 64-bit integer representation of "yyyyMMddHHmm" so we
		/// can do an accurate comparison.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <returns></returns>
		public static long ConvertToInt64(this DateTime thisDate)
		{
			long dateValue = Convert.ToInt64(thisDate.ToString("yyyyMMddHHmm"));
			return dateValue;
		}

		/// <summary>
		/// Convert the date to a 64-bit integer representation of this date using the specified
		/// string format "yyyMMddHHmm" so we can do an accurate comparison.
		/// </summary>
		/// <param name="thisDate">The this date.</param>
		/// <param name="format">The format.</param>
		/// <returns></returns>
		public static long ConvertToInt64(this DateTime thisDate, string format)
		{
			long dateValue = Convert.ToInt64(thisDate.ToString(format));
			return dateValue;
		}

		private static int ThisCentury(int year)
		{
			int result = (int)((double)year * 0.01);
			return result;
		}
		private static int ThisCentury(this DateTime thisDate)
		{
			int result = (int)((double)thisDate.Year * 0.01);
			return result;
		}

		/// <summary>
		/// Converts from "mmm-yyyy" to a datetime (if possible) (day set to 1 by default)
		/// </summary>
		/// <param name="data"></param>
		/// <returns>The resulting date, or a zero date if invalid text</returns>
		public static DateTime FromMonthYearString(this DateTime date, string data)
		{
			DateTime newDate = new DateTime(0);
			string[] parts = data.Split('-');
			if (parts.Length == 2)
			{
				if (parts[0].Length==3 && (parts[1].Length == 2 || parts[1].Length == 4))
				{
					if (parts[1].Length == 2)
					{
						int value = Convert.ToInt32(parts[1]);
						int year = (DateTime.Now.Year - value) + value;
						parts[1] = string.Format("{0}", year);
					}
					data = string.Format("{0}-01-{2}", parts[0], parts[1]);
					try
					{
						newDate = DateTime.ParseExact(data, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
					}
					catch (Exception)
					{
						newDate = new DateTime(0);
					}
				}
			}
			return newDate;
		}

		public static DateTime FromDayMonthYearString(this DateTime date, string data)
		{
			DateTime newDate = new DateTime(0);
			string[] parts = data.Split('-');
			if (parts.Length == 3)
			{
				if (parts[1].Length==3 && (parts[2].Length == 2 || parts[2].Length == 4))
				{
					if (parts[2].Length == 2)
					{
						int value = Convert.ToInt32(parts[2]);
						int year = (DateTime.Now.Year - value) + value;
						parts[2] = string.Format("{0}", year);
					}
					data = string.Format("{0}-{1:00}-{2}", parts[1], Convert.ToInt32(parts[0]), Convert.ToInt32(parts[2]));
					try
					{
						newDate = DateTime.ParseExact(data, "MMM-dd-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
					}
					catch (Exception)
					{
						newDate = new DateTime(0);
					}
				}
			}
			return newDate;
		}

		public static DateTime FromMonthYear(int month, int year)
		{
			DateTime result = new DateTime(year, month, 1);
			return result;
		}

		/// <summary>
		/// To the access date time.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <param name="includeTime">if set to <c>true</c> [include time].</param>
		/// <returns></returns>
		public static string ToAccessDateTime(this DateTime date, bool includeTime=false)
		{
			string format = "#{0}{1}#";
			string dateStr = date.ToString("MM/dd/yyyy");
			string timeStr = (includeTime) ? date.ToString(" hh:MM:ss") : "";
			string result = string.Format(format, dateStr, timeStr);
			return result;
		}


		public static DateTime GetDateByOrdinalDay(this DateTime dt,
												   DayOfWeek dayOfWeek, 
												   int ordinal)
		{
			DateTime newDate;
			if (ordinal == 0)
			{
				newDate = dt.LastDayOfWeek(dayOfWeek);
			}
			else
			{
				// Make sure ordinal and month parameters are  within the allowable range
				ordinal = Math.Min(Math.Max(ordinal, 1), 5);

				//Set workingDate to the first day of the month.
				newDate = new DateTime(dt.Year, dt.Month, 1).SetTime(dt.TimeOfDay);
				int diff = 7-Math.Abs((int)dayOfWeek - (int)newDate.DayOfWeek);
				newDate = (diff == 7) ? newDate : newDate.AddDays(diff);
				if (ordinal > 1)
				{
					newDate = newDate.AddDays(7*(ordinal-1));
				}
			}
			return newDate;
		}

		/// <summary>
		/// Counts the number of week days between this date and the specified date. Does 
		/// not include weekend days in the count. it doesn't matter if "thatDate" is in 
		/// the future or past.
		/// </summary>
		/// <param name="thisDate">This date.</param>
		/// <param name="thatDate">That date.</param>
		/// <returns></returns>
		public static int CountWeekDays(this DateTime thisDate, DateTime thatDate)
		{
			int days = Math.Abs((thisDate - thatDate).Days) + 1;
			return ((days/7) * 5) + (days % 7);
		}

		/// <summary>
		/// Returns the ordinal position of the specifie
		/// </summary>
		/// <param name="thisDate"></param>
		/// <param name="dow"></param>
		/// <returns></returns>
		public static int CountDayOfWeek(this DateTime thisDate, DayOfWeek dow)
		{
			int count = 4;
			DateTime newDate = thisDate.GetDateByOrdinalDay(dow, 1);
			count = (newDate.DaysInMonth() - newDate.Day) % 7;
			return count;
		}

		/// <summary>
		/// returns a datetime that representds the last day of the week that contains this date.
		/// </summary>
		/// <param name="thisDate"></param>
		/// <param name="dow"></param>
		/// <returns></returns>
		public static DateTime LastDayOfWeek(this DateTime thisDate, DayOfWeek dow)
		{
			DateTime newDate = thisDate.SetDay(1).AddMonths(1);
			while (!(newDate.DayOfWeek == dow && newDate.Month == thisDate.Month))
			{
				newDate = newDate.AddDays(-1);
			}
			return newDate;
		}

		/// <summary>
		/// Counts the number of instances of the specified day of the week between 
		/// thisDate and the thatDate.
		/// </summary>
		/// <param name="thisDate">The date represented by this date.</param>
		/// <param name="thatDate">The date that represents the other end of the date range.</param>
		/// <param name="dayOfWeek">The day of week to count.</param>
		/// <returns>The number of instances of the specified day of the week</returns>
		public static int CountWeekday(this DateTime thisDate, DateTime thatDate, DayOfWeek dayOfWeek) 
		{
			 int      result    = 0;
			 DateTime past      = (thisDate > thatDate) ? thatDate : thisDate;
			 DateTime future    = (thisDate > thatDate) ? thisDate : thatDate;
			 int      totalDays = (future - past).Days + 1;
			 int temp = (int)((TimeSpan.FromTicks(Math.Abs(future.Ticks - past.Ticks))).TotalDays);

			 int      remainder = totalDays % 7;
			 int      adj       = future.DayOfWeek - dayOfWeek;

			 result  = totalDays / 7;
			 adj    += (adj < 0) ? 7 : 0;
			 result += (remainder >= adj) ? 1 : 0;
			 return result;
		}

		/// <summary>
		/// Returns true if this date is between the two specified dates. Comparison is inclusive.
		/// </summary>
		/// <param name="thisDate"></param>
		/// <param name="date1"></param>
		/// <param name="date2"></param>
		/// <returns></returns>
		public static bool IsBetween(this DateTime thisDate, DateTime date1, DateTime date2)
		{
			bool result = false;
			if (date1 != date2)
			{
				result = (thisDate.Ticks >= Math.Min(date1.Ticks, date2.Ticks) && 
						  thisDate.Ticks <= Math.Max(date1.Ticks, date2.Ticks));
			}
			return result;
		}

		/// <summary>
		/// Returns true if this time is bwteen the two specified times. Comparison is inclusive.
		/// </summary>
		/// <param name="thisTime"></param>
		/// <param name="time1"></param>
		/// <param name="time2"></param>
		/// <returns></returns>
		public static bool IsBetween(this TimeSpan thisTime, TimeSpan time1, TimeSpan time2)
		{
			bool result = false;
			if (time1.TotalMilliseconds != time2.TotalMilliseconds)
			{
				result = (thisTime.TotalMilliseconds >= Math.Min(time1.TotalMilliseconds, time2.TotalMilliseconds) && 
						  thisTime.TotalMilliseconds <= Math.Max(time1.TotalMilliseconds, time2.TotalMilliseconds));
			}
			return result;
		}

		/// <summary>
		/// Returns the datetime that represents the first day of the week that contains this date.
		/// </summary>
		/// <param name="thisDate"></param>
		/// <returns></returns>
		public static DateTime FirstDayOfWeek(this DateTime thisDate)
		{
			DateTime newDate = thisDate;
			CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
			newDate = newDate.AddDays(-(newDate.DayOfWeek - cultureInfo.DateTimeFormat.FirstDayOfWeek));
			return newDate;
		}

		/// <summary>
		/// Rounds the datetime up to the next minute, or if the number of seconds is > 
		/// delta, to the minute after the next minute. The seconds and milliseconds are 
		/// also set to 0 on the returned datetime value.
		/// </summary>
		/// <param name="thisDate"></param>
		/// <param name="delta"></param>
		/// <returns></returns>
		public static DateTime NormalizeToMinute(this DateTime thisDate, int delta=45)
		{
			DateTime newDate = thisDate;
			int minutesToAdd = (newDate.Second < delta) ? 1 : 2;
			newDate = newDate.AddMinutes(minutesToAdd).SetSecond(0).SetMillisecond(0);
			return newDate;
		}

		#endregion Helper methods
	}
}
