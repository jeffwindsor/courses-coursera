using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStructures.W3
{
    public class HashChains
    {
        const string NOT_FOUND = "no";
        const string FOUND = "yes";
        const string ADD = "add";
        const string FIND = "find";
        const string DEL = "del";
        const string CHECK = "check";

        public class Program
        {
            public static void Main(string[] args)
            {
                Process(HashChains.Process);
            }

            private static void Process(Func<string[], string[]> process)
            {
                var input = new List<string>();
                string s;
                while ((s = Console.ReadLine()) != null)
                {
                    input.Add(s);
                }

                foreach (var item in process(input.ToArray()))
                {
                    Console.WriteLine(item);
                }
            }
        }

        public static string[] Process(string[] inputs)
        {
            var chars = new[] { ' ' };
            var size = long.Parse(inputs[0]);
            var queryCount = int.Parse(inputs[1]);
            var queries = inputs.Skip(2).Take(queryCount).Select(l =>
            {
                var splits = l.Split(chars);
                return new Query(splits[0].ToLower(), splits[1]);
            });

            return Process(size, queries)
                .ToArray();
        }

        public static IEnumerable<string> Process(long size, IEnumerable<Query> queries)
        {
            var ht = new HashTable(size);
            var results = new List<string>();
            foreach (var query in queries)
            {
                //Console.WriteLine("[{0}] {1} => ",query.Command,query.Value);
                switch (query.Command)
                {
                    case ADD:
                        ht.Add(query.Value);
                        break;
                    case DEL:
                        ht.Remove(query.Value);
                        break;
                    case FIND:
                        results.Add(ht.Find(query.Value) ? FOUND : NOT_FOUND);
                        break;
                    case CHECK:
                        var i = long.Parse(query.Value);
                        var list = ht.GetList(i);
                        var result = string.Join(" ", list);
                        results.Add(result);
                        //Console.WriteLine(result);
                        //Console.WriteLine(ht);
                        break;
                    default:
                        throw new ArgumentException("Command Not Known");
                }
            }
            return results;
        }

        public class HashTable : Set<string>
        {
            const long p = 1000000007;
            const long x = 263;

            public HashTable(long size):base(size, o => HashFunction(size,o)){}

            private static long HashFunction(long size, string value)
            {
                long hash = 0;
                for (var i = value.Length - 1; i > -1; i--)
                {
                    hash = (hash*x + value[i])%p;
                }
                return hash % size;
                //return value.Select( (c,i) => (long)Math.Pow(x, i) * c).Sum() % p % size;
            }
        }

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
                if (v == null) l.Insert(0,value);
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
                return (list == null)? null: list.FirstOrDefault(o => o.Equals(value));
            }

            public override string ToString()
            {
                var results = from i in Enumerable.Range(0, _lists.Length)
                    select string.Format("[{0}] {1}",i, string.Join(" ", GetList(i)));
                return string.Join(Environment.NewLine, results);
            }
        }

        public class Query
        {
            public Query(string c, string value)
            {
                Command = c.ToLower();
                Value = value;
            }

            public string Command;
            public string Value;

            public override string ToString()
            {
                return string.Format("{0} {1}",Command,Value);
            }
        }
    }
}
