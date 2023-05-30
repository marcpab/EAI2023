using EAI.General.Storage;

namespace EAI.AzureStorage
{
    public class LargeStorageQueueMessage : StorageQueueMessage
    {
        private string _blobName;

        public LargeStorageQueueMessage(string messageId, string popReceipt, string messageContent, string blobName) : base(messageId, popReceipt, messageContent)
        {
            _blobName = blobName;
        }

        public string BlobName { get => _blobName; }
    }


}
