using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W2
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        Process(MergingTables.Answer);
    //    }
    //    private static void Process(Func<string[], string[]> process)
    //    {
    //        var input = new List<string>();
    //        string s;
    //        while ((s = Console.ReadLine()) != null)
    //        {
    //            input.Add(s);
    //        }

    //        foreach (var item in process(input.ToArray()))
    //        {
    //            Console.WriteLine(item);
    //        }
    //    }
    //}

    public class MergingTables
    {
        public static IList<string> Answer(IList<string> inputs)
        {
            var chars = new[] { ' ' };
            var splits0 = inputs[0].Split(chars);

            var tableCount = int.Parse(splits0[0]);
            var queryCount = int.Parse(splits0[1]);
            var tableRowCounts = inputs[1].Split(chars)
                .Select(int.Parse)
                .ToArray();

            var queries = inputs.Skip(2).Take(queryCount).Select(l => {
                var splits1 = l.Split(chars);
                return new Query (int.Parse(splits1[0]), int.Parse(splits1[1]));
                });

            return Process(tableRowCounts, queries)
                .Select(q => q.ToString())
                .ToArray();
        }

        public static IEnumerable<int> Process(int[] tableRowCounts, IEnumerable<Query> queries)
        {
            var results = new List<int>();
            var set = new Tables(tableRowCounts);

            Console.WriteLine(set);
            foreach (var q in queries)
            {
                Console.WriteLine(q);                
                set.Union(q.Destination, q.Source);
                Console.WriteLine(set);

                results.Add(set.MaxCount);
                Console.WriteLine(results.Last());

            }
            return results;
        }

        public class Query
        {
            public Query(int d, int s) { Destination = d - 1; Source = s - 1; }
            public int Destination;
            public int Source;
            public override string ToString()
            {
                return string.Format("{0} <== {1}", Destination, Source);
            }
        }

        public class Tables : DisjointSets
        {
            private int[] _counts;
            private int _maxcount;
            public Tables(int[] counts): base(counts.Length)
            {
                _counts = counts;
                _maxcount = counts.Max();
            }
            public int MaxCount { get { return _maxcount; } }

            protected override void Merge(int destination, int source)
            {
                _counts[destination] += _counts[source];
                _counts[source] = 0;

                _maxcount = Math.Max(_maxcount, _counts[destination]);
            }
            public override string ToString()
            {
                return string.Format("{2}{0}Rows:    [{1}]", 
                    Environment.NewLine,
                    string.Join(", ", _counts.Select(i => i.ToString())),
                    base.ToString());
            }
        }

        public class DisjointSets
        {
            private int[] _parent;
            private int[] _rank;
            public DisjointSets(int size)
            {
                var range = Enumerable.Range(0, size);
                _rank = range.Select(_ => 0).ToArray();
                _parent = range.Select(i => i).ToArray();
            }

            protected virtual void Merge(int destination, int source) { }
            public void MakeSet(int i) { _parent[i] = i; }
            public int Find(int i)
            {
                if (i != _parent[i])
                    _parent[i] = Find(_parent[i]);
                return _parent[i];
            }            
            public void Union(int a, int b)
            {
                a = Find(a);
                b = Find(b);

                if (a == b) return;
                //smaller depth tree under larger depth tree
                if (_rank[a] > _rank[b])
                {
                    _parent[b] = a;
                    Merge(a, b);
                }
                else
                {
                    _parent[a] = b;
                    Merge(b, a);
                    if (_rank[a] == _rank[b]) _rank[b] += 1;
                }
            }
            public override string ToString()
            {
                return string.Format("Parents: [{1}]{0}Ranks:   [{2}]",
                    Environment.NewLine, 
                    string.Join(", ", _parent.Select(i => i.ToString())),
                    string.Join(", ", _rank.Select(i => i.ToString())));
            }
        }
    }
}
