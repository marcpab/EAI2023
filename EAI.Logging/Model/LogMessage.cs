using System;
using System.Text;

namespace EAI.Logging.Model
{
    public class LogMessage
    {
        public string Content { get; private set; }
        public string Operation { get; private set; }
        public LogMessageType MsgType { get; private set; }

        public LogMessage(LogMessageType messageType, string operation, string content)
        {
            MsgType = messageType;
            Operation = string.IsNullOrWhiteSpace(operation) ? null : operation;
            Content = string.IsNullOrWhiteSpace(content) ? null : content;
        }
    }
}
