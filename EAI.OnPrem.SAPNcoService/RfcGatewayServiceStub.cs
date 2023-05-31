using EAI.Abstraction.SAPNcoService;
using EAI.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EAI.OnPrem.SAPNcoService
{
    public class RfcGatewayServiceStub : IServiceRequestDispatcher
    {
        private IServiceHost _serviceHost;
        private IRequestListener _requestListener;
        private IRfcGatewayService _rfcGatewayService;

        public IRequestListener RequestListener { get => _requestListener; }
        public IRfcGatewayService RfcGatewayService { get => _rfcGatewayService; }
        public IServiceHost ServiceHost { get => _serviceHost; set => _serviceHost = value; }

        public void Initialize(IRequestListener requestListener)
        {
            _requestListener = requestListener;
            _rfcGatewayService = _serviceHost.GetServices<IRfcGatewayService>().FirstOrDefault();

            _requestListener.RegisterRequestHandler<CallRfcRequest, CallRfcResponse>(CallRfcAsyc);
        }

        private async Task<CallRfcResponse> CallRfcAsyc(CallRfcRequest request)
        {
            var rfcResponse = await _rfcGatewayService.CallRfcAsync(request._name, request._jRfcRequestMessage);

            return new CallRfcResponse()
                        {
                            _ret = rfcResponse
                        };
        }
    }
}
