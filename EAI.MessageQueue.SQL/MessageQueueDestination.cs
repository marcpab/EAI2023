using EAI.General;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.MessageQueue.SQL.Model;
using EAI.Messaging.Abstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EAI.MessageQueue.SQL
{
    public class MessageQueueDestination
    {
        private LoggerV2 _log;
        private IMessageSender _messageSender;
        private Filter[] _messageFilters;
        private ProcessingModeEnum _processingMode;

        public LoggerV2 Log { get => _log; set => _log = value; }
        public IMessageSender MessageSender { get => _messageSender; set => _messageSender = value; }
        public Filter[] Filters { get => _messageFilters; set => _messageFilters = value; }
        public ProcessingModeEnum ProcessingMode { get => _processingMode; set => _processingMode = value; }


        public bool IsMatch(string endpointName, string messageType)
        {
            if (_messageFilters == null)
                return true;
            
            foreach(var filter in _messageFilters) 
                if(filter != null)
                    if(filter.IsMatch(endpointName, messageType))
                        return true;

            return false;
        }

        internal Task SendMessageAsync(MQueue queue, QueueMessage message)
        {
            switch(_processingMode)
            {
                case ProcessingModeEnum.SendOnly:
                    return _messageSender.SendMessageAsync(message, message._messageType, message._messageKey);

                case ProcessingModeEnum.StatusHandling:
                    return SendMessageWithStatusAsync(queue, message);
                default:
                    throw new EAIException($"Unknown processing mode {_processingMode}");
            }
        }

        private async Task SendMessageWithStatusAsync(MQueue queue, QueueMessage queueMessage)
        {
            using (var _ = new ProcessScope())
            {
                try
                {
                    _log?.Start<Info>(nameof(queueMessage), queueMessage, $"Processing queue message {queueMessage._messageId}");

                    await _messageSender.SendMessageAsync(queueMessage._messageContent, queueMessage._messageKey, queueMessage._messageKey);

                    await queue.SuccessAsync(queueMessage);

                    _log?.Success<Info>();
                }
                catch (Exception ex)
                {
                    await queue.FailedAsync(queueMessage);

                    _log?.Failed<Info>(ex);
                }
            }
        }
    }
}
