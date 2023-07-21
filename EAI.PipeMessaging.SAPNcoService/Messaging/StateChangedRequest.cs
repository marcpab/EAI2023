namespace EAI.PipeMessaging.SAPNcoService.Messaging
{
    internal class StateChangedRequest
    {
        public RfcServerStateEnum _oldState { get; set; }
        public RfcServerStateEnum _newState { get; set; }
    }
}