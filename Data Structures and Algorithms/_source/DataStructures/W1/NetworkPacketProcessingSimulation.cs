using System;
using System.Collections.Generic;
using System.Linq;

namespace DataStructures.W1
{
    public class NetworkPacketProcessingSimulation
    {
        public static char[] splitOn = new[] { ' ' };

        public static IList<string> Answer(IList<string> inputs)
        {
            var line0 = inputs[0].Split(splitOn);
            var size = int.Parse(line0[0]);
            var count = int.Parse(line0[1]);

            if (count == 0)
                return new string[] { };

            var requests = inputs
                .Skip(1)
                .Take(count)
                .Select(s => new Request(s.Split(splitOn)))
                .ToArray();

            return GetResponses(size, requests)
                .Select(p => p.ToString())
                .ToArray();
        }

        public static IEnumerable<int> GetResponses(int bufferSize, ICollection<Request> requests)
        {
            var b = new Buffer(bufferSize);
            var results = requests.Select(r => b.Process(r));
            return results;
        }
    }
    public class Buffer
    {
        const int DROPPED = -1;
        private int _size;
        private int _lastProcessedTime = 0;
        private Queue<int> _queue = new Queue<int>();

        public Buffer(int size)
        {
            _size = size;
        }
        public int Process(Request request)
        {
            //Dequeue all processed before arrival time
            while (_queue.Count > 0 && _queue.Peek() <= request.ArrivalTime)
                _queue.Dequeue();

            //if buffer full => -1 else queue => process time
            if (_queue.Count < _size)
            {
                var start = Math.Max(request.ArrivalTime, _lastProcessedTime);
                _lastProcessedTime = start + request.ProcessTime;
                _queue.Enqueue(_lastProcessedTime);
                return start;
            }
            else
            {
                return DROPPED;
            }
        }
    }
    public class Request
    {
        public Request(string[] input)
        {
            ArrivalTime = int.Parse(input[0]);
            ProcessTime = int.Parse(input[1]);
        }
        public int ArrivalTime { get; set; }
        public int ProcessTime { get; set; }
    }
}
