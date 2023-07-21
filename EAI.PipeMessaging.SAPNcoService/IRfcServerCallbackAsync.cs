using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public interface IRfcServerCallbackAsync
    {
        Task ApplicationErrorAsync(Exception error);
        Task InvokeFunctionAsync(string functionName, JObject functionData);
        Task ServerErrorAsync(Exception error);
        Task StateChangedAsync(RfcServerStateEnum oldState, RfcServerStateEnum newState);
    }
}