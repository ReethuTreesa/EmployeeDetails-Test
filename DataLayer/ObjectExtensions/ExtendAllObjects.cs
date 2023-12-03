using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ObjectExtensions
{
	/// <summary>
	/// Object extensions that apply to all objects.
	/// </summary>
	public static class Extensions
	{
		/// <summary>
		/// Compares two complex class objects (of the same type) to see if they're equal. Each 
		/// property is compared for equality between the two objects. Only pumblic properties 
		/// are compared, but non-public properties can optionally be included in the comparison.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="objectFromCompare">The object from compare.</param>
		/// <param name="objectToCompare">The object to compare.</param>
		/// <param name="includeNonPublic">if set to <c>true</c> [include non public].</param>
		/// <returns></returns>
		public static bool CompareEquals<T>(this T objectFromCompare, T objectToCompare, bool includeNonPublic = false)
		{
			bool result = (objectFromCompare == null && objectToCompare == null);
			if (!result)
			{
				Type fromType = objectFromCompare.GetType();
				// If the objects are primitives, we can simply use the Equals method
				if (fromType.IsPrimitive)
				{
					result = objectFromCompare.Equals(objectToCompare);
				}
				// If the objects are strings, we cast them and compare (string comparisons are 
				// always case-sensitive). 
				else if (fromType.FullName.Contains("System.String"))
				{
					result = ((objectFromCompare as string) == (objectToCompare as string));
				}
				// If the objects are DateTimes, we parse the objects to DateTimes and then 
				// compare the ticks
				else if (fromType.FullName.Contains("DateTime"))
				{
					result = DateTime.Parse(objectFromCompare.ToString()).Ticks == DateTime.Parse(objectToCompare.ToString()).Ticks;
				}
				// If the objects are StringBuilders, we compare the strings. If you actually want 
				// to compare the individucal properties of the stringbuilder class, you can simply 
				// comment out this if-block and they will be handled as a complex class.
				else if (fromType.FullName.Contains("System.Text.StringBuilder"))
				{
					result = ((objectFromCompare as StringBuilder).ToString() == (objectToCompare as StringBuilder).ToString());
				}

				// If the objects are collections (like a List<T>, ObservableCollection<T>, or an 
				// array), we have to compare each item in the collections by iterating the 
				// collection and calling this method to do the comparison.
				else if (fromType.IsGenericType || fromType.IsArray)
				{
					// determine which length property and "get" method we need
					string propName = (fromType.IsGenericType) ? "Count" : "Length";
					string methName = (fromType.IsGenericType) ? "get_Item" : "Get";
					// retrieve the property and method
					PropertyInfo propInfo = fromType.GetProperty(propName);
					MethodInfo methInfo = fromType.GetMethod(methName);
					// if our property and method aren't null
					if (propInfo != null && methInfo != null)
					{
						// get the property values for both of our objects
						int fromCount = (int)propInfo.GetValue(objectFromCompare, null); 
						int toCount   = (int)propInfo.GetValue(objectToCompare, null); 
						// make sure we have the same number of objects in both arrays
						result = (fromCount == toCount);
						// if we do, and the count is greater than 0
						if (result && fromCount > 0)
						{
							// Iterate the list using the retrieved "get" method at the specified index.
							for (int index = 0; index < fromCount; index++) 
							{ 
								// Get an instance of the item in the list object 
								object fromItem = methInfo.Invoke(objectFromCompare, new object[] { index });
								object toItem = methInfo.Invoke(objectToCompare, new object[] { index });
								// call this method
								result = CompareEquals(fromItem, toItem);
								// short circuit if the method returned false.
								if (!result)
								{
									break;
								}
							}
						}
					}
					else
					{
						// This should never happen, but if you have issues with other types of 
						// collections, you can start degugging here
					}
				}
				else
				{
					BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
					if (includeNonPublic)
					{
						flags |= BindingFlags.NonPublic;
					}
					PropertyInfo[] props = typeof(T).GetProperties(flags);
					foreach (PropertyInfo prop in props)
					{
						Type type = fromType.GetProperty(prop.Name).GetValue(objectToCompare, null).GetType();
						//Type type = objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare,null).GetType();
						object dataFromCompare = objectFromCompare.GetType().GetProperty(prop.Name).GetValue(objectFromCompare, null);
						object dataToCompare = objectToCompare.GetType().GetProperty(prop.Name).GetValue(objectToCompare, null);
						result = CompareEquals(Convert.ChangeType(dataFromCompare, type), Convert.ChangeType(dataToCompare, type), includeNonPublic);
						// no point in continuing beyond the first property that isn't equal.
						if (!result)
						{
							break;
						}
					}
				}
			}
			return result;
		}

	}
}
