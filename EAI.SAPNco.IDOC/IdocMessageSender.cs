using EAI.Messaging.Abstractions;
using EAI.SAPNco.IDOC.Json;
using EAI.SAPNco.IDOC.Model;
using EAI.SAPNco.IDOC;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAI.Abstraction.SAPNcoService;
using EAI.General;

namespace EAI.IdocReceiver
{
    public class IdocMessageSender : IMessageSender
    {
        public IRfcGatewayService RfcClient { get; set; }

        public string SapSystemName { get; set; }

        public IMessageSender MessageSender { get; set; }

        public async Task SendMessageAsync(object message)
        {
            var idoc_inbound_asynchronous_message = CastMessage<IDOC_INBOUND_ASYNCHRONOUS_Message>(message);

            var rfcClient = GetRfcClient();
            var sapSystemName = GetSapSystemName();

            var idocs = await IdocBuilder.BuildIdocsAsync(rfcClient, sapSystemName, idoc_inbound_asynchronous_message.IDOC_INBOUND_ASYNCHRONOUS);

            foreach (var idoc in idocs)
            {
                var jIdoc = JIdocBuilder.CreateJIdoc(idoc);

                await MessageSender.SendMessageAsync(jIdoc);
            }
        }

        private IRfcGatewayService GetRfcClient()
        {
            var rfcClient = RfcClient;
            if (rfcClient == null)
                rfcClient = ProcessContext.GetCurrent().GetService<IRfcGatewayService>();

            if (rfcClient == null)
                throw new Exception("No rfc client available");
            return rfcClient;
        }

        private string GetSapSystemName()
        {
            var sapSystemName = SapSystemName;
            if (sapSystemName == null)
                sapSystemName = ProcessContext.GetCurrent().GetService<ISapSystem>()?.Name;
            return sapSystemName;
        }

        private T CastMessage<T>(object message) { 
            switch(message)
            {
                case string stringMessage:
                    return JsonConvert.DeserializeObject<T>(stringMessage);
                case JObject jObjectMessage:
                    return jObjectMessage.ToObject<T>();
                case T tMessage:
                    return tMessage;
                default:
                    return (T)message;
            }
        }
    }
}
