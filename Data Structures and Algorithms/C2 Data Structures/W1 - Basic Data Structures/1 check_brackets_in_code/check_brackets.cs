using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var input = Console.ReadLine() ?? string.Empty;
            Console.WriteLine(Bracket.Check(input));
        }
    }
    
    public class Bracket
    {
        public char Value { get; set; }
        public int Index { get; set; }

        public static string Check(string input)
        {
            var stack = new Stack<Bracket>();
            foreach (var b in input.Select((v, i) => new Bracket { Value = v, Index = i + 1 }))
            {
                if (b.IsOpen()) { stack.Push(b); }
                else if (b.IsClose())
                {
                    if(!stack.Any() || !stack.Pop().IsMatch(b))
                        return b.Index.ToString();
                }
            
            }
            return (stack.Any())
                ? stack.Pop().Index.ToString()
                : "Success";
        }

        public bool IsOpen()
        {
            return Value == '[' || Value == '{' || Value == '(';
        }
        public bool IsClose()
        {
            return Value == ']' || Value == '}' || Value == ')';
        }
        public bool IsMatch(Bracket b)
        {
            return IsMatch(b.Value);
        }
        public bool IsMatch(char c)
        {
            return (Value == '[' && c == ']')
                   || (Value == '{' && c == '}')
                   || (Value == '(' && c == ')');
        }
    }

}
