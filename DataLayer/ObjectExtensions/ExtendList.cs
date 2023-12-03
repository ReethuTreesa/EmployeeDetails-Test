using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using SQLXCommon;

namespace ObjectExtensions
{
	public class PointD
	{
		public double X { get; set; }
		public double Y { get; set; }
		public PointD (double x, double y)
		{
			this.X = x;
			this.Y = y;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	public static class ExtendList
	{
		public static bool throwExtensionExceptions = false;

		/// <summary>
		/// Caluclates the Modes for the list of values.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <returns></returns>
        public static List<double> Modes(this List<double> list)
        {
            var modesList = list.GroupBy(values => values).Select(valueCluster => new
                {
                    Value = valueCluster.Key,
                    Occurrence = valueCluster.Count(),
                }).ToList();
 
            int maxOccurrence = modesList.Max(g => g.Occurrence);
 
            return modesList.Where(x => x.Occurrence == maxOccurrence && maxOccurrence > 1).Select(x => x.Value).ToList();
        }

		/// <summary>
		/// Calculates the median value of the list.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="roundPlaces">The round places.</param>
		/// <returns></returns>
		public static double Median(this List<double> list, int roundPlaces=0)
        {
			List<double> orderedList = list.OrderBy(numbers => numbers).ToList();
 
			int listSize = orderedList.Count;
			double result;
 
			if (listSize % 2 == 0) // even
			{
				int midIndex = listSize/2;
				double v1 = orderedList.ElementAt(midIndex-1);
				double v2 = orderedList.ElementAt(midIndex);
				result = (v1 + v2) / 2d;
			}
			else // odd
			{
				double element = (double) listSize/2;
				element = Math.Round(element, MidpointRounding.AwayFromZero);
 
				result = orderedList.ElementAt((int) (element - 1));
			}

            return Math.Round(result, roundPlaces);
        }

		/// <summary>
		/// Finds the first occurrence of strings like the one specified. The IsLike extension 
		/// method performs similarly to the SQL "LIKE" function.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string FindLike(this List<string> list, string text)
		{
			string found = string.Empty;
			foreach(string item in list)
			{
				if (item.IsLike(text))
				{
					found = item;
					break;
				}
			}
			return found;
		}

		public static int FindIndexLike(this List<string> list, string text)
		{
			int found = -1;
			for (int index = 0; index<list.Count; index++)
			{
				if (list[index].IsLike(text))
				{
					found = index;
					break;
				}
			}
			return found;
		}

		public static string CommaSeparated(this List<string> list)
		{
			StringBuilder result = new StringBuilder();
			foreach(string item in list)
			{
				result.AppendFormat("{0}{1}", (result.Length==0)?"":",", item);
			}
			return result.ToString();
		}

		public static string ToDelimitedString(this List<string> list, char delimiter)
		{
			StringBuilder result = new StringBuilder();
			foreach(string item in list)
			{
				result.AppendFormat("{0}{1}", (result.Length==0)?"":delimiter.ToString(), item);
			}
			return result.ToString();
		}

		/// <summary>
		/// Finds the exact string (including case sensitivity) in a string list.
		/// </summary>
		/// <param name="list"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string FindExact(this List<string> list, string text)
		{
			string found = string.Empty;
			if (list.Contains(text))
			{
				found = text;
			}
			return found;
		}

		/// <summary>
		/// Finds all items that contain the named property with the specified value (the comparison 
		/// is case sensitive).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static IEnumerable<T> FindExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			IEnumerable<T> found = Enumerable.Empty<T>();
			try
			{
				PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
				if (info != null && info.CanRead && info.PropertyType == typeof(string) && info.GetIndexParameters().Length == 0)
				{
					found = (from item in list 
							 let value = (string)(info.GetValue(item, null)) 
							 where value == text 
							 select item).ToList();
				}
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Finds all items that contain the named property that is similar to the specified text 
		/// (the comparison is SQL-like and is case-insensitive).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static IEnumerable<T> FindLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			IEnumerable<T> found = default(IEnumerable<T>);
			try
			{
				PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
				if (info != null && info.CanRead && info.PropertyType == typeof(string) && info.GetIndexParameters().Length == 0)
				{
					found = (from item in list 
								let value = (string)(info.GetValue(item, null)) 
								where value.IsLike(text) 
								select item).ToList();
				}
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Finds the first item that contains the named property whose value is equal to the 
		/// specified text.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static T FindFirstExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			T found = default(T);
			try
			{
				found = list.FindExact(valuePropertyName, text).FirstOrDefault();
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Finds the last item that contains the named property whose value is equal to the 
		/// specified text.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static T FindLastExact<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			T found = default(T);
			try
			{
				found = list.FindExact(valuePropertyName, text).LastOrDefault();
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Finds the first item that contains the named property whose value is similar to the 
		/// specified text (the comparison is SQL-like and is case-insensitive).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static T FindFirstLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			T found = default(T);
			try
			{
				found = list.FindLike(valuePropertyName, text).FirstOrDefault();
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Finds the last item that contains the named property whose value is similar to the 
		/// specified text (the comparison is SQL-like and is case-insensitive).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public static T FindLastLike<T>(this IEnumerable<T> list, string valuePropertyName, string text)
		{
			T found = default(T);
			try
			{
				found = list.FindLike(valuePropertyName, text).LastOrDefault();
			}
			catch (Exception)
			{
			}
			return found;
		}

		/// <summary>
		/// Retrieves a list that is a subset of this list where the items returned are within the specified range.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="startIndex"></param>
		/// <param name="endIndex"></param>
		/// <remarks>The specified indexes are "normalized to ensure that they fall within the appropriate range of 
		/// values. An empty list may be returned with no indication as to why, so if you're expecting values, check 
		/// the list upon which this method is called to ensure that it has items, as well as the specified indexes.</remarks>
		/// <returns>The list of items, or an empty list if no items are found.</returns>
		public static List<T> SubsetByIndex<T>(this List<T> list, int startIndex, int endIndex=-1)
		{
			List<T> result = new List<T>();

			if (startIndex < 0 || startIndex >= list.Count)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw new ArgumentOutOfRangeException("startIndex");
				}
			}

			if (list.Count > 0)
			{
				endIndex = (endIndex < 0) ? list.Count - 1 : Math.Min(endIndex, list.Count - 1);
				startIndex = Math.Min(endIndex, Math.Max(0, startIndex));
				result = (from item in list 
						  where list.IndexOf(item) >= startIndex && list.IndexOf(item) <= endIndex 
						  select item).ToList();
			}
			return result;
		}

		/// <summary>
		/// Find the first item that does not have a zero value for the specified property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName"></param>
		/// <returns></returns>
		public static T FirstNonZeroItem<T>(this List<T> list, string valuePropertyName)
		{
			T result = default(T);
			try
			{
				PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
				var found = (from item in list
							 where (double)Convert.ChangeType(info.GetValue(item, null), typeof(double)) > 0.0d
							 select item).FirstOrDefault();
				result = (T)found;
			}
			catch (Exception ex)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw ex;
				}
			}
			return result;
		}

		/// <summary>
		/// Calculates the highest value of the named property, and is used to set max axis values 
		/// for telerik charts.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="valuePropertyName">The name of the property to evaluate</param>
		/// <param name="minValue">The minimum value to return</param>
		/// <param name="roundUpBy">The increment by which you want to increment the list's max value</param>
		/// <param name="multiplyBy">The amount to multiply the property's value (only needed if the 
		/// property's value is a percentage that needs to be multiplied by 100 in order to establish 
		/// the max value).</param>
		/// <returns>A value that represents the maximum value of a chart axis</returns>
		public static double HighestValue<T>(this List<T> list, string valuePropertyName, double minValue, int roundUpBy, double multiplyBy=1d)
		{
			double result = 0d;
			PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
			if (info != null && info.PropertyType.IsNumeric())
			{
				double max = minValue;
				foreach(T item in list)
				{
					object propValue = info.GetValue(item, null);
					double value = (double)Convert.ChangeType(propValue, typeof(double));
					max = Math.Max(max, value*multiplyBy);
				}

				long remainder; 
				long quotient = Math.DivRem(Convert.ToInt64(Math.Round(max,0)), Convert.ToInt64(roundUpBy), out remainder);
				result = (quotient+1) * roundUpBy;

				if (result - Math.Round(max, 0) <= 1d)
				{
					result += Math.Round(roundUpBy * 0.5, 0);
				}
			}
			return result;
		}

		//public static double HighestValue<T>(this IEnumerator<T> list, string valuePropertyName, double minValue, int roundUpBy, double multiplyBy=1d)
		//{
		//	double result = 0d;
		//	PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
		//	if (info != null && info.PropertyType.IsNumeric())
		//	{
		//		T found = list.MaxBy(valuePropertyName);
		//		object propValue = info.GetValue(found, null);
		//		double max = (double)Convert.ChangeType(propValue, typeof(double));

		//		long remainder; 
		//		long quotient = Math.DivRem(Convert.ToInt64(Math.Round(max,0)), Convert.ToInt64(roundUpBy), out remainder);
		//		result = (quotient+1) * roundUpBy;

		//		if (result - Math.Round(max, 0) <= 1d)
		//		{
		//			result += Math.Round(roundUpBy * 0.5, 0);
		//		}
		//	}
		//	return result;
		//}


		//public static double LowestValue<T>(this List<T> list, string valuePropertyName, double maxValue, int roundUpBy, double multiplyBy=1d)
		//{
		//	double result = 0d;
		//	PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
		//	if (info != null && info.PropertyType.IsNumeric())
		//	{
		//		double min = maxValue;
		//		foreach(T item in list)
		//		{
		//			object propValue = info.GetValue(item, null);
		//			double value = (double)Convert.ChangeType(propValue, typeof(double));
		//			min = Math.Max(min, value * multiplyBy);
		//		}
		//		long rem; 
		//		Math.DivRem(Convert.ToInt64(min), Convert.ToInt64(roundUpBy), out rem);
		//		result = Math.Round((min / roundUpBy), 0) + (roundUpBy - rem);
		//	}
		//	return result;
		//}

		#region trend calcs

		/// <summary>
		/// Gets the value at a given X using the line of best fit (Least Square Method) to determine the equation
		/// </summary>
		/// <param name="points">Points to calculate the value from</param>
		/// <param name="x">Function input</param>
		/// <returns>Value at X in the given points</returns>
		public static double LeastSquaresValueAtX(List<PointD> points, double x)
		{
			double slope      = ExtendList.SlopeOfPoints(points);
			double yIntercept = ExtendList.YInterceptOfPoints(points, slope);
			return ((slope * x) + yIntercept);
		}

		/// <summary>
		/// Gets the slope for a set of points using the formula:
		/// m = ∑ (x-AVG(x)(y-AVG(y)) / ∑ (x-AVG(x))²
		/// </summary>
		/// <param name="points">Points to calculate the Slope from</param>
		/// <returns>SlopeOfPoints</returns>
		private static double SlopeOfPoints(List<PointD> points)
		{
			double xBar     = points.Average(p => p.X);
			double yBar     = points.Average(p => p.Y);
			double dividend = points.Sum(p => (p.X - xBar) * (p.Y - yBar));
			double divisor  = (float)points.Sum(p => Math.Pow(p.X - xBar, 2));
			return (dividend / divisor);
		}

		/// <summary>
		/// Gets the Y-Intercept for a set of points using the formula:
		/// b = AVG(y) - m( AVG(x) )
		/// </summary>
		/// <param name="points">Points to calculate the intercept from</param>
		/// <returns>Y-Intercept</returns>
		private static double YInterceptOfPoints(List<PointD> points, double slope)
		{
			double xBar = points.Average(p => p.X);
			double yBar = points.Average(p => p.Y);
			return (yBar - (slope * xBar));
		}       

		/// <summary>
		/// Base trend calc method. All CalcTrendValues overrides call this method.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static double[] CalcTrendBase<T>(this List<T> list, PropertyInfo info)
		{
			double[] points = new double[] {0d, 0d};
			if (list != null && list.Count > 2 && info != null)
			{
				List<PointD> data = new List<PointD>();
				foreach(T item in list)
				{
					object propValue = info.GetValue(item, null);
					double y = (double)Convert.ChangeType(propValue, typeof(double));
					if (y > 0.0d)
					{
						double x = (double)(list.IndexOf(item));
						data.Add(new PointD(x, y));
					}
				}
				points[0] = ExtendList.LeastSquaresValueAtX(data, 0);
				points[1] = ExtendList.LeastSquaresValueAtX(data, data.Count-1);
			} 
			return points;
		}

		/// <summary>
		/// Calculate the trend line points encompassing all items in this list, using the value of 
		/// the specified property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static double[] CalcTrendValues<T>(this List<T> list, string propertyName)
		{
			double[] points = new double[] {0d, 0d};
			try
			{
				if (list.Count > 2)
				{
					PropertyInfo info = typeof(T).GetProperty(propertyName);
					if (info != null && info.PropertyType.IsNumeric())
					{
						points = list.CalcTrendBase(info);
					}
				}
			}
			catch (Exception ex)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw ex;
				}
			}
			return points;
		}

		/// <summary>
		/// Calculates the trend value points for elements between (includive) the specified start and end dates, using the values for the named properties
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points</param>
		/// <param name="datePropertyName">The name of the property that contains the desired date value</param>
		/// <param name="startDate">The starting date of the date range</param>
		/// <param name="endDate">The ending date of the date range</param>
		/// <returns></returns>
		public static double[] CalcTrendValues<T>(this List<T> list, string valuePropertyName, string datePropertyName, DateTime startDate, DateTime endDate)
		{
			double[] points = new double[] { 0d, 0d };
			try
			{
				if (list.Count > 2)
				{
					PropertyInfo info     = typeof(T).GetProperty(valuePropertyName);
					PropertyInfo dateInfo = typeof(T).GetProperty(datePropertyName);

					if (info     != null && info.PropertyType.IsNumeric() && 
						dateInfo != null && dateInfo.PropertyType.IsDateTime())
					{
						var newList = (from item in list
									   where ((DateTime)(dateInfo.GetValue(item, null))).IsBetween(startDate, endDate, true)
									   select item).ToList();
						points = newList.CalcTrendBase(info);
					}
				}
			}
			catch (Exception ex)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw ex;
				}
			}
			return points;
		}

