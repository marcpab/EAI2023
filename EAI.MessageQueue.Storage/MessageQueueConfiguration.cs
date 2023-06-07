using EAI.MessageQueue.Storage.Manager;
using EAI.MessageQueue.Storage.Sender;
using System.Text.Json.Serialization;

namespace EAI.MessageQueue.Storage
{
    public class MessageQueueConfiguration
    {
        public TicketConfiguration MaxTickets { get; set; } = new TicketConfiguration();
        public TimeoutConfiguration Timeouts { get; set; } = new TimeoutConfiguration();
        public string Manager { get; set; } = string.Empty;
        public string Sender { get; set; } = string.Empty;
        public int LeaseTimeout { get; set; } = 0;
        public ArchiveConfiguration Archive { get; set; } = new ArchiveConfiguration();

        [JsonIgnore]
        public Type ManagerType => Type.GetType(Manager) ?? typeof(DefaultManager);

        [JsonIgnore]
        public Type SenderType => Type.GetType(Sender) ?? typeof(DefaultMessageSender);


        public int GetTimeout(string queue)
        {
            var timeout = Timeouts.Default;
            if (Timeouts.Custom != null && Timeouts.Custom.ContainsKey(queue))
                timeout = Timeouts.Custom[queue];

            return timeout;
        }

        public int GetMaxTickets(string queue)
        {
            var maxTickets = MaxTickets.Default;
            if (MaxTickets.Custom != null && MaxTickets.Custom.ContainsKey(queue))
                maxTickets = MaxTickets.Custom[queue];

            return maxTickets;
        }

        public class TicketConfiguration
        {
            public int Default { get; set; } = 0;
            public Dictionary<string, int> Custom { get; set; } = new Dictionary<string, int>();
        }

        public class TimeoutConfiguration
        {
            public int Default { get; set; } = 0;
            public Dictionary<string, int> Custom { get; set; } = new Dictionary<string, int>();
        }

        public class ArchiveConfiguration
        {
            public bool Default { get; set; } = false;
            public Dictionary<string, bool> Custom { get; set; } = new Dictionary<string, bool>();
        }
    }
}

