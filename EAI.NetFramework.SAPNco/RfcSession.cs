namespace EAI.NetFramework.SAPNco
{
    class RfcSession
    {
        private readonly string _sessionId;

        public RfcSession(string sessionId)
        {
            _sessionId = sessionId;
        }

        public string SessionId { get => _sessionId; }
        public bool IsActive { get; set; }
    }

}
