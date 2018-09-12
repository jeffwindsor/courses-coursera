using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AlgorithmsOnGraphs.Tests
{
    [TestFixture]
    public class MinPriorityQueueTests
    {
        [Test]
        public void DequeuesByPriority()
        {
            var expected = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var priorities = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            var q = CreateQueue(priorities);
            AssertDequeuesByPriority(q, expected);
        }

        [Test]
        public void DequeuesByPriorityAfterPriorityChanges()
        {
            var expected = new[] {0, 8, 7, 6, 5, 4, 3, 2, 1};
            var priorities = new[] { 0, 10, 20, 30, 40, 50, 60, 70, 80 };

            var q = CreateQueue(priorities);
            q.ChangePriority(8, 5);
            q.ChangePriority(7, 15);
            q.ChangePriority(6, 25);
            q.ChangePriority(5, 35);
            q.ChangePriority(4, 45);
            q.ChangePriority(3, 55);
            q.ChangePriority(2, 65);
            q.ChangePriority(1, 75);

            AssertDequeuesByPriority(q, expected);
        }


        public void AssertDequeuesByPriority(MinPriorityQueue<int> q, int[] expected)
        {
            var actual = DequeueAll(q);
            actual.Should().Equal(expected);
        }

        private static int[] DequeueAll(MinPriorityQueue<int> queue)
        {
            var result = new List<int>();

            while (!queue.IsEmpty())
            {
                Console.WriteLine("Queue: {0}", queue);
                var dq = queue.Dequeue();
                Console.WriteLine(" Dequeue: {0}", dq);
                result.Add(dq);
            }

            return result.ToArray();
        }

        private static MinPriorityQueue<int> CreateQueue(int[] priorities)
        {
            var queue = new MinPriorityQueue<int>(priorities.Length, int.MaxValue);
            for (var i = 0; i < priorities.Length; i++)
            {
                queue.Enqueue(i, priorities[i]);
            }
            return queue;
        }
    }
}
