namespace EAI.MessageQueue.Storage.Sender
{
    public interface IMessageSender
    {
        Task SendMessageAsync(MessageItem message);
        string GetSenderName(MessageItem message);
    }
}
