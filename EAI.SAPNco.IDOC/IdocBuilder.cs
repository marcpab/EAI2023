using EAI.Abstraction.SAPNcoService;
using EAI.SAPNco.IDOC.Model.Structure;
using EAI.SAPNco.IDOC.Model;
using System.Collections.Generic;
using System.Threading.Tasks;
using EAI.SAPNco.IDOC.Metadata;
using System.Linq;

namespace EAI.SAPNco.IDOC
{
    public class IdocBuilder
    {
        public static async Task<IEnumerable<Idoc>> BuildIdocsAsync(IRfcGatewayService rfcClient, string sapSystemName, IDOC_INBOUND_ASYNCHRONOUS idoc_inbound_asynchronous)
        {
            var idocList = new List<Idoc>();

            foreach (var dc40 in idoc_inbound_asynchronous.IDOC_CONTROL_REC_40)
            {
                Idoc idoc = await BuildIdocAsync(rfcClient, sapSystemName, idoc_inbound_asynchronous, dc40);

                idocList.Add(idoc);
            }

            return idocList;
        }

        public static async Task<Idoc> BuildIdocAsync(IRfcGatewayService rfcClient, string sapSystemName, IDOC_INBOUND_ASYNCHRONOUS idoc_inbound_asynchronous, EDI_DC40 dc40)
        {
            var idocMetadata = await IdocMetadataCache.GetIdocMetadataAsync(rfcClient, sapSystemName, dc40.IDOCTYP, dc40.CIMTYP, dc40.DOCREL);

            var idoc = new Idoc(idocMetadata, dc40);

            var segmentMap = new Dictionary<string, IIdocSegmentContainer>
                {
                    { "000000", idoc }
                };

            foreach (var dd40 in idoc_inbound_asynchronous
                                    .IDOC_DATA_REC_40
                                    .Where<EDI_DD40>(dd => dd.DOCNUM == dc40.DOCNUM))
            {
                var pSegmentContainer = segmentMap[dd40.PSGNUM];

                var segment = pSegmentContainer.AddSegment(dd40);

                segmentMap.Add(dd40.SEGNUM, segment);
            }

            return idoc;
        }
    }
}
