namespace EAI.General.Storage
{
    public class StorageQueueMessage
    {
        private string _messageId;
        private string _messageContent;
        private string _popReceipt;

        public StorageQueueMessage(string messageId, string popReceipt, string messageContent)
        {
            _messageId = messageId;
            _messageContent = messageContent;
            _popReceipt = popReceipt;
        }

        public string MessageId { get => _messageId; }
        public string PopReceipt { get => _popReceipt; }
        public string MessageContent { get => _messageContent; }
    }
}