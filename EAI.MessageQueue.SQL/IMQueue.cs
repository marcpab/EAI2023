using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public interface IMQueue
    {
        IQueueMessageManager MessageManager { get; set; }
        IQueueMessageStore MessageStore { get; set; }

        Task<long> EnqueueAsync(string endpointName, object message, string messageType = null, string messageKey = null);
    }
}