using EAI.AzureStorage;
using EAI.LoggingV2.Levels;
using EAI.Messaging.Abstractions;
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

            SetupProcessContext(null);

            try
            {
                Log?.Start<Info>(nameof(queueItem), queueItem, "Start (test)");

                var queue = GetQueue();

                var queueMessage = await queue.FromStorageQueueTrigger(queueItem);

                SetParentContext((queueMessage as IMessageProcessContext)?.ProcessContext);

                var requestMessage = GetMessage<requestT>(queueMessage.MessageContent);

                SetMetadataAndLog(requestMessage);

                await ProcessRequestAsync(requestMessage);

                Log?.Success<Info>();

            }
            catch (Exception ex)
            {
                Log?.Failed<Error>(ex);
            }

            if (Log != null)
                await Log.FlushAsync();
        }

        public async Task RunEntityTestAsync(requestT requestMessage)
        {
            await InitializeTestAsync();

            SetupProcessContext(null);

            try
            {
                Log?.Start<Info>(nameof(requestMessage), requestMessage, "Start (test)");

                var queue = GetQueue();

                SetMetadataAndLog(requestMessage);

                await ProcessRequestAsync(requestMessage);

                Log?.Success<Info>();

            }
            catch (Exception ex)
            {
                Log?.Failed<Error>(ex);
            }

            if (Log != null)
                await Log.FlushAsync();
        }

        protected async Task RunQueueRequestAsync(FunctionContext functionContext, string queueItem)
        {
            await InitializeAsync(functionContext);

            SetupProcessContext(null);

            try
            {
                Log?.Start<Info>(nameof(queueItem), queueItem);

                var queue = GetQueue();

                var queueMessage = await queue.FromStorageQueueTrigger(queueItem);

                var requestMessage = GetMessage<requestT>(queueMessage.MessageContent);

                SetMetadataAndLog(requestMessage);

                await ProcessRequestAsync(requestMessage);

                await queue.CompletedAsync(queueMessage);

                Log.Success<Info>();
            }
            catch (Exception ex)
            {
                Log?.Failed<Error>(ex);
            }

            if (Log != null)
                await Log?.FlushAsync();
        }

        protected abstract LargeMessageQueue GetQueue();

        protected abstract Task ProcessRequestAsync(requestT requestMessage);
    }
}