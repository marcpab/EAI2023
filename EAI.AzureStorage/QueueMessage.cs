using EAI.General;

namespace EAI.AzureStorage
{
    public class QueueMessage
    {
        public ProcessContext _processContext;
        public StorageLocationEnum _storageLocation;
        public string _content;
    }
}
