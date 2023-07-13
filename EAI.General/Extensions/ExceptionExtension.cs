using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.General.Extensions
{
    public static class ExceptionExtension
    {
        public static IEnumerable<Exception> GetExceptions(this Exception exception)
        {
            while(exception != null)
            {
                yield return exception;

                exception = exception.InnerException;
            }
        }

        public static string GetExceptionInformation(this Exception exception)
        {
            if (exception == null)
                return null;

            return JsonConvert.SerializeObject(exception, Formatting.Indented);
        }
    }
}
