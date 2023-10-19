using EAI.MessageQueue.SQL.Model;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public interface IQueueMessageStore
    {
        Task SaveAsync(QueueMessage queueMessage);
        Task LoadAsync(QueueMessage queueMessage);
        Task DeleteAsync(QueueMessage queueMessage);
    }
}