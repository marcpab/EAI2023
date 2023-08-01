using EAI.SAPNco.IDOC.Model.Structure;

namespace EAI.SAPNco.IDOC.Metadata
{
    public class IdocMessageMetadata
    {
        private IdocMetadata _idocMetadata;
        private EDI_IAPI17 _message;

        public IdocMessageMetadata(IdocMetadata idocMetadata, EDI_IAPI17 message)
        {
            _idocMetadata = idocMetadata;
            _message = message;
        }
    }
}
