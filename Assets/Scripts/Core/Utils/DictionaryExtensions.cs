using System;
using System.Collections.Generic;

namespace Core.Utils
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue, TKey2, TValue2>(this IDictionary<TKey, TValue> target, IDictionary<TKey2, TValue2> source) where TKey2:TKey where TValue2:TValue
        {
            if (source == null || target == null)
            {
                return;
            }

            foreach (var item in source)
            {
                if (!target.ContainsKey(item.Key))
                {
                    target.Add(item.Key,item.Value);
                }
            }
        }

        public static void CopyFrom<TKey, TValue>(this IDictionary<TKey, TValue> target, IDictionary<TKey, TValue> source)
        {
            target.Clear();

            if (source == null || source == target)
            {
                return;
            }

            foreach (var item in source)
            {
                target.Add(item.Key, item.Value);
            }
        }

        public static IDictionary<TKey, TValue> Convert<TKey, TValue, TPair>(this List<TPair> list,
            Func<TPair, TKey> keyConverter, Func<TPair, TValue> valueConverter)
        {
            var result = new Dictionary<TKey, TValue>();

            foreach (var item in list)
            {
                var key = keyConverter(item);
                var value = valueConverter(item);

                if (result.ContainsKey(key))
                {
                    result[key] = value;
                }
                else
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

        public static Dictionary<TKey2, TValue2> Convert<TKey, TValue, TKey2, TValue2>(this Dictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, TKey2> keyConverter, Func<TKey, TValue, TValue2> valueConverter)
        {
            var result = new Dictionary<TKey2, TValue2>();

            foreach (var item in dictionary)
            {
                var key = keyConverter(item.Key,item.Value);
                var value = valueConverter(item.Key,item.Value);

                if (result.ContainsKey(key))
                {
                    result[key] = value;
                }
                else
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

        public static Dictionary<TKey, TValue> Convert<TData, TKey, TValue>(this IEnumerable<TData> collection,
            Func<TData, TKey> keyConverter, Func<TData, TValue> valueConverter)
        {
            var result = new Dictionary<TKey, TValue>();

            foreach (var item in collection)
            {
                var key = keyConverter(item);
                var value = valueConverter(item);

                if (result.ContainsKey(key))
                {
                    result[key] = value;
                }
                else
                {
                    result.Add(key, value);
                }
            }

            return result;
        }

        public static List<TKey> GetKeys<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                return new List<TKey>();
            }

            return new List<TKey>(dictionary.Keys);
        }

        public static List<TValue> GetValues<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
            {
                return new List<TValue>();
            }

            return new List<TValue>(dictionary.Values);
        }

        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
        {
            var result = new Dictionary<TValue, TKey>();
            
            foreach (var item in dictionary)
            {
                if (item.Value != null && !result.ContainsKey(item.Value))
                {
                    result.Add(item.Value,item.Key);
                }
            }

            return result;
        }

        public static KeyValuePair<TKey, TValue> Find<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> predicate)
        {
            if (predicate == null)
            {
                return default(KeyValuePair<TKey, TValue>);
            }

            foreach (var item in dictionary)
            {
                if (predicate.Invoke(item.Key, item.Value))
                {
                    return item;
                }
            }
            
            return default(KeyValuePair<TKey, TValue>);
        }
        
        public static Dictionary<TKey, TValue> FindAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> predicate)
        {
            if (predicate == null)
            {
                return default(Dictionary<TKey, TValue>);
            }

            var result = new Dictionary<TKey, TValue>();

            foreach (var item in dictionary)
            {
                if (predicate.Invoke(item.Key, item.Value))
                {
                    result.Add(item.Key, item.Value);
                }
            }

            return result;
        }
        
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Dictionary<TKey, TValue> dictionaryToRemove)
        {
            foreach (var item in dictionaryToRemove)
            {
                if (dictionary.ContainsKey(item.Key))
                {
                    dictionary.Remove(item.Key);
                }
            }
        }
        
        public static void RemoveAll<TKey, TValue>(this Dictionary<TKey, TValue> dictionary,
            Func<TKey, TValue, bool> dictionaryToRemove)
        {
            if (dictionaryToRemove == null)
            {
                return;
            }

            foreach (var item in dictionary)
            {
                if (dictionaryToRemove(item.Key, item.Value))
                {
                    dictionary.Remove(item.Key);
                }
            }
        }
    }
}