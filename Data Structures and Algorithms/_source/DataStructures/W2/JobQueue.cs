using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W2
{
    //public class Program
    //{
    //    public static void Main(string[] args)
    //    {
    //        Process(JobQueue.Answer);
    //    }
    //    private static void Process(Func<string[], string[]> process)
    //    {
    //        var input = new List<string>();
    //        string s;
    //        while ((s = Console.ReadLine()) != null)
    //        {
    //            input.Add(s);
    //        }

    //        foreach (var item in process(input.ToArray()))
    //        {
    //            Console.WriteLine(item);
    //        }
    //    }
    //}

    public class JobQueue
    {
        public static IList<string> Answer(IList<string> inputs)
        {
            var chars = new[] { ' ' };
            var splits0 = inputs[0].Split(chars);
            var threadCount = int.Parse(splits0[0]);
            var jobCount = int.Parse(splits0[1]);

            var jobStrings = inputs[1].Split(chars);
            if (jobStrings.Length != jobCount)
                jobStrings = jobStrings.Take(jobCount).ToArray();
            var jobs = jobStrings.Select(long.Parse).ToArray();
      
            return AssignJobs(threadCount, jobs)
                .Select(pj => string.Format("{0} {1}", pj.ThreadId, pj.StartOfProcessing))
                .ToArray();
        }

        public static IEnumerable<Job> AssignJobs(int threadCount, long[] jobs)
        {
            long currentTime = 0;
            var queue = new MinBinaryHeap<Job>(threadCount);
            var initials = new Queue<int>(Enumerable.Range(0, threadCount));

            //Process Results
            var results = new List<Job>();
            int id;
            foreach (var processingTime in jobs)
            {
                var q = queue.Max();
                if (initials.Count > 0 && (q == null || q.EndOfProcessing > currentTime))
                {
                    //Process job on unused thread
                    //Do not Dequeue max since it does not exist or has not finished processing yet
                    id = initials.Dequeue();
                }
                else
                {
                    //Dequeue Max and use that thread to process new job
                    queue.ExtractMax();
                    currentTime = q.EndOfProcessing;
                    id = q.ThreadId;
                }
                //adding job now since reporting on start time not end of processing time
                var job = new Job(id, currentTime, processingTime);
                results.Add(job);
                queue.Insert(job);
            }

            return results;
        }

        public class Job : IComparable<Job>
        {
            public Job(int id, long currentTime, long procesingTime)
            {
                ThreadId = id;
                StartOfProcessing = currentTime;
                ProcessingTime = procesingTime;
                EndOfProcessing = StartOfProcessing + ProcessingTime;
            }
            public int ThreadId { get; private set; }
            public long StartOfProcessing { get; private set; }
            public long ProcessingTime { get; private set; }
            public long EndOfProcessing { get; private set; }

            public int CompareTo(Job other)
            {
                //EndOfProcessing of processin then thread id
                return (EndOfProcessing == other.EndOfProcessing)
                    ? ThreadId.CompareTo(other.ThreadId)
                    : EndOfProcessing.CompareTo(other.EndOfProcessing);
            }
        }

        private class MinBinaryHeap<T> : BinaryHeap<T> where T : IComparable<T>
        {
            public MinBinaryHeap(int size) : base(size) { }

            public override int SiftUp(int i)
            {
                var swapId = i;
                while (i > 0 && Value(ParentId(i)).CompareTo(Value(i)) > 0)
                {
                    swapId = ParentId(i);
                    Swap(i, swapId);
                    i = swapId;
                }
                return swapId;
            }

            public override int SiftDown(int i)
            {
                var maxIndex = i;

                var r = RightChildId(i);
                if (r < _size && Value(r).CompareTo(Value(maxIndex)) < 0) maxIndex = r;

                var l = LeftChildId(i);
                if (l < _size && Value(l).CompareTo(Value(maxIndex)) < 0) maxIndex = l;

                if (i != maxIndex)
                {
                    Swap(i, maxIndex);
                    return SiftDown(maxIndex);
                }
                return maxIndex;
            }
        }

        private abstract class BinaryHeap<T>
        {
            protected int _maxSize;
            protected int _size;
            private T[] H;

            protected BinaryHeap(int size   )
            {
                H = new T[size];
                _maxSize = size;
                _size = 0;
            }

            protected int ParentId(int i) { return ((i - 1) / 2); }
            protected int LeftChildId(int i) { return 2 * i + 1; }
            protected int RightChildId(int i) { return 2 * i + 2; }

            protected T Value(int i) { return H[i]; }

            public int Size {
                get { return _size; }
            }
            public abstract int SiftUp(int i);

            public abstract int SiftDown(int i);

            public void Insert(T p)
            {
                if (_size == _maxSize) throw new ArgumentOutOfRangeException();

                _size += 1;
                H[_size - 1] = p;
                SiftUp(_size - 1);
            }

            public T Max()
            {
                return H[0];
            }
            public void ExtractMax()
            {
                var result = H[0];
                H[0] = H[_size - 1];
                _size -= 1;
                SiftDown(0);
            }

            //public void Remove(int i)
            //{
            //    H[i] = int.MaxValue;
            //    SiftUp(i);
            //    ExtractMax();
            //}

            //public void ChangePriority(int i, T p)
            //{
            //    var q = H[i];
            //    H[i] = p;
            //    if (p > q)
            //        SiftUp(i);
            //    else
            //        SiftDown(i);

            //}

            protected void Swap(int id1, int id2)
            {
                var temp = H[id2];
                H[id2] = H[id1];
                H[id1] = temp;
            }
        }
    }

}
