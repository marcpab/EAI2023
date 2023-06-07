namespace EAI.MessageQueue.Storage.Ticket
{
    public interface IQueueTicket
    {
        Type TicketType { get; }
        Task<MessageItem?> Get(string cs);
    }
}
