using EAI.OnPrem.Storage;
using System;

namespace EAI.OnPrem.SAPNcoService
{
    public class CallRfcRequest : OnPremMessage
    {
        public string _name;
        public string _jRfcRequestMessage;
        public bool _autoCommit;
    }
}
