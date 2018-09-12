using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W2
{
    public class BwtInvert
    {
        //public static void Main(string[] args)
        //{
        //    string s;
        //    var inputs = new List<string>();
        //    while ((s = Console.ReadLine()) != null)
        //        inputs.Add(s);

        //    foreach (var result in Answer(inputs.ToArray()))
        //        Console.WriteLine(result);
        //}

        public static IList<string> Answer(IList<string> inputs)
        {
            var input = inputs.First();
            var answer = BurrowsWheelerInversion(input);
            return new[] { answer };
        }
        public static string BurrowsWheelerInversion(string input)
        {
            var first = input.OrderBy(c=>c).ToArray();
            var last = input.ToCharArray();

            var indexes = first.ByCharNumber();
            var numbers = last.ToNumbers();

            var sb = new System.Text.StringBuilder();
            var index = 0;
            do
            {
                sb.Append(first[index]);
                index = indexes.NextIndex(last[index], numbers[index]);
            } while (index != 0);
            return new string(sb.ToString().Reverse().ToArray());
        }
    }

    public static class BwtInvertExtensions
    {
        public static int[] ToNumbers(this char[] input)
        {
            var counts = Enumerable.Range(0, 27).Select(_ => -1).ToArray();
            var result = input.Select(c =>
            {
                var i = AlphaIndex(c);
                counts[i] += 1;
                return counts[i];
            });
            return result.ToArray();
        }
        public static List<int>[] ByCharNumber(this char[] input)
        {
            var result = Enumerable.Range(0, 27).Select(_ => new List<int>()).ToArray();

            for (int i = 0; i < input.Length; i++)
            {
                result[AlphaIndex(input[i])].Add(i);
            }
            return result;
        }

        public static int NextIndex(this List<int>[] cns, char c, int n)
        {
            return cns[AlphaIndex(c)][n];
        }
        private static readonly int AlphaFloor = Convert.ToInt32('a') - 1;
        private static int AlphaIndex(char c)
        {
            return (c == '$') ? 0 : Convert.ToInt32(Char.ToLower(c)) - AlphaFloor;
        }
    }
}
