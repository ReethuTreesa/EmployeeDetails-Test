using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ObjectExtensions
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class ExtendLinqToXml
    {
		/// <summary>
		/// Determines whether the specified element has attribute.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static bool HasAttribute(this XElement element, string name)
		{
			bool result = false;
			IEnumerable<XAttribute> attributes = element.Attributes(name);
			result = (attributes.Count() > 0);
			return result;
		}

		/// <summary>
		/// Determines whether the specified element has attribute.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="name">The name.</param>
		/// <returns></returns>
		public static bool HasElement(this XElement element, string name)
		{
			bool result = false;
			IEnumerable<XElement> elements = element.Elements(name);
			result = (elements.Count() > 0);
			return result;
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public static T GetValue<T>(this XElement root, string name, T defaultValue)
		{
			T value = defaultValue;
			string strValue = (string)root.Elements(name).FirstOrDefault() ?? defaultValue.ToString();

			if (value is string)
			{
				return ((T)(object)strValue);
			}
			else
			{
				var tryParse = typeof (T).GetMethod("TryParse", new [] {typeof(string), typeof(T).MakeByRefType()});
				if (tryParse == null)
				{
					throw new InvalidOperationException();
				}
				var parameters = new object[] {strValue, value};
				if ((bool)tryParse.Invoke(null, parameters))
				{
					value = (T)parameters[1];
				}
			}
			return value;
		}

		///// <summary>
		///// Parses the rectangle coordinates.
		///// </summary>
		///// <param name="coords">The coords.</param>
		///// <returns></returns>
		//private static Rectangle ParseRectCoords(string coords)
		//{
		//	Rectangle rect = new Rectangle(-1,-1,-1,-1);
		//	coords         = coords.Replace(" ", "");
		//	string[] parts = coords.Split(',');
		//	bool     valid = true;

		//	foreach(string value in parts)
		//	{
		//		int temp;
		//		if (!int.TryParse(value, out temp))
		//		{
		//			valid = false;
		//			break;
		//		}
		//	}

		//	if (valid)
		//	{
		//		rect.X      = int.Parse(parts[0]);
		//		rect.Y      = int.Parse(parts[1]);
		//		rect.Width  = int.Parse(parts[2]);
		//		rect.Height = int.Parse(parts[3]);
		//	}

		//	return rect;
		//}

		///// <summary>
		///// Gets the attribute that represents a System.Drawing.Rectangle object.
		///// </summary>
		///// <param name="root">The root.</param>
		///// <param name="name">The name.</param>
		///// <param name="defaultTect">The default tect.</param>
		///// <returns></returns>
		//public static Rectangle GetAttribute(this XElement root, string name, Rectangle defaultTect)
		//{
		//	string strValue = (string)root.Attributes(name).FirstOrDefault() ?? "0,0,0,0";
		//	return ParseRectCoords(strValue);
		//}

		///// <summary>
		///// Gets the attribute that represents a group of System.Drawing.Rectangle objects.
		///// </summary>
		///// <param name="root">The root.</param>
		///// <param name="name">The name.</param>
		///// <returns></returns>
		//public static List<Rectangle> GetAttribute(this XElement root, string name)
		//{
		//	string strValue = (string)root.Attributes(name).FirstOrDefault() ?? "0,0,0,0";
		//	strValue = strValue.Replace(" ", "");

		//	List<Rectangle> rects = new List<Rectangle>();
		//	string[] rectObjs = strValue.Split(';');

		//	foreach(string rectStr in rectObjs)
		//	{
		//		Rectangle rect = ParseRectCoords(rectStr);
		//		if (rect.X != -1)
		//		{
		//			rects.Add(rect);
		//		}
		//	}
		//	return rects;
		//}

		/// <summary>
		/// Gets the attribute that represents the type of the variable on the left side of 
		/// the "=" operator.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="root">The root.</param>
		/// <param name="name">The name.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns></returns>
		/// <exception cref="System.InvalidOperationException"></exception>
		public static T GetAttribute<T>(this XElement root, string name, T defaultValue)
		{
			T value = defaultValue;
			string strValue = (string)root.Attributes(name).FirstOrDefault() ?? defaultValue.ToString();
			if (value is string)
			{
				value = (T)(object)strValue;
			}
			else
			{
				var tryParse = typeof (T).GetMethod("TryParse", new [] {typeof(string), typeof(T).MakeByRefType()});
				if (tryParse == null)
				{
					throw new InvalidOperationException();
				}
				var parameters = new object[] {strValue, value};
				if ((bool)tryParse.Invoke(null, parameters))
				{
					value = (T)parameters[1];
				}
			}

			return value;
		}
 
    }
}
