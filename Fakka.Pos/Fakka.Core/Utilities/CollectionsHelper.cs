using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Fakka.Core.Utilities
{
    public static class CollectionsHelper
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> range)
        {
            if (collection == null)
                collection = new ObservableCollection<T>();

            foreach (var item in range)
                collection.Add(item);
        }

        public static void RemoveRange<T>(this ICollection<T> collection, IEnumerable<T> range)
        {
            if (collection == null)
                collection = new ObservableCollection<T>();

            for(int i=0; i < range.Count(); i++)
                collection.Remove(range.ElementAt(i));

        }

        public static void RemoveRange<T>(this ICollection<T> collection, Func<T, bool> exp)
        {
            if (collection == null)
                collection = new ObservableCollection<T>();

            var range = collection.Where(exp).ToArray();
            int rangeLength = range.Length;

            for (int i = 0; i < rangeLength; i++)
                collection.Remove(range[i]);

        }

    }
}
