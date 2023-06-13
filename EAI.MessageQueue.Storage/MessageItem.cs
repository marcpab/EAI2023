using EAI.MessageQueue.Storage.Manager;

namespace EAI.MessageQueue.Storage
{
    public class MessageItem
    {
        public Type MessageManager { get; set; } = typeof(DefaultManager);
        public string? Id { get; set; } = null;
        public string? Endpoint { get; set; } = null;
        public string MessageType { get; set; } = string.Empty;
        public string MessageKey { get; set; } = string.Empty;
        public string Payload { get; set; } = string.Empty;
        public DateTimeOffset ReceivedOn { get; set; }
        public ProcessingStatus Status { get; set; } = ProcessingStatus.New;

        public string GetQueue => Endpoint?.ToLower() ?? "default";
        public string MessageName => $"{Endpoint}-{Id}";

        public static MessageItem Create(string endpoint, string jsonObject, string key, string msgType, Type manager)
        {
            var meta = new MessageItem();

            if (manager is null || manager.IsAssignableFrom(typeof(IMessageManager)))
                throw new ArgumentOutOfRangeException($"manager {manager?.Name ?? EAI.Texts.Properties.NULL} not accepted");

            if (string.IsNullOrWhiteSpace(msgType))
                throw new ArgumentException("msgType is flow control! Field cannot be empty");

            if (string.IsNullOrWhiteSpace(endpoint))
                throw new ArgumentException("endpoint is flow control! Field cannot be empty");

            if (string.IsNullOrWhiteSpace(jsonObject))
                throw new ArgumentException("jsonObject is empty, queuing makes no sense");

            meta.Id = DateTime.Now.Ticks.ToString();
            meta.MessageKey = key;
            meta.Payload = jsonObject;
            meta.MessageManager = manager;
            meta.MessageType = msgType;
            meta.Endpoint = endpoint.ToLower();
            meta.Status = ProcessingStatus.New;
            meta.ReceivedOn = DateTimeOffset.UtcNow;

            return meta;
        }
    }
}
