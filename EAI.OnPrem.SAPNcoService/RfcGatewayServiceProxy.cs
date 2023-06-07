using EAI.Abstraction.SAPNcoService;
using EAI.OnPrem.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EAI.OnPrem.SAPNcoService
{
    public class RfcGatewayServiceProxy : IRfcGatewayService
    {
        private OnPremClient _onPremClient;

        public OnPremClient OnPremClient { get => _onPremClient; set => _onPremClient = value; }

        public async Task<string> CallRfcAsync(string name, string jRfcRequestMessage)
        {
            var callRfcRequest = new CallRfcRequest()
            {
                _name = name,
                _jRfcRequestMessage = jRfcRequestMessage
            };

            var callRfcResponse = await _onPremClient.SendRequest<CallRfcResponse, CallRfcRequest>(callRfcRequest);

            return callRfcResponse._ret;
        }

        public async Task<T> CallRfcAsync<T>(string name, T jRfcRequestMessage)
        {
            var callRfcRequest = new CallRfcRequest()
            {
                _name = name,
                _jRfcRequestMessage = JsonConvert.SerializeObject(jRfcRequestMessage)
            };

            var callRfcResponse = await _onPremClient.SendRequest<CallRfcResponse, CallRfcRequest>(callRfcRequest);

            return JsonConvert.DeserializeObject<T>(callRfcResponse._ret);
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
    }
}
