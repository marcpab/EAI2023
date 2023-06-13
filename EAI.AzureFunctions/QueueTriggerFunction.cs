using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public abstract class QueueTriggerFunction<requestT> : Function
    {
        public async Task RunTestAsync(requestT requestMessage)
        {
            await InitializeTestAsync();

            try
            {

                await ProcessRequestAsync(requestMessage);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
        }

        protected async Task RunQueueRequestAsync(ILoggerFactory loggerFactory, string queueItem)
        {
            await InitializeAsync(loggerFactory);

            try
            {
#warning log raw request

                var requestMessage = GetMessage<requestT>(queueItem);

#warning log request

                await ProcessRequestAsync(requestMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General error");
            }
        }

        protected abstract Task ProcessRequestAsync(requestT requestMessage);
    }
}