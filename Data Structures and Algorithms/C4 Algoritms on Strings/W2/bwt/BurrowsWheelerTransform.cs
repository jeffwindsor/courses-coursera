using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmsOnStrings.W1
{
    public class Bwt
    {
        public static void Main(string[] args)
        {
            string s;
            var inputs = new List<string>();
            while ((s = Console.ReadLine()) != null)
                inputs.Add(s);

            foreach (var result in Answer(inputs.ToArray()))
                Console.WriteLine(result);
        }

        public static IList<string> Answer(IList<string> inputs)
        {
            var input = inputs.First();
            var answer = BurrowsWheelerTransform(input);
            return new[] { answer };
        }

        public static string BurrowsWheelerTransform(string input)
        {
            var length = input.Length;
            var cycles = Enumerable.Range(0, length)
                .Select(i => input.Substring(i) + input.Substring(0, i))
                .OrderBy(s => s);
            var lastLetters = cycles.Select(s => s.Substring(length - 1));
            var result = string.Join("", lastLetters);

            return result;
        }
    }
}
