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
	public static class ExtendEnumerators
	{
		/// <summary>
		/// Tries to parse the specified string value to this type of enumerator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="theEnum">The enum.</param>
		/// <param name="value">The value.</param>
		/// <param name="returnValue">The return value.</param>
		/// <returns>
		/// True if parsed. Otherwise, false.
		/// </returns>
		public static bool TryParse<T>(this Enum theEnum, string value, out T returnValue)
		{
			bool result = false;
			returnValue = default(T);
			int ordinal;
			if (Int32.TryParse(value, out ordinal))
			{
				result = theEnum.TryParse(ordinal, out returnValue);
				//if (Enum.IsDefined(typeof(T), intEnumValue))
				//{
				//	returnValue = (T)(object)intEnumValue;
				//	result = true;
				//}
			}
			return result;
		}

		/// <summary>
		/// Tries to parse the specified integer value to this type of enumerator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="theEnum">The enum.</param>
		/// <param name="value">The value.</param>
		/// <param name="returnValue">The return value.</param>
		/// <returns></returns>
		public static bool TryParse<T>(this Enum theEnum, int value, out T returnValue)
		{
			bool result = false;
			returnValue = default(T);
			if (Enum.IsDefined(typeof(T), value))
			{
				returnValue = (T)(object)value;
				result = true;
			}
			return result;
		}
	}
}
