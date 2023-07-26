using EAI.General;
using EAI.General.SettingJson;
using EAI.General.Settings;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;
using EAI.Messaging.Abstractions;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace EAI.AzureFunctions
{
    public class Function
    {
        protected ILogger _logger;

        public LoggerV2 Log { get; set; }
        public string Stage { get; set; }

        public ProcessContext Context { get => ProcessContext.GetCurrent(); }

        protected T GetMessage<T>(string messageContent)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)messageContent;

            return JsonConvert.DeserializeObject<T>(messageContent);
        }

        protected virtual void SetupProcessContext(string processId)
        {
            ProcessContext.Create(processId, Stage, GetType().FullName);
        }

        protected virtual Task InitializeTestAsync()
        {
            var currentType = GetType();

            _logger = NullLoggerFactory.Instance.CreateLogger(currentType);

            return LoadSettingAsync(currentType);        
        }

        protected virtual Task InitializeAsync(FunctionContext functionContext)
        {
            var currentType = GetType();

            _logger = functionContext.GetLogger(currentType.Name);

            return LoadSettingAsync(currentType);
        }

        protected async Task LoadSettingAsync(Type currentType)
        {
            var config = await ConfigurationHandler.Instance.GetConfigurationAsync(currentType);
            var configJson = config.ToString();

            new SettingJsonHandler(_logger).DeserializeInstance(this, configJson);
        }

        protected void SetMetadataAndLog<requestT>(requestT requestMessage)
        {
            if (typeof(requestT) != typeof(string))
            {
                SetParentContext((requestMessage as IMessageProcessContext)?.ProcessContext);

                SetTransactionKey((requestMessage as IMessageTransactionKey)?.TransactionKey);

                Log?.Message<Debug>(nameof(requestMessage), requestMessage, "initial message");
            }
        }

        protected static void SetParentContext(ProcessContext parentProcessContext)
        {
            if (parentProcessContext != null)
                ProcessContext.SetParentContext(parentProcessContext);
        }

        private void SetTransactionKey(string transactionKey)
        {
            if (transactionKey != null)
            {
                if (Log != null)
                {
                    Log.TransactionKey = transactionKey;
                    Log.Update<None>();
                }
            }
        }
    }
}