using Newtonsoft.Json.Linq;
using System;

namespace EAI.NetFramework.SAPNco
{
    public interface IRfcServerCallback
    {
        void ApplicationError(Exception error);
        void InvokeFunction(string functionName, JObject function);
        void ServerError(Exception error);
        void StateChanged(RfcServerState oldState, RfcServerState newState);
    }
}