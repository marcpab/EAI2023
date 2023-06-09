﻿using EAI.General.SettingJson;
using EAI.General.Settings;
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

        protected T GetMessage<T>(string messageContent)
        {
            if (typeof(T) == typeof(string))
                return (T)(object)messageContent;

            return JsonConvert.DeserializeObject<T>(messageContent);
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
    }
}