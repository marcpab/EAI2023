using EAI.General;
using EAI.General.Storage;
using EAI.Messaging.Abstractions;
using System;

namespace EAI.AzureStorage
{
    public class LargeStorageQueueMessage : StorageQueueMessage, IMessageProcessContext
    {
        private string _blobName;
        private ProcessContext _processContext;

        public LargeStorageQueueMessage(string messageId, string popReceipt, ProcessContext processContext, string messageContent, string blobName) : base(messageId, popReceipt, messageContent)
        {
            _blobName = blobName;
            _processContext = processContext;
        }

        public string BlobName { get => _blobName; }
        public ProcessContext ProcessContext { get => _processContext; set => throw new NotImplementedException();  }
    }


}
