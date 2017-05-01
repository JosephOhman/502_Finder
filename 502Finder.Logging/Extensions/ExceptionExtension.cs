using System;

namespace _502Finder.Logging.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetMessage(this Exception ex)
        {
            string msg = $"{ex.Message}";
            if (ex.InnerException != null)
                msg += $" {ex.InnerException.Message}";

            return msg;
        }
    }
}
