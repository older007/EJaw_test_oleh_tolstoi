using System;
using System.Collections.Generic;

namespace Core.Utils
{
    public static class CollectionExtensions
    {
        public static T Random<T>(this List<T> collection)
        {
            if (collection.Count <= 0)
            {
                return default(T);
            }

            return collection[UnityEngine.Random.Range(0, collection.Count)];
        }

        public static List<T> AddAny<T>(this List<T> collection, List<T> other, Predicate<T> predicate)
        {
            var result = new List<T>(collection);
            result.AddRange(other.FindAll(predicate));
            
            return result;
        }

        public static List<List<T>> FindDuplicates<T>(this List<T> collection)
        {
            var result = new Dictionary<T,List<T>>();

            foreach (var item in collection)
            {
                if (result.ContainsKey(item))
                {
                    continue;
                }

                var duplicates = collection.FindAll(fa => fa.Equals(item));

                if (duplicates.Count > 1)
                {
                    result.Add(item, duplicates);
                }
            }

            return result.GetValues();
        }
    }
}