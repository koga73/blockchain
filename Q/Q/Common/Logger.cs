using System;
using System.Diagnostics;

namespace Q.Common
{
    public class Logger
    {
        public enum TYPE {
            ERROR,
            WARNING,
            INFO
        }

        static Logger()
        {
            Trace.Listeners.Add(new TextWriterTraceListener("application.log", "myListener"));
            Trace.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Trace.AutoFlush = true;
        }

        public static void Error(Exception ex)
        {
            WriteEntry(TYPE.ERROR, ex.Message);
        }

        public static void Warning(string message)
        {
            WriteEntry(TYPE.WARNING, message);
        }

        public static void Info(string message)
        {
            WriteEntry(TYPE.INFO, message);
        }

        public static void WriteEntry(TYPE type, string message)
        {
            string msg = message.Replace("{", "{{").Replace("}", "}}");
            Trace.WriteLine(string.Format($"{type}: {DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss")} - {msg}"));
        }
    }
}
