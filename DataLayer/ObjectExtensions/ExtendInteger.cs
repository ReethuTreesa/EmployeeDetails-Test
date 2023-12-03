using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtendInteger
	{
		/// <summary>
		/// Determine if theis value is in the specified string
		/// </summary>
		/// <param name="value"></param>
		/// <param name="container"></param>
		/// <returns></returns>
		public static bool IsIn(this int value, string container)
		{
			return value.ToString().IsIn(container);
		}

		/// <summary>
		/// Determines whether the specified value is even.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static bool IsEven(this int value)
		{
			int remainder = 0;
			if (value > 0)
			{
				Math.DivRem(value, 2, out remainder);
			}
			return (remainder == 0);
		}

		/// <summary>
		/// Determines whether the specified value is even.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <returns></returns>
		public static bool IsEven(this long value)
		{
			long remainder = 0;
			if (value > 0)
			{
				Math.DivRem(value, 2, out remainder);
			}
			return (remainder == 0);
		}

		public static double RoundUpToNearest(this double value, int nearest=10)
		{
			nearest = Math.Max(nearest, 0);
			double result = (nearest > 0) ? Math.Round(Math.Ceiling(value / (double)nearest) * (double)nearest, 0) : 0d;
			return result;
		}

		public static int RoundUpToNearest(this int value, int nearest=10)
		{
			nearest = Math.Max(nearest, 0);
			int result = (int)(Math.Round(Math.Ceiling((double)value / (double)nearest) * (double)nearest, 0));
			return result;
		}

		public static double RoundDownToNearest(this double value, int nearest=10)
		{
			nearest = Math.Max(nearest, 0);
			double result = Math.Max((nearest > 0) ? Math.Round(Math.Floor(value / (double)nearest) * (double)nearest, 0) : 0d, 0d);
			return result;
		}

		public static int DivRemainder(this int value, int divisor)
		{
			int result;
			Math.DivRem(value, divisor, out result);
			return result;
		}

		public static bool IsBetween(this int value, int range1, int range2, bool isInclusive=true)
		{
			bool result = (isInclusive) 
							? (value >= Math.Min(range1, range2) && value <= Math.Max(range1, range2)) 
							: (value > Math.Min(range1, range2) && value < Math.Max(range1, range2));
			return result;
		}
	}

}
