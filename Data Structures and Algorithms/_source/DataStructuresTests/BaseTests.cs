using System;
using System.Collections.Generic;
using System.IO;
using FluentAssertions;

namespace DataStructures.Tests
{
    public abstract class BaseTests
    {
        const string input_ext = ".i";
        const string answer_ext = ".a";

        private static string CurrentPath()
        {
            return string.Format(@"{0}\..\..", System.AppDomain.CurrentDomain.BaseDirectory);
        }
        private static string CurrentTestPath(string location)
        {
            return string.Format(@"{0}\testfiles\{1}", CurrentPath(), location);
        }
        
        protected static void TestFromRelativeFilePath(string path, Func<IList<string>, IList<string>> getActual)
        {
            TestFromFilePath(CurrentTestPath(path), getActual);
        }

        protected static void TestFromFilePath(string path, Func<IList<string>, IList<string>> getActual)
        {
            var input = File.ReadAllLines(path + input_ext);
            var expected = File.ReadAllLines(path + answer_ext);
            var actual = getActual(input);

            Console.WriteLine();
            Console.WriteLine("[File {0}]", path);
            //actual.Count.Should().Be(expected.Length);
            for (var a = 0; a < actual.Count; a++)
            {   //Output Values
                Console.WriteLine("{2}[{3}] {0} => {1}", expected[a], actual[a], (expected[a]==actual[a])?"":"* ", a+1);
            }
            for (var a = 0; a < actual.Count; a++)
            {   //Validate Values
                actual[a].Should().Be(expected[a]);
            }
        }
        
        protected static void WriteTestFiles(string name, string location, IEnumerable<string> lines, IEnumerable<string> answerLines)
        {
            var l = CurrentTestPath(location);
            File.WriteAllLines(l + @"\" + name + input_ext, lines);
            File.WriteAllLines(l + @"\" + name + answer_ext, answerLines);
        }
    }
}
