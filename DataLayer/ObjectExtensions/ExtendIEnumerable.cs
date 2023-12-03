using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtendIEnumerable
	{
		/// <summary>
		/// Finds the distinct records in a list according to the specified property
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TKey">The type of the key.</typeparam>
		/// <param name="items">The items.</param>
		/// <param name="property">The property.</param>
		/// <returns></returns>
		public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
		{
			return items.GroupBy(property).Select(x => x.First());
		}

       /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the minimal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values.
        /// </summary>
        /// <remarks>
        /// If more than one element has the minimal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current minimal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The minimal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var min = sourceIterator.Current;
                var minKey = selector(min);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, minKey) < 0)
                    {
                        min = candidate;
                        minKey = candidateProjected;
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection.
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MaxBy(selector, Comparer<TKey>.Default);
        }

        /// <summary>
        /// Returns the maximal element of the given sequence, based on
        /// the given projection and the specified comparer for projected values. 
        /// </summary>
        /// <remarks>
        /// If more than one element has the maximal projected value, the first
        /// one encountered will be returned. This overload uses the default comparer
        /// for the projected type. This operator uses immediate execution, but
        /// only buffers a single result (the current maximal element).
        /// </remarks>
        /// <typeparam name="TSource">Type of the source sequence</typeparam>
        /// <typeparam name="TKey">Type of the projected element</typeparam>
        /// <param name="source">Source sequence</param>
        /// <param name="selector">Selector to use to pick the results to compare</param>
        /// <param name="comparer">Comparer to use to compare projected values</param>
        /// <returns>The maximal element, according to the projection.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/>, <paramref name="selector"/> 
        /// or <paramref name="comparer"/> is null</exception>
        /// <exception cref="InvalidOperationException"><paramref name="source"/> is empty</exception>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            using (var sourceIterator = source.GetEnumerator())
            {
                if (!sourceIterator.MoveNext())
                {
                    throw new InvalidOperationException("Sequence contains no elements");
                }
                var max = sourceIterator.Current;
                var maxKey = selector(max);
                while (sourceIterator.MoveNext())
                {
                    var candidate = sourceIterator.Current;
                    var candidateProjected = selector(candidate);
                    if (comparer.Compare(candidateProjected, maxKey) > 0)
                    {
                        max = candidate;
                        maxKey = candidateProjected;
                    }
                }
                return max;
            }
        }

		public static T MaxBy<T>(this IEnumerable<T> list, string valuePropertyName)
		{
			T result = default(T);
			PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
			if (info != null && info.PropertyType.IsNumeric())
			{
				using (var looper = list.GetEnumerator())
				{
					if (!looper.MoveNext())
					{
						throw new InvalidOperationException("Sequence contains no elements");
					}
					T max = default(T);
					while (looper.MoveNext())
					{
						double maxValue = (double)Convert.ChangeType(info.GetValue(max, null), typeof(double));
						T item = looper.Current;
						object propValue = info.GetValue(item, null);
						double number = (double)Convert.ChangeType(propValue, typeof(double));
						if (max.Equals(default(T)) || maxValue < number)
						{
							max = item;
						}
					}
					result = max;
				}
			}
			return result;
		}

		public static Type GetEnumeratedType<T>(this IEnumerable<T> _)
		{
			return typeof(T);
		}

		//public static string PropertyToDelimitedString<T>(this IEnumerable<T> list, string propertyName)
		//{
		//	string delimited = string.Empty;
		//	PropertyInfo props = typeof(T).GetProperties().FirstOrDefault(x=>x.Name == propertyName);

		//	return delimited;
		//}

		#region Trend line calcs for business charts

		public enum TrendType { Exponential, Linear, Logarithmic, PointToPoint, Polynomial, Power, MovingAverage};

		public class PointD
		{
			public double X {get; set; }
			public double Y {get; set; }
			public PointD()
			{
				this.X = Double.NegativeInfinity;
				this.Y = Double.NegativeInfinity;
			}
			public PointD(double x, double y)
			{
				this.X = x;
				this.Y = y;
			}
		}

		/// <summary>
		/// Gets the index of the specified item.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public static int IndexOf<T>(this IEnumerable<T> list, T item)
		{
			int index = -1;
			int counter = 0;
			using (var indexer = list.GetEnumerator())
			{
				while (indexer.MoveNext())
				{
					if (indexer.Current.Equals(item))
					{
						index = counter;
						break;
					}
					counter++;
				}
			}
			return index;
		}

		/// <summary>
		/// Retrieves a list that is a subset of this list where the items returned are within the specified index 
		/// range (regardless of date).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list">This list.</param>
		/// <param name="startIndex">The starting index of the subset.</param>
		/// <param name="endIndex">The ending index of the subset. Default value = 0 indicating the end of the list.</param>
		/// <remarks>The specified indexes are "normalized to ensure that they fall within the appropriate range of 
		/// values. An empty list may be returned with no indication as to why, so if you're expecting values, check 
		/// the list upon which this method is called to ensure that it has items, as well as the specified indexes.</remarks>
		/// <returns>The list of items, or an empty list if no items are found.</returns>
		public static IEnumerable<T> SubsetByIndex<T>(this IEnumerable<T> list, int startIndex, int endIndex=-1)
		{
			//List<T> result = new List<T>();

			//if (startIndex < 0 || startIndex >= list.Count())
			//{
			//	if (ExtendList.throwExtensionExceptions)
			//	{
			//		throw new ArgumentOutOfRangeException("startIndex");
			//	}
			//}

			//if (list.Count() > 0)
			//{
			//	endIndex = (endIndex < 0) ? list.Count() - 1 : Math.Min(endIndex, list.Count() - 1);
			//	startIndex = Math.Min(endIndex, Math.Max(0, startIndex));
			//	result = (from item in list 
			//			  where list.IndexOf(item) >= startIndex && list.IndexOf(item) <= endIndex 
			//			  select item).ToList();
			//}
			//return result;

			List<T> result = new List<T>();
			if (list.Count() > 0)
			{
				if (startIndex < 0 || startIndex >= list.Count())
				{
					if (ExtendList.throwExtensionExceptions)
					{
						throw new ArgumentOutOfRangeException((startIndex < 0) ? "startIndex < 0" : "startIndex > list.Count-1");
					}
				}
				if (endIndex >= 0)
				{
					if (endIndex < startIndex || endIndex >= list.Count())
					{
						throw new ArgumentOutOfRangeException((endIndex < startIndex) ? "endIndex < startIndex" : "endIndex > list.Count-1");
					}
				}
				else
				{
					endIndex = list.Count() - 1;
				}

				result = list.Where(x=>list.IndexOf(x) >= startIndex && list.IndexOf(x) <= endIndex).ToList();
			}
			return result;
		}

		/// <summary>
		/// Gets the value at a given X using the line of best fit (Least Square Method) to determine 
		/// the value. While there are other ways to calculate a trend line, I feel (as does my boss) 
		/// that this is the proper way to calculate slope for a trend line.
		/// </summary>
		/// <param name="points">Points to calculate the value from</param>
		/// <param name="x">Function input</param>
		/// <returns>Value at X in the given points</returns>
		public static double CalcLeastSquares(IEnumerable<PointD> points, double x)
		{
			double result = 0d;
			if (points != null && points.Count() >= 0)
			{
				double slope      = ExtendIEnumerable.CalcSlope(points);
				double yIntercept = ExtendIEnumerable.CalcYIntercept(points, slope);
				result = (slope * x) + yIntercept;
			}
			return result;
		}

		/// <summary>
		/// Gets the slope for a set of points using the formula:
		/// m = ∑ (x-AVG(x)(y-AVG(y)) / ∑ (x-AVG(x))²
		/// </summary>
		/// <param name="points">Points from which to calculate the slope</param>
		/// <returns>The calculated slope value.</returns>
		private static double CalcSlope(IEnumerable<PointD> points)
		{
			double result = 0d;
			if (points != null && points.Count() >= 0)
			{
				double xAvg     = points.Average(p => p.X);
				double yAvg     = points.Average(p => p.Y);
				double dividend = points.Sum(p => (p.X - xAvg) * (p.Y - yAvg));
				double divisor  = (double)points.Sum(p => Math.Pow(p.X - xAvg, 2));
				result = dividend / divisor;
			}
			return result;
		}

		/// <summary>
		/// Gets the Y-Intercept for a set of points using the formula:
		/// b = AVG(y) - m( AVG(x) )
		/// </summary>
		/// <param name="points">Points to calculate the intercept from</param>
		/// <returns>The Y-Intercept value</returns>
		private static double CalcYIntercept(IEnumerable<PointD> points, double slope)
		{
			double result = 0d;
			if (points != null && points.Count() >= 0)
			{
				double xAvg = points.Average(p => p.X);
				double yAvg = points.Average(p => p.Y);
				result = yAvg - (slope * xAvg);
			}
			return result;
		}

		/// <summary>
		/// Base trend calc method. All CalcTrendValues override this method, but eventually call it.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static double[] CalcLinearTrend<T>(this IEnumerable<T> list, PropertyInfo info)
		{
			double[] points = new double[] {0d, 0d};
			if (list != null && list.Count() > 2 && info != null)
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
				points[0] = ExtendIEnumerable.CalcLeastSquares(data, 1);
				points[1] = ExtendIEnumerable.CalcLeastSquares(data, data.Count);
			} 
			return points;
		}

		/// <summary>
		/// Calculates a linear point-to-point trend using the first value in the list and the last 
		/// value in the list as the first and last linear points of the trend line. This is a 
		/// completely invalid way of determining a trend, but it's been requested from time to time, 
		/// so that's why it's here.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="info"></param>
		/// <returns></returns>
		public static double[] CalcPointToPointTrend<T>(this IEnumerable<T> list, PropertyInfo info)
		{
			double[] points = new double[] {0d, 0d};
			if (list != null && list.Count() > 2 && info != null)
			{
				int ptIndex = -1;
				for (int i = 0; i < list.Count(); i++)
				{
					T item = list.ElementAt(i);
					if (i == 0)
					{
						ptIndex = i;
					}
					else if (i == list.Count()-1)
					{
						ptIndex = 1;
					}
					else
					{
						ptIndex = -1;
					}
					if (ptIndex != -1)
					{
						object propValue = info.GetValue(item, null);
						double y = (double)Convert.ChangeType(propValue, typeof(double));
						points[ptIndex] = y;
					}
				}
			}
			return points;
		}

		/// <summary>
		/// Caluclates the trend points based on the specified trend type.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <param name="info"></param>
		/// <param name="trendType"></param>
		/// <returns></returns>
		public static double[] CalcTrendPoints<T>(this IEnumerable<T> list, PropertyInfo info, TrendType trendType)
		{
			double[] points = new double[]  {0d, 0d};

			switch (trendType)
			{
				#region Linear
				case TrendType.Linear :
					{
						points = list.CalcLinearTrend(info);
					}
					break;
				#endregion Linear

				#region Point-to-point
				case TrendType.PointToPoint  :
					{
						points = list.CalcPointToPointTrend(info);
					}
					break;
				#endregion Point-to-point

				case TrendType.Exponential   :
				case TrendType.Logarithmic   :
				case TrendType.MovingAverage :
				case TrendType.Polynomial    :
				case TrendType.Power         :
				default                      :
					throw new Exception(string.Format("TrendType.{0} is not supported at this time.", trendType.ToString()));
			}
			return points;
		}

		/// <summary>
		/// Calculate the trend line points encompassing all items in this list, using the value of 
		/// the specified property
		/// </summary>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, TrendType trendType = TrendType.Linear)
		{
			double[] points = new double[] {0d, 0d};
			try
			{
				if (list.Count() > 2)
				{
					PropertyInfo info = typeof(T).GetProperty(valuePropertyName);
					if (info != null && info.PropertyType.IsNumeric())
					{
						list.CalcTrendPoints(info, trendType);
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
		/// Calculates the trend value points for elements between the specified start and end dates 
		/// (inclusive), using the values for the named properties
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points</param>
		/// <param name="datePropertyName">The name of the property that contains the desired date value</param>
		/// <param name="startDate">The starting date of the date range</param>
		/// <param name="endDate">The ending date of the date range</param>
		/// <returns>An array of points.</returns>
		public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, string datePropertyName, DateTime startDate, DateTime endDate, TrendType trendType = TrendType.Linear)
		{
			double[] points = new double[] { 0d, 0d };
			try
			{
				if (list.Count() > 2)
				{
					PropertyInfo info     = typeof(T).GetProperty(valuePropertyName);
					PropertyInfo dateInfo = typeof(T).GetProperty(datePropertyName);

					if (info     != null && info.PropertyType.IsNumeric() && 
						dateInfo != null && dateInfo.PropertyType.IsDateTime())
					{
						var newList = list.Where(x=>((DateTime)(dateInfo.GetValue(x, null))).IsBetween(startDate, endDate, true));
						newList.CalcTrendPoints(info, trendType);
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
		/// Calculates the trend value points for elements with a date between (inclusive) the 
		/// endDate minus rolling (months) and the specified end date, using the values for the 
		/// named properties.
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points</param>
		/// <param name="datePropertyName">The name of the property that contains the desired date value</param>
		/// <param name="endDate">The ending date of the date range</param>
		/// <param name="rolling">The number of months to include in the trend calculation (this method subtracts 1 from this value)</param>
		/// <returns></returns>
		public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, string datePropertyName, DateTime endDate, int rolling, TrendType trendType = TrendType.Linear)
		{
			DateTime startDate = endDate.AddMonths(-(rolling-1));
			return list.CalcTrendValues(valuePropertyName, datePropertyName, startDate, endDate, trendType);
		}

		/// <summary>
		/// Calculates the trend value points for all items within the subset (inclusive) as 
		/// indicated by the starting/ending index values.
		/// </summary>
		/// <param name="valuePropertyName">The name of the property used to calculate the trend points.</param>
		/// <param name="startIndex">The starting index.</param>
		/// <param name="endIndex">The ending index (or last index if this property is -).1</param>
		/// <returns>A list containing the items at/between the specified indexes.</returns>
		public static double[] CalcTrendValues<T>(this IEnumerable<T> list, string valuePropertyName, int startIndex, int endIndex=-1, TrendType trendType = TrendType.Linear)
		{
			double[] points = new double[] {0d, 0d};
			try
			{
				if (list.Count() > 2)
				{
					PropertyInfo info = typeof(T).GetProperty(valuePropertyName);

					if (info != null && info.PropertyType.IsNumeric())
					{
						var newList = list.SubsetByIndex(startIndex, endIndex);
						points = newList.CalcTrendPoints(info, trendType);
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

		#endregion Trend line calcs for business charts

	}
}
