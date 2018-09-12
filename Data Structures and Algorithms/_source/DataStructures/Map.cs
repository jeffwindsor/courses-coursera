using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// Uses Hash and Chaining
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class Map<TKey, TValue> 
        where TValue: class, IEquatable<TValue>
        where TKey: class
    {
        private readonly List<KeyValue>[] _lists;
        private readonly Func<TValue, int> _hashFunction;
        public Map(int size, Func<TValue, int> hashFunction)
        {
            _lists = new List<KeyValue>[size];
            _hashFunction = hashFunction;
        }

        public bool HasKey(TValue value)
        {
            var l = GetList(value);
            return l.Any(kv => kv.Value.Equals(value));
        }

        public TKey GetKey(TValue value)
        {
            var found = FindKeyValue(value);
            return found?.Key;
        }

        public void Set(TKey key, TValue value)
        {
            var l = GetList(value);
            var found = FindKeyValue(value, l);
            if (found == null) l.Add(new KeyValue(key, value));
            else if (found.Key != key) found.Key = key;
        }
        
        private List<KeyValue> GetList(TValue value)
        {
            return _lists[_hashFunction(value)];
        }
        
        private KeyValue FindKeyValue(TValue value)
        {
            return FindKeyValue(value, GetList(value));
        }
        private static KeyValue FindKeyValue(TValue value, IEnumerable<KeyValue> list)
        {
            return list.FirstOrDefault(kv => kv.Value.Equals(value));
        }
        private class KeyValue
        {
            public KeyValue(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
            public TKey Key { get; set; }
            public TValue Value { get; }
        }
    }
}
