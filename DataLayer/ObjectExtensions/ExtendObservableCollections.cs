using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectExtensions
{
	/// <summary>
	/// 
	/// </summary>
	public static class ExtendObservableCollections
	{
		/// <summary>
		/// Adds the list of items to this collection. There is no check for uniqueness 
		/// other than to make sure the object itsel is not already part of the collection.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="collection">The collection.</param>
		/// <param name="list">The list.</param>
		public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> list)
		{
			foreach(T item in list)
			{
				collection.Add(item);
			}
		}
	}
}
