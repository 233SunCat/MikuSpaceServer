using System.Diagnostics;

namespace MikuSpaceServer.Logger
{
    public class ConsoleListener: TraceListener
    {
        public override void Write(string message) { 
            Console.Write("override Write: "+message);
        }
        public override void WriteLine(string message) { 
            Console.WriteLine("override Write: " + message);
        }
    }
}
