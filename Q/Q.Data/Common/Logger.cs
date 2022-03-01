using System;
using System.Diagnostics;

namespace Q.Data.Common
{
    public class Logger
    {
        public static string LOG_FILE_NAME = "q.log";

        public enum TYPE
        {
            ERROR,
            WARNING,
            INFO
        }

        static Logger()
        {
            string logFile = Path.Join(Paths.LogsPath, LOG_FILE_NAME); ;
            Trace.Listeners.Add(new TextWriterTraceListener(logFile, "myListener"));
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
