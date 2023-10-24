using System.Text.RegularExpressions;

namespace EAI.MessageQueue.SQL
{
    public class Filter
    {
        private string _endpointName;
        private string _messageType;

        public string EndpointName { get => _endpointName; set => _endpointName = value; }
        public string MessageType { get => _messageType; set => _messageType = value; }

        internal bool IsMatch(string endpointName, string messageType)
        {
            if(!string.IsNullOrEmpty(_endpointName) && !Regex.IsMatch(endpointName, _endpointName.Replace("*", ".*")))
                return false;

            if (!string.IsNullOrEmpty(_messageType) && !Regex.IsMatch(messageType, _messageType.Replace("*", ".*")))
                return false;

            return true;
        }
    }
}