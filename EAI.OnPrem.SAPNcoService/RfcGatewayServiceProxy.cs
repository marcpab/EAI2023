using EAI.Abstraction.SAPNcoService;
using EAI.OnPrem.Storage;
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

            var foo = await _onPremClient.SendRequest<CallRfcResponse, CallRfcRequest>(callRfcRequest);

            return foo._ret;
        }
    }
}
