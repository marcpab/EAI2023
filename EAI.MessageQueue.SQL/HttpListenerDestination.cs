using EAI.General;
using EAI.MessageQueue.SQL.Model;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public class MessageQueueDestination
    {
        private IMessageSender _messageSender;
        private Filter[] _messageFilters;

        public IMessageSender MessageSender { get => _messageSender; set => _messageSender = value; }
        public Filter[] Filters { get => _messageFilters; set => _messageFilters = value; }

        public bool IsMatch(string endpointName, string messageType)
        {
            if (_messageFilters == null)
                return true;
            
            foreach(var filter in _messageFilters) 
                if(filter != null)
                    if(Regex.IsMatch(endpointName, filter.EndpointName.Replace("*", ".*")) &&
                        Regex.IsMatch(messageType, filter.MessageType.Replace("*", ".*")))
                        return true;

            return false;
        }

        internal Task SendMessage(QueueMessage message)
            => _messageSender.SendMessageAsync(message._messageContent, message._messageType, message._messageKey);

    }
}
