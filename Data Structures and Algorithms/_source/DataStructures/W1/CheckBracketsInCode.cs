using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W1
{
    public class CheckBracketsInCode
    {
        public static IList<string> Answer(IList<string> inputs)
        {
            var brackets = inputs[0].Select((v, i) => new Bracket {Value = v, Index = i + 1});
            return new[] { Check(brackets)};
        }

        public static string Check(IEnumerable<Bracket> brackets)
        {
            var stack = new Stack<Bracket>();
            foreach (var b in brackets)
            {
                if (b.IsOpen())
                {
                    stack.Push(b);
                }
                else if (b.IsClose())
                {
                    if (!stack.Any() || !stack.Pop().IsMatch(b))
                        return b.Index.ToString();
                }

            }
            return (stack.Any())
                ? stack.Pop().Index.ToString()
                : "Success";
        }
    }

    public class Bracket
    {
        public char Value { get; set; }
        public int Index { get; set; }

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
