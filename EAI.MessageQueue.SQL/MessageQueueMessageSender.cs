﻿using EAI.General;
using EAI.General.Storage;
using EAI.MessageQueue.SQL;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public class MessageQueueMessageSender : IMessageSender
    {
        private IMQueue _queue;
        private string _endpoint;

        public IMQueue Queue { get => _queue; set => _queue = value; }

        public string Endpoint { get => _endpoint; set => _endpoint = value; }

        public Task SendMessageAsync(object message)
        {
            return SendMessageAsync(message, GetMessageType(message), null);
        }

        public Task SendMessageAsync(object message, string messageType, string transactionKey)
        {
            return _queue.EnqueueAsync(_endpoint, message, messageType, transactionKey);
        }

        private string GetMessageType(object message)
        {
            var messageType = message as IMessageType;

            if(messageType != null)
                return messageType.MessageType;

            return message?.GetType()?.Name;
        }
    }
}
