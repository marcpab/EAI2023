using EAI.General;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.HttpListener
{
    public class HttpListenerDestination
    {
        private IMessageSender _messageSender;
        private string[] _messageFilters;

        public IMessageSender MessageSender { get => _messageSender; set => _messageSender = value; }
        public string[] UriFilters { get => _messageFilters; set => _messageFilters = value; }

        public bool IsMatch(string requestUri)
        {
            if (_messageFilters == null)
                return true;
            
            foreach(var filter in _messageFilters) 
                if(filter != null)
                    if(Regex.IsMatch(requestUri, filter.Replace("*", ".*")))
                        return true;

            return false;
        }

        internal Task SendMessage(HttpMessage message)
            => _messageSender.SendMessageAsync(message);

    }
}
