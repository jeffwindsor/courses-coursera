using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures
{
    /// <summary>
    /// Uses Hash and Chaining
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class Set<TValue>
               where TValue : class, IEquatable<TValue>
    {
        private readonly List<TValue>[] _lists;
        private readonly Func<TValue, long> _hashFunction;

        public Set(long size, Func<TValue, long> hashFunction)
        {
            _lists = new List<TValue>[size];
            _hashFunction = hashFunction;
        }

        public bool Find(TValue value)
        {
            return (FindValue(value) != null);
        }

        public void Add(TValue value)
        {
            var l = GetList(value);
            var v = FindValue(value, l);
            if (v == null) l.Insert(0, value);
        }

        public void Remove(TValue value)
        {
            var l = GetList(value);
            var v = FindValue(value, l);
            if (v != null) l.Remove(value);
        }

        public List<TValue> GetList(long i)
        {
            return _lists[i] ?? (_lists[i] = new List<TValue>());
        }

        private List<TValue> GetList(TValue value)
        {
            var i = _hashFunction(value);
            return GetList(i);
        }

        private TValue FindValue(TValue value)
        {
            return FindValue(value, GetList(value));
        }

        private static TValue FindValue(TValue value, IEnumerable<TValue> list)
        {
            return (list == null) ? null : list.FirstOrDefault(o => o.Equals(value));
        }

        public override string ToString()
        {
            var results = from i in Enumerable.Range(0, _lists.Length)
                          select string.Format("[{0}] {1}", i, string.Join(" ", GetList(i)));
            return string.Join(Environment.NewLine, results);
        }
    }
}
