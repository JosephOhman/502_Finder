using NLog;

namespace _502Finder.Logging
{
    public static class Logger<T>
    {
        private static NLog.Logger _logger = LogManager.GetCurrentClassLogger(typeof(T));

        public static void Trace(string message, string json = null)
        {
        }

        public static void Debug(string message, string json = null)
        {
        }

        public static void Info(string message, string json = null)
        {
        }

        public static void Warning(string message, string json = null)
        {
        }

        public static void Error(string message, string json = null)
        {
        }

        public static void Fatal(string message, string json = null)
        {
        }
    }
}