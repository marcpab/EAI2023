using EAI.General.Cache;
using EAI.SAPNco.IDOC.Model.Structure;
using EAI.SAPNco.IDOC.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EAI.Abstraction.SAPNcoService;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocMetadataCache
    {
        public static async Task<IdocMetadata> GetIdocMetadataAsync(IRfcGatewayService rfcClient, string sapSystemName, string idoctyp, string cimtyp, string docrel)
        {
            var key = $"{sapSystemName}/{idoctyp}/{cimtyp}/{docrel}";

            var idocMetadata = await ResourceCache<IdocMetadata>.GetResourceAsync(
                key, async () => new ResourceCacheItem<IdocMetadata>(await CreateIdocMetadataAsync(rfcClient, sapSystemName, idoctyp, cimtyp, docrel))
                {
                    ExpiresOn = DateTime.UtcNow.AddMinutes(20),
                }
                );

            return idocMetadata;
        }

        public static async Task<IdocMetadata> CreateIdocMetadataAsync(IRfcGatewayService rfcClient, string sapSystemName, string idoctyp, string cimtyp, string docrel)
        {
            var idoctype_read_complete = new IDOCTYPE_READ_COMPLETE_Message()
            {
                IDOCTYPE_READ_COMPLETE = new IDOCTYPE_READ_COMPLETE()
                {
                    PI_IDOCTYP = idoctyp,
                    PI_CIMTYP = cimtyp,
                    PI_RELEASE = docrel,
                    PI_VERSION = "3",
                    PI_READ_UNREL = "X"
                }
            };

            idoctype_read_complete = await rfcClient.CallRfcAsync(sapSystemName, idoctype_read_complete, true);

            return IdocMetadata.FromIDOCTYPE_READ_COMPLETE(idoctype_read_complete.IDOCTYPE_READ_COMPLETE_Response);
        }
    }
}
