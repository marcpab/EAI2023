using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EAI.Logging.Model
{
    public class LogException
    {
        private readonly static string _INDENT = new string(' ', 10);

        public int NestLevel { get; private set; }
        public string Exception { get; private set; }
        public string Message { get; private set; }
        public string Source { get; private set; }
        public int HResult { get; private set; }
        public string TargetSite { get; private set; }
        public string StackTrace { get; private set; }

        public LogException(Exception exception, int nestLevel)
        {
            NestLevel = nestLevel;
            Exception = exception.GetType().Name;
            Message = FormatText(exception.Message);
            Source = FormatText(exception.Source);
            HResult = exception.HResult;
            TargetSite = FormatText(exception.TargetSite?.ToString());
            StackTrace = FormatText(exception.StackTrace);
        }

        private static readonly Func<string,string> FormatText = (string message) =>
        {
            if(string.IsNullOrWhiteSpace(message))
                return EAI.Texts.Properties.NULL;

            return message.Replace("\n", $"\n{_INDENT}");
        };

        public static LogException[] GetExceptionArray(Exception exception)
        {
            var nestLevel = 0;

            return GetExceptionList(exception).
                Select(ex => new LogException(ex, nestLevel++)).
                ToArray();
        }

        public static List<Exception> GetExceptionList(Exception exception)
        {
            var exceptionList = new List<Exception>();

            if (exception == null)
                return exceptionList;

            exceptionList.Add(exception);

            while (exception?.InnerException != null)
            {
                exception = exception.InnerException;

                if (exceptionList.Contains(exception))
                    break;

                exceptionList.Add(exception);
            }

            return exceptionList;
        }
    }
}
