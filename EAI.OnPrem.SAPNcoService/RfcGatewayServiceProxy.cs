using EAI.Abstraction.SAPNcoService;
using EAI.General.SettingJson;
using EAI.OnPrem.Storage;
using EAI.SAPNco.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OnPrem.SAPNcoService
{
    public class RfcGatewayServiceProxy : IRfcGatewayService
    {
        private static readonly JsonSerializerSettings _rfcSerializerSettings = new JsonSerializerSettings() { 
            NullValueHandling = NullValueHandling.Ignore, 
            TypeNameHandling = TypeNameHandling.Auto,
            Converters = new JsonConverter[]
                {
                    new DateConverter()
                }
        };

        private OnPremClient _onPremClient;

        public OnPremClient OnPremClient { get => _onPremClient; set => _onPremClient = value; }

        public async Task<string> CallRfcAsync(string name, string jRfcRequestMessage, bool autoCommit = false)
        {
            var callRfcRequest = new CallRfcRequest()
            {
                _name = name,
                _jRfcRequestMessage = jRfcRequestMessage,
                _autoCommit = autoCommit
            };

            var callRfcResponse = await _onPremClient.SendRequest<CallRfcResponse, CallRfcRequest>(callRfcRequest);

            return callRfcResponse._ret;
        }

        public async Task<T> CallRfcAsync<T>(string name, T jRfcRequestMessage, bool autoCommit = false)
        {
            var callRfcRequest = new CallRfcRequest()
            {
                _name = name,
                _jRfcRequestMessage = JsonConvert.SerializeObject(jRfcRequestMessage, _rfcSerializerSettings),
                _autoCommit = autoCommit
            };

            var callRfcResponse = await _onPremClient.SendRequest<CallRfcResponse, CallRfcRequest>(callRfcRequest);

            return JsonConvert.DeserializeObject<T>(callRfcResponse._ret, _rfcSerializerSettings);
        }

        public async Task<string> GetJRfcSchemaAsync(string name, string functionName)
        {
            var getJRfcSchemaRequest = new GetJRfcSchemaRequest()
            {
                _name = name,
                _functionName = functionName
            };

            var getJRfcSchemaResponse = await _onPremClient.SendRequest<GetJRfcSchemaResponse, GetJRfcSchemaRequest>(getJRfcSchemaRequest);

            return getJRfcSchemaResponse._ret;
        }

        public async Task<RfcFunctionMetadata> GetRfcFunctionMetadataAsync(string name, string functionName)
        {
            var getRfcFunctionMetadataRequest = new GetRfcFunctionMetadataRequest()
            {
                _name = name,
                _functionName = functionName
            };

            var getJRfcSchemaResponse = await _onPremClient.SendRequest<GetRfcFunctionMetadataResponse, GetRfcFunctionMetadataRequest>(getRfcFunctionMetadataRequest);

            return getJRfcSchemaResponse._ret;
        }
    }
}
