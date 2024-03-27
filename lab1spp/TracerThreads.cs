using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab1spp
{
    public class TracerThreadsContent
    {
        public int Id { get; set; }
        public long Time { get; set; }
        public TraceResult TraceResult { get; set; } = new();
    }

    public class TracerThreads
    {
        private int threadCount;
        private long time;
        private List<Thread> threads;
        private List<Tracer> traces;
        private List<TraceResult> traceResult;
        private List<TracerThreadsContent> tracerThreadsContents;
        public TracerThreads(List<Thread> threads, List<Tracer> traces)
        {
            this.threads = threads;
            this.traces = traces;
        }
        public void Start()
        {
            foreach (var thread in threads)
            {
                thread.Start();
            }
        }
        public void Join()
        {
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
        public List<TraceResult> GetTraceResults()
        {

            List<TraceResult> traceResults = new List<TraceResult>();
            for (int i = 0; i < traces.Count; i++)
            {
                traceResults.Add(traces[i].GetTraceResult());
            }
            return traceResults;
        }
    }
}
