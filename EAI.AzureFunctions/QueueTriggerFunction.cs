using Azure.Core;
using EAI.General;
using EAI.LoggingV2.Levels;
using EAI.Messaging.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.Tracing;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public abstract class QueueTriggerFunction<requestT> : Function
    {
        public async Task RunTestAsync(requestT requestMessage)
        {
            await InitializeTestAsync();

            SetupProcessContext(null);

            try
            {
                Log?.Start<Info>(nameof(requestMessage), requestMessage, "Start (test)");

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

            SetupProcessContext(functionContext.InvocationId);

            try
            {

                Log?.Start<Info>(nameof(queueItem), queueItem);

                var requestMessage = GetMessage<requestT>(queueItem);

                SetMetadataAndLog(requestMessage);

                await ProcessRequestAsync(requestMessage);

                Log?.Success<Info>();
            }
            catch (Exception ex)
            {
                Log?.Failed<Error>(ex);
            }

            if(Log != null) 
                await Log.FlushAsync();
        }

        protected abstract Task ProcessRequestAsync(requestT requestMessage);
    }
}