namespace lab1spp
{
    public class Foo
    {
        private Bar _bar;
        private ITracer _tracer;

        internal Foo(ITracer tracer)
        {
            _tracer = tracer;
            _bar = new Bar(_tracer);
        }

        public void MyMethod()
        {
            _tracer.StartTrace();
            _bar.InnerMethod();
            _tracer.StopTrace();
        }
    }

    public class Bar
    {
        private ITracer _tracer;

        internal Bar(ITracer tracer)
        {
            _tracer = tracer;
        }

        public void InnerMethod()
        {
            _tracer.StartTrace();
            _tracer.StopTrace();
        }
    }

    public class Program
    {
        static void Main()
        {
            ITracer tracer = new Tracer();

            Foo foo = new Foo(tracer);
            Bar bar = new Bar(tracer);

            //Thread thread1 = new Thread(foo.MyMethod);
            //Thread thread2 = new Thread(bar.InnerMethod);

            //thread1.Start();
            //thread2.Start();

            //thread1.Join();
            //thread2.Join();

            tracer.StartTrace();
            foo.MyMethod();
            tracer.StopTrace();

            TraceResult traceResult = tracer.GetTraceResult();

            string jsonResult = Tracer.JsonSerializeTraceResult(traceResult);
            string xmlResult = Tracer.XmlSerializeTraceResult(traceResult);

            Console.WriteLine(jsonResult);
            Console.WriteLine(xmlResult);

            File.WriteAllText("results.json", jsonResult);
            File.WriteAllText("results.xml", xmlResult);
        }
    }
}
