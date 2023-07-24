using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.PipeMessaging.SAPNcoService.Messaging
{
    public class ExceptionData
    {
        public string _message;
        public string _stacktrace;
        public string _helpLink;
        public int _hResult;
        public string _source;
        public string _targetSite;
        public ExceptionData _innerException;

        public static ExceptionData FromException(Exception e)
            => new ExceptionData()
                {
                    _message = e.Message,
                    _stacktrace = e.StackTrace,
                    _helpLink = e.HelpLink,
                    _hResult = e.HResult,
                    _source = e.Source,
                    _targetSite = e.TargetSite?.Name,
                    _innerException = e.InnerException == null ? null : FromException(e.InnerException)
                };
    }
}
