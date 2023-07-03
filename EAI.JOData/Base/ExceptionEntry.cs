namespace EAI.JOData.Base
{
    public class ExceptionEntry
    {
        public int NestLevel { get; set; } = 0;
        public string Exception { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Source { get; set; } = null;
        public int HResult { get; set; } = 0;
        public string? TargetSite { get; set; } = null;
        public string? StackTrace { get; set; } = null;

        public static IEnumerable<ExceptionEntry> GetExceptionEntries(Exception? exception)
        {
            if (exception is null)
                return Enumerable.Empty<ExceptionEntry>();

            var nestLevel = 0;

            return ToList(exception).Select(ex => new ExceptionEntry()
            {
                NestLevel = nestLevel++,
                Exception = ex.GetType().Name,
                Message = ex.Message,
                Source = ex.Source,
                TargetSite = ex.TargetSite?.Name,
                StackTrace = ex.StackTrace,
                HResult = ex.HResult
            });
        }

        private static List<Exception> ToList(Exception exception)
        {
            var exceptionList = new List<Exception>();

            if (exception is not null)
                exceptionList.Add(exception);

            while (exception?.InnerException is not null)
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
