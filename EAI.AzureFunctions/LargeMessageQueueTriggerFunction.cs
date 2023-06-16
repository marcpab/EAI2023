using EAI.AzureStorage;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public abstract class LargeMessageQueueTriggerFunction<requestT> : Function
    {
        public async Task RunTestAsync(string queueItem)
        {
            await InitializeTestAsync();

            try
            {
                var queue = GetQueue();

                var queueMessage = await queue.FromStorageQueueTrigger(queueItem);

                var requestMessage = GetMessage<requestT>(queueMessage.MessageContent);

                await ProcessRequestAsync(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
        }

        protected async Task RunQueueRequestAsync(FunctionContext functionContext, string queueItem)
        {
            await InitializeAsync(functionContext);

            try
            {
#warning log raw request

                var queue = GetQueue();

                var queueMessage = await queue.FromStorageQueueTrigger(queueItem);

                var requestMessage = GetMessage<requestT>(queueMessage.MessageContent);

#warning log request

                await ProcessRequestAsync(requestMessage);

                await queue.CompletedAsync(queueMessage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
        }

        protected abstract LargeMessageQueue GetQueue();

        protected abstract Task ProcessRequestAsync(requestT requestMessage);
    }
}