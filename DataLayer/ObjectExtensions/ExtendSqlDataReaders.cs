using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtendSqlDataReaders
	{
		/// <summary>
		/// Gets the column string value. It is assumed that the column ordinal has already been verified to exist.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static string GetStringOrDefault(this SqlDataReader reader, int ordinal, string defaultValue)
		{
			string value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlString(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column int32 value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static Int32 GetInt32OrDefault(this SqlDataReader reader, int ordinal, Int32 defaultValue)
		{
			Int32 value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlInt32(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column int32 value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static Int64 GetInt64OrDefault(this SqlDataReader reader, int ordinal, Int64 defaultValue)
		{
			Int64 value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlInt64(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column DateTime value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static DateTime GetDateTimeOrDefault(this SqlDataReader reader, int ordinal, DateTime defaultValue)
		{
			DateTime value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlDateTime(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column double value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static double GetDoubleOrDefault(this SqlDataReader reader, int ordinal, double defaultValue)
		{
			double value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlDouble(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column decimal value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">The default value.</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static decimal GetDecimalOrDefault(this SqlDataReader reader, int ordinal, decimal defaultValue)
		{
			decimal value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlDecimal(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Gets the column bool value.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="ordinal">The ordinal.</param>
		/// <param name="defaultValue">if set to <c>true</c> [default value].</param>
		/// <returns>The value in the column, or the default if null</returns>
		public static bool GetBoolOrDefault(this SqlDataReader reader, int ordinal, bool defaultValue)
		{
			bool value = defaultValue;
			if (!reader.IsDBNull(ordinal))
			{
				value = reader.GetSqlBoolean(ordinal).Value;
			}
			return value;
		}

		/// <summary>
		/// Clears the specified row.
		/// </summary>
		/// <param name="row">The row.</param>
		/// <exception cref="System.Exception"></exception>
		public static void Clear(this DataRow row)
		{
			// for each column in the schema
			for (int i = 0; i < row.Table.Columns.Count; i++)
			{
				// get the column
				DataColumn column = row.Table.Columns[i];
				// if the column doesn't have a default value
				if (column.DefaultValue != null)
				{
					if (!column.ReadOnly)
					{
						// Based on the data type of the column, set an appropriate 
						// default value. Since we're only dealing with intrinsic 
						// types, we can derive a kind of shortcut for the type name,
						// thus making our switch statemenbt a bit shorter.
						switch (column.DataType.Name.ToLower().Substring(0,3))
						{
							case "str": case "cha":
								row[i] = "";
								break;
							case "int": case "uin": case "sho": case "byt": 
							case "sby": case "dec": case "dou": case "sin":
								row[i] = 0;
								break;
							case "boo":
								row[i] = false;
								break;
							case "dat":
								row[i] = new DateTime(0);
								break;
							case "obj": default :
								row[i] = DBNull.Value;
								break;
						}
					}
					else
					{
						throw new Exception(string.Format("Column {0} is read-only.", i));
					}
				}
				// otherwise, set the column to its default value
				else
				{
					row[i] = column.DefaultValue;
				}
			}
		}
	}
}
