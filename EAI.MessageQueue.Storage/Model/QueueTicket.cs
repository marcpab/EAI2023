namespace EAI.MessageQueue.Storage.Model
{
    public class QueueTicket
    {
        public string Payload { get; set; }
        public string Ticket { get; set; }

        public QueueTicket(string payload, string ticket)
        {
            Payload = payload;
            Ticket = ticket;
        }
    }
}
