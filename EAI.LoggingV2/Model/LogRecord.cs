using System;
using System.Collections.Generic;
using System.Text;

namespace EAI.LoggingV2.Model
{
    public class LogRecord
    {
        public LogActionEnum _logAction;

        public string _stage;
        public string _processId;

        public DateTimeOffset _createdOnUTC;

        public ProcessData _processData;

        public LogData _logData;

        public override string ToString()
        {
            if (_logData?._message == null)
                return string.Format("    {0,-5} {1}:{2:X8} {3} {4}", _logData?._logLevel, _processData._serviceName, _processId.GetHashCode(), _logData?._messageKey, _logData?._logText);
            else
                return string.Format("Msg {0,-5} {1}:{2:X8} {3} {4} # {5} ({6}): {7}", _logData?._logLevel, _processData._serviceName, _processId?.GetHashCode(), _logData?._messageKey, _logData?._logText, _logData._message._name, _logData._message._contentType, _logData._message._content);
        }
    }
}