		/// <summary>
		/// Calculates the trend value points for elements between (includive) the specified start 
		/// and end dates, using the values for the named properties
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points</param>
		/// <param name="datePropertyName">The name of the property that contains the desired date value</param>
		/// <param name="endDate">The ending date of the date range</param>
		/// <param name="rolling">How many months to include in the trend calculation</param>
		/// <returns></returns>
		public static double[] CalcTrendValues<T>(this List<T> list, string propertyName, string datePropName, DateTime endDate, int rolling)
		{
			double[] points = new double[] {0d, 0d};
			try
			{
				if (list.Count > 2)
				{
					PropertyInfo info     = typeof(T).GetProperty(propertyName);
					PropertyInfo dateInfo = typeof(T).GetProperty(datePropName);

					if (info     != null && info.PropertyType.IsNumeric() && 
						dateInfo != null && dateInfo.PropertyType.IsDateTime())
					{
						DateTime startDate = endDate.AddMonths(-(rolling-1));
						var newList = (from item in list
									   where ((DateTime)(dateInfo.GetValue(item, null))).IsBetween(startDate, endDate, true)
									   select item).ToList();
						points = newList.CalcTrendBase(info);
					}
				}
			}
			catch (Exception ex)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw ex;
				}
			}
			return points;
		}

		/// <summary>
		/// Calculates the trend value points for all items within the subset (inclusive) as 
		/// indicated by the starting/ending index values.
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points.</param>
		/// <param name="startIndex">The starting index.</param>
		/// <param name="endIndex">The ending index (or last index if this property is -).1</param>
		/// <returns>A list containing the items at/between the specified indexes.</returns>
		public static double[] CalcTrendValuesStartingAtIndex<T>(this List<T> list, string propertyName, int startIndex, int endIndex=-1)
		{
			double[] points = new double[] {0d, 0d};
			try
			{
				if (list.Count > 2)
				{
					PropertyInfo info = typeof(T).GetProperty(propertyName);

					if (info != null && info.PropertyType.IsNumeric())
					{
						var newList = list.SubsetByIndex(startIndex, endIndex);
						points = newList.CalcTrendBase(info);
					}
				}
			}
			catch (Exception ex)
			{
				if (ExtendList.throwExtensionExceptions)
				{
					throw ex;
				}
			}
			return points;
		}	

		#endregion trend calcs

	}
}


