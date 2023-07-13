using System;

namespace EAI.LoggingV2.Model
{
    public class ProcessData
    {
        public string _serviceName;
        public string _parentStage;
        public string _parentProcessId;
        public string _initialMessageKey;
        public StatusEnum _status;
        public DateTimeOffset? _finishOnUTC;
    }
}
