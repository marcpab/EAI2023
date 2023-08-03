using EAI.General;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.SAPNco.IDOC.MessageSender
{
    public class IdocDestination
    {
        private IMessageSender _messageSender;
        private string[] _messageFilters;
        private IdocFormatEnum _format;

        public IMessageSender MessageSender { get => _messageSender; set => _messageSender = value; }
        public string[] MessageFilters { get => _messageFilters; set => _messageFilters = value; }
        public IdocFormatEnum Format { get => _format; set => _format = value; }

        public bool IsMatch(string idocType)
        {
            if (_messageFilters == null)
                return true;
            
            foreach(var filter in _messageFilters) 
                if(filter != null)
                    if(Regex.IsMatch(idocType, filter.Replace("*", ".*")))
                        return true;

            return false;
        }

        public Task SendMessage(object message, string messageType, string transactionKey)
            => _messageSender.SendMessageAsync(message, messageType, transactionKey);

        public override string ToString()
            => MessageFilters == null ? "no filter" : string.Join(", ", MessageFilters);
    }
}
