using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;

namespace AlgorithmsOnStrings.Tests
{
    public abstract class BaseTests
    {
        const string input_ext = ".i";
        const string answer_ext = ".a";

        private static string CurrentPath()
        {
            return string.Format(@"{0}\..\..", AppDomain.CurrentDomain.BaseDirectory);
        }
        private static string CurrentTestPath(string location)
        {
            return string.Format(@"{0}\testfiles\{1}", CurrentPath(), location);
        }
        
        protected static void TestFromRelativeFilePath(string path, Func<IList<string>, IList<string>> getActual, bool exactOrder = true)
        {
            TestFromFilePath(CurrentTestPath(path), getActual, exactOrder);
        }

        protected static void TestFromFilePath(string path, Func<IList<string>, IList<string>> getActual, bool exactOrder = true)
        {
            var input = File.Exists(path + input_ext) ? File.ReadAllLines(path + input_ext) : File.ReadAllLines(path);
            var expected = File.ReadAllLines(path + answer_ext);
            var actual = getActual(input);

            Console.WriteLine();
            Console.WriteLine("[File {0}]", path);
            
            if (exactOrder)
            {
                for (var a = 0; a < actual.Count; a++)
                {   //Output Values
                    Console.WriteLine("{2}[{3}] {0} => {1}", expected[a], actual[a], (expected[a] == actual[a]) ? "" : "* ", a + 1);
                }
                for (var a = 0; a < actual.Count; a++)
                {
                    //Validate Values
                    actual[a].Should().Be(expected[a]);
                }
            }
            else
            {
                expected = expected.OrderBy(s => s).ToArray();
                actual = actual.OrderBy(s => s).ToArray();
                for (var a = 0; a < actual.Count; a++)
                {   //Output Values
                    Console.WriteLine("{2}[{3}] {0} => {1}", expected[a], actual[a], (expected[a] == actual[a]) ? "" : "* ", a + 1);
                }
                actual.ShouldAllBeEquivalentTo(expected);
            }
        }
    }
}
