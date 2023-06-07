using EAI.MessageQueue.Storage.Ticket;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace EAI.MessageQueue.Storage
{
    public class MessageQueueTicket
    {
        private static readonly string _Storage = "AzureWebJobsStorage";

        public Type TicketType { get; set; } = typeof(ContainerTicket);

        public static async Task<MessageItem?> GetMessageItem(IConfiguration configuration, string ticketJson)
        {
            var baseTicket = JsonConvert.DeserializeObject<MessageQueueTicket>(ticketJson);
            if(baseTicket == null) 
            {
                return null;
            }

            var ticket = (IQueueTicket?)JsonConvert.DeserializeObject(ticketJson, baseTicket.TicketType);            
            var cs = configuration[_Storage] ?? configuration[$"Values:{_Storage}"];
            if(ticket == null || string.IsNullOrWhiteSpace(cs)) 
            {
                return null;
            }

            return await ticket.Get(cs);
        }
    }
}
