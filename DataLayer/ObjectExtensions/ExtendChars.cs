using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	public static class ExtendChars
	{
		/// <summary>
		/// Returns true if the ascii value represents a number
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public static bool IsAsciiNumeric(this char ch)
		{
			int chInt = (int)ch;
			return (chInt >= 48 && chInt <= 57);
		}
		/// <summary>
		/// Returns true is the ascii value represents a letter
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public static bool IsAsciiAlpha(this char ch)
		{
			int chInt = (int)ch;
			return ((chInt >= 65 && chInt <= 90) || (chInt >=97 && chInt <=122));
		}
		/// <summary>
		/// Returns true if the ascii value represents a non-printable character
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public static bool IsAsciiNonPrintable(this char ch)
		{
			int chInt = (int)ch;
			return (chInt >= 0 && chInt <= 31 || chInt == 127);
		}
		/// <summary>
		/// Returns true if the ascii value represents a character that is not alpnumeric and non-printable.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		public static bool IsAsciiOther(this char ch)
		{
			return (!ch.IsAsciiNumeric() && 
				   !ch.IsAsciiAlpha() &&
				   !ch.IsAsciiNonPrintable());
		}
	}
}
