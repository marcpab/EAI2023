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
using EAI.SAPNco.IDOC.Metadata;
using EAI.SAPNco.IDOC.Model.Structure;
using EAI.LoggingV2;
using EAI.LoggingV2.Levels;

namespace EAI.SAPNco.IDOC.MessageSender
{
    public class IdocMessageSender : IMessageSender
    {
        public IRfcGatewayService RfcClient { get; set; }

        public string SapSystemName { get; set; }

        public IdocDestination[] IdocDestinations { get; set; }

        public LoggerV2 Log { get; set; }

        public Task SendMessageAsync(object message, string messageType, string transactionKey)
        {
            return SendMessageAsync(message);
        }

        public async Task SendMessageAsync(object message)
        {
            var idoc_inbound_asynchronous_message = CastMessage<IDOC_INBOUND_ASYNCHRONOUS_Message>(message);
            var idoc_inbound_asynchronous = idoc_inbound_asynchronous_message.IDOC_INBOUND_ASYNCHRONOUS;

            foreach (var dc40 in idoc_inbound_asynchronous.IDOC_CONTROL_REC_40)
                await SendIdoc(idoc_inbound_asynchronous, dc40);
        }

        private async Task SendIdoc(IDOC_INBOUND_ASYNCHRONOUS idoc_inbound_asynchronous, EDI_DC40 dc40)
        {
            using (var _ = new ProcessScope(null, null, GetType().FullName))
                try
                {
                    Log?.Start<Info>(nameof(dc40), dc40, $"Processing idoc {dc40.GetIdocNumber()}");

                    var idocDestination = GetIdocDestination(dc40);

                    Log?.String<Info>($"Send idoc to destination {idocDestination} => {idocDestination.MessageSender}");

                    if (idocDestination.MessageFilters == null)
                    {
                        Log?.String<Warning>("No message sender defined, discarding idoc");
                        return;
                    }

                    switch (idocDestination.Format)
                    {
                        case IdocFormatEnum.IDOC_INBOUND_ASYNCHRONOUS:
                            await SendAsIDOC_INBOUND_ASYNCHRONOUS(idoc_inbound_asynchronous, dc40, idocDestination.MessageSender);

                            break;
                        case IdocFormatEnum.json:
                            await SendAsJidoc(idoc_inbound_asynchronous, dc40, idocDestination.MessageSender);

                            break;
                        case IdocFormatEnum.xml:
                            throw new NotImplementedException("not implemented yet: xml");

                        default:
                            throw new NotImplementedException();
                    }

                    Log?.Success<Info>();

                }
                catch (Exception ex)
                {
                    Log?.Failed<Error>(ex);
                }
        }

        private async Task SendAsIDOC_INBOUND_ASYNCHRONOUS(IDOC_INBOUND_ASYNCHRONOUS idoc_inbound_asynchronous, EDI_DC40 dc40, IMessageSender messageSender)
        {
            var singleIDOC_INBOUND_ASYNCHRONOUS = new IDOC_INBOUND_ASYNCHRONOUS_Message()
            {
                IDOC_INBOUND_ASYNCHRONOUS = new IDOC_INBOUND_ASYNCHRONOUS()
                {
                    IDOC_CONTROL_REC_40 = new List<EDI_DC40>() { dc40 },
                    IDOC_DATA_REC_40 = idoc_inbound_asynchronous.IDOC_DATA_REC_40.Where(d => d.DOCNUM == dc40.DOCNUM).ToList()
                }
            };

            Log.Message<Debug>(nameof(singleIDOC_INBOUND_ASYNCHRONOUS), singleIDOC_INBOUND_ASYNCHRONOUS, "send idoc");

            await messageSender.SendMessageAsync(singleIDOC_INBOUND_ASYNCHRONOUS, dc40.GetIdocType(), dc40.DOCNUM);
        }

        private async Task SendAsJidoc(IDOC_INBOUND_ASYNCHRONOUS idoc_inbound_asynchronous, EDI_DC40 dc40, IMessageSender messageSender)
        {
            var rfcClient = GetRfcClient();
            var sapSystemName = GetSapSystemName();

            var idoc = await IdocBuilder.BuildIdocAsync(rfcClient, sapSystemName, idoc_inbound_asynchronous, dc40);

            var jIdoc = JIdocBuilder.CreateJIdoc(idoc);

            Log.Message<Debug>(nameof(jIdoc), jIdoc, "send jidoc");

            await messageSender.SendMessageAsync(jIdoc, idoc.Type, idoc.Number);
        }

        private IdocDestination GetIdocDestination(EDI_DC40 dc40)
        {
            var idocType = dc40.GetIdocType();

            var idocDestination = IdocDestinations.Where(d => d.IsMatch(idocType)).FirstOrDefault();

            if (idocDestination == null)
                throw new Exception($"No idoc destination found for type {idocType}");

            return idocDestination;
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

        private T CastMessage<T>(object message)
        {
            switch (message)
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
