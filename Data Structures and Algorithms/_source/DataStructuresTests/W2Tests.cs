using System.Linq;
using DataStructures.W2;
using NUnit.Framework;

namespace DataStructures.Tests
{
    [TestFixture]
    public class W2Tests : BaseTests
    {
        const string path = @"W2 - Priority Queues and Disjoint Sets\";
        const string location_jobqueue = "2 job_queue";
        [Test]
        public void MakeHeapTests()
        {
            //TestDirectory(path + "1 make_heap", MakeHeap.Answer);
        }

        [Test]
        public void JobQueueTests()
        {
            //TestDirectory(path + location_jobqueue, JobQueue.Answer);
        }

        [Test]
        public void MergingTablesTests()
        {
            //TestDirectory(path + "3 merging_tables", MergingTables.Answer);
        }


        [Test]
        public void QuizQ1()
        {
            var queries = new[]
            {
                new MergingTables.Query(2,10),
                new MergingTables.Query(7,5),
                new MergingTables.Query(6,1),
                new MergingTables.Query(3,4),
                new MergingTables.Query(5,11),
                new MergingTables.Query(7,8),
                new MergingTables.Query(7,3),
                new MergingTables.Query(12,2),
                new MergingTables.Query(9,6)
            };
            MergingTables.Process(new[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, queries);
        }

        [Test]
        public void QuizQ2()
        {
            var n = 40;
            var set = new MergingTables.DisjointSets(n);
            for (int i = 0; i < n; i++)
            {
                set.MakeSet(i);
            }
            for (int i = 0; i < n - 1; i++)
            {
                set.Union(i, i+1);
            }
            System.Console.WriteLine(set);
        }

        [Test]
        public void QuizQ3()
        {
            var set = new MergingTables.DisjointSets(60);
            for (int i = 0; i < 60; i++) { set.MakeSet(i);}
            for (int i = 0; i < 30; i++) { set.Union(i, 2*1); }
            for (int i = 0; i < 20; i++) { set.Union(i, 3*i); }
            for (int i = 0; i < 12; i++) { set.Union(i, 5*i); }
            for (int i = 0; i < 60; i++) { set.Find(i); }
            
            System.Console.WriteLine(set);
        }

        const long JobQueueTests_max_t = 1000000000;
        private void JobQueueTestsGenerateLargeNumberOfLargeTimes(int id)
        {
            const int threadCount = 1;
            const int jobCount = 100000;
            var jobs = Enumerable.Range(0, jobCount)
                .Select(_ => JobQueueTests_max_t)
                .Select(i => i.ToString());
            var joblines = new[] { string.Format("{0} {1}", threadCount, jobCount), string.Join(" ", jobs) };
            var answerlines = Enumerable.Range(0, jobCount)
                .Select( i => i * JobQueueTests_max_t)
                .Select(i => string.Format("0 {0}", i));

            WriteTestFiles(id.ToString(), path + location_jobqueue, joblines, answerlines);
        }
        
    }
}