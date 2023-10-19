namespace EAI.MessageQueue.SQL
{
    public class Filter
    {
        private string _endpointName;
        private string _messageType;

        public string EndpointName { get => _endpointName; set => _endpointName = value; }
        public string MessageType { get => _messageType; set => _messageType = value; }


    }
}