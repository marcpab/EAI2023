using System;
using System.Collections.Generic;

namespace EAI.LoggingV2.Model
{
    public class LogData
    {
        public string _logLevel;
        public string _messageKey;
        public string _logText;

        public MessageData _message;
        public ExceptionData[] _exceptions;
        public Exception _exception;
    }
}
