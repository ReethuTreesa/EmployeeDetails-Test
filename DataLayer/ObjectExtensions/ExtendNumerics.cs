using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendNumerics
	{
		public static double Truncate(this double value, int places)
		{
			double result = Math.Round(((value >= 0d) ? Math.Floor(value * 100d) : Math.Ceiling(value * 100d)) / 100d, places);
			return result;
		}

		public static decimal Truncate(this decimal value, int places)
		{
			decimal result = Math.Round(((value >= 0M) ? Math.Floor(value * 100M) : Math.Ceiling(value * 100M)) / 100M, places);
			return result;
		}
	}
}
