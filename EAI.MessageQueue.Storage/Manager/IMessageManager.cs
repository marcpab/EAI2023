using EAI.MessageQueue.Storage.Ticket;

namespace EAI.MessageQueue.Storage.Manager
{
    /// <summary>
    /// manages metadata wrapper of the stored message
    /// </summary>
    public interface IMessageManager
    {
        string GetContainerEnqueue(string queueName);
        string GetContainerDequeue(string queueName);

        // lock queue resource for dequeuing
        Task<IQueueLease?> RequestLeaseAsync(string queueName);

        // enqueue new messages
        Task<MessageItem> EnqueueAsync(MessageItem message);

        // dequeue with integrated locking and free timeout handling
        IAsyncEnumerable<MessageItem> DequeueAsync(string queueName, int maxTickets, int timeoutInSeconds);

        // dequeue with external locking and free timeout handling
        IAsyncEnumerable<MessageItem> DequeueAsync(IQueueLease lease, string queueName, int maxTickets);

        // release single message from dequeue
        Task<MessageItem?> ReleaseAsync(MessageItem message, bool fault);

        // removes timeout messages from dequeue
        Task<int> FreeTimeoutMessagesAsync(IQueueLease lease, string queueName, int timeoutInSeconds);

        // returns a list of all existing queues (list should be maintained via enqueue operation)
        IAsyncEnumerable<string> GetQueues();

        IQueueTicket GetTicket(MessageItem message);

        DequeueStatus Status { get; }
    }
}
