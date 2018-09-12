using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AlgorithmsOnStrings.Tests
{
    [TestFixture]
    public class Quizzes
    {
        [Test]
        public void Week4_Q1()
        {
            var actual = SuffixArray.SortCharacters("AACGATAGCGGTAGA$", SuffixArray.NucleotideAlphabet);
            Console.WriteLine(string.Join(",",actual.Select(i=>i.ToString())));
        }

        [Test]
        public void Week4_Q2()
        {
            var s = "AACGATAGCGGTAGA$";
            var order = SuffixArray.SortCharacters(s, SuffixArray.NucleotideAlphabet);
            var actual = SuffixArray.ComputeCharClasses(s, order);
            Console.WriteLine(string.Join(",", actual.Select(i => i.ToString())));
        }

        [Test]
        public void Week4_Q4()
        {
            var s = "AACGATAGCGGTAGA$";
            var order = SuffixArray.SortCharacters(s, SuffixArray.NucleotideAlphabet);
            var classes = SuffixArray.ComputeCharClasses(s, order);
            var actual = SuffixArray.SortDoubled(s, 1, order, classes);
            Console.WriteLine(string.Join(",", actual.Select(i => i.ToString())));
        }

        [Test]
        public void Week4_Q5()
        {
            var s = "AACGATAGCGGTAGA$";
            var order = SuffixArray.SortCharacters(s, SuffixArray.NucleotideAlphabet);
            var classes = SuffixArray.ComputeCharClasses(s, order);
            
            var l = 1;
            while (l < 3)
            {
                order = SuffixArray.SortDoubled(s, l, order, classes);
                classes = SuffixArray.UpdateClasses(order, classes, l);
                l = 2 * l;
            }

            Console.WriteLine(string.Join(",", classes.Select(i => i.ToString())));
        }

        [Test]
        public void Week4_Q6()
        {
            var s = "AACGATAGCGGTAGA$";
            var order = SuffixArray.SortCharacters(s, SuffixArray.NucleotideAlphabet);
            var classes = SuffixArray.ComputeCharClasses(s, order);

            var l = 1;
            while (l < s.Length)
            {
                order = SuffixArray.SortDoubled(s, l, order, classes);
                classes = SuffixArray.UpdateClasses(order, classes, l);
                l = 2 * l;
            }

            Console.WriteLine(string.Join(",", order.Select(i => i.ToString())));
        }

        
    }
}
