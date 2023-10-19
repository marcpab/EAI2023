using System;

namespace EAI.MessageQueue.SQL.Model
{
    public class QueueMessage
    {
        public long _messageId;
        public string _processId;
        public string _stage;
        public string _endpointName;
        public QueueMessageStatusEnum _id_status;
        public string _messageKey;
        public byte[] _messageHash;
        public string _messageType;
        public string _messageContentType;
        public string _messageContent;
        public DateTimeOffset _createdOnUTC;
    }
}
