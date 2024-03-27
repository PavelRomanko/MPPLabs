using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace lab1spp
{
    //[DataContract]
    public class TraceResult
    {
        //[DataMember]
        public string Name { get; set; } = string.Empty;
        //[DataMember]
        public string Class { get; set; } = string.Empty;
        //[DataMember]
        public long Time { get; set; }
        //[DataMember]
        public List<TraceResult> Methods { get; set; } = new List<TraceResult>();
    }
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        TraceResult GetTraceResult();
    }
    public class Tracer : ITracer
    {
        //private readonly Stopwatch stopwatch;
        private readonly Stack<TraceResult> traceStack;
        private readonly TraceResult root;
        private TraceResult parent;
        private object lockObject = new object();
        DateTime start;
        DateTime end;

        public Tracer()
        {
            //stopwatch = new();
            traceStack = new();
            root = new();
            parent = root;
        }

        public TraceResult GetTraceResult()
        {
            return root.Methods[0];
        }

        public void StartTrace()
        {
            lock (lockObject)
            {
                //stopwatch.Start();
                start = DateTime.Now;
                StackFrame stackFrame = new(1);
                string methodName = stackFrame.GetMethod().Name;
                Type declaringType = stackFrame.GetMethod().DeclaringType;
                string className = declaringType != null ? declaringType.Name : string.Empty;
                TraceResult traceResult = new()
                {
                    Class = className,
                    Name = methodName
                };
                parent.Methods.Add(traceResult);
                traceStack.Push(parent);
                parent = traceResult;
            }
        }

        public void StopTrace()
        {
            lock (lockObject)
            {
                //stopwatch.Stop();
                //TimeSpan elapsed = stopwatch.Elapsed;
                //parent.Time = elapsed.TotalMilliseconds;
                end = DateTime.Now;
                long ticks = end.Ticks - start.Ticks;
                //long milliseconds = ticks / TimeSpan.TicksPerMillisecond;
                parent.Time = ticks;
                parent = traceStack.Pop();
            }
        }
        public static string JsonSerializeTraceResult(TraceResult traceResult)
        {
            JsonSerializerOptions options = new()
            {
                IgnoreNullValues = true,
                WriteIndented = true
            };
            string serializedJson = JsonSerializer.Serialize(traceResult, options);
            return serializedJson;
        }
        public static string XmlSerializeTraceResult(TraceResult traceResult)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TraceResult));
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, traceResult);
            string serializedXml = stringWriter.ToString();
            return serializedXml;
        }
    }
}
