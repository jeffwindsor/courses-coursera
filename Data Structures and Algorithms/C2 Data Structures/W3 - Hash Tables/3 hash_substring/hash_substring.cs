using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W3
{
    public class HashSubstring
    {
        public class Program
        {
            public static void Main(string[] args)
            {
                Process(HashSubstring.Process);
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
            var pattern = inputs[0].Trim().ToCharArray();
            var text = inputs[1].Trim().ToCharArray();

            var indexes = RabinKarp(pattern, text);
            var results = string.Join(" ", indexes.Select(i => i.ToString()));

            return new[] { results };
        }

        public static IEnumerable<int> RabinKarp(char[] pattern, char[] text)
        {
            const long p = 1000000007;
            const long x = 263; //random from 1 ot p-1

            var patternHash = PolyHashFunction(pattern, 0, pattern.Length - 1, p, x);
            var hashes = Hashes(text, pattern.Length, p, x);

            var result = new List<int>();
            var loopLength = text.Length - pattern.Length;
            for (int i = 0; i <= loopLength; i++)
            {
                if (patternHash == hashes[i] && AreEqual(text, i, pattern))
                {
                    result.Add(i);
                }
            }
            return result;
        }
        private static bool AreEqual(char[] value, int valueStartIndex, char[] other)
        {
            for (int i = 0; i < other.Length; i++)
            {
                if (value[valueStartIndex + i] != other[i])
                    return false;
            }
            return true;
        }

        private static long[] Hashes(char[] text, int patternLength, long p, long x)
        {
            var lastIndex = text.Length - patternLength;
            var hashes = new long[lastIndex + 1];
            long y = 1;
            for (int i = 1; i <= patternLength; i++)
            {
                y = SafeMod(y * x, p);
            }

            hashes[lastIndex] = PolyHashFunction(text, lastIndex, text.Length - 1, p, x);
            for (int i = lastIndex - 1; i > -1; i--)
            {
                long v = (x * hashes[i + 1]) + text[i] - (y * text[i + patternLength]);
                hashes[i] = SafeMod(v, p);
            }
            return hashes;
        }
        private static long SafeMod(long value, long mod)
        {
            return ((value % mod) + mod) % mod;
        }

        private static long PolyHashFunction(char[] value, int startIndex, int endIndex, long p, long x)
        {
            long hash = 0;
            for (var i = endIndex; i >= startIndex; i--)
            {
                hash = (hash * x + value[i]) % p;
            }
            return hash;
        }
    }
}
