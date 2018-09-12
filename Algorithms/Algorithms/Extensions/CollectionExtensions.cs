using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    internal static class CollectionExtensions
    {
        public static void ForEachIndex(this int[] source, Action<int[], int> action)
        {
            for (var i = 0; i < source.Length; ++i)
            {
                action(source, i);
            }
        }
    }
}
