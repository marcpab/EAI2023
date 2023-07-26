using EAI.General;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.GenericServer.SAPNcoService
{
    public class RfcServerDestination
    {
        private IMessageSender _messageSender;
        private string[] _messageFilters;

        public IMessageSender MessageSender { get => _messageSender; set => _messageSender = value; }
        public string[] MessageFilters { get => _messageFilters; set => _messageFilters = value; }

        public bool IsMatch(string rfcFunctionName)
        {
            if (_messageFilters == null)
                return true;
            
            foreach(var filter in _messageFilters) 
                if(filter != null)
                    if(Regex.IsMatch(rfcFunctionName, filter.Replace("*", ".*")))
                        return true;

            return false;
        }

        public Task SendMessage(JObject message)
            => _messageSender.SendMessageAsync(message);

    }
}
