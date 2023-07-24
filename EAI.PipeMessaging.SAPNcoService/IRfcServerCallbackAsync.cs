using EAI.PipeMessaging.SAPNcoService.Messaging;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace EAI.PipeMessaging.SAPNcoService
{
    public interface IRfcServerCallbackAsync
    {
        Task ApplicationErrorAsync(ExceptionData error);
        Task InvokeFunctionAsync(string functionName, JObject functionData);
        Task ServerErrorAsync(ExceptionData error);
        Task StateChangedAsync(RfcServerStateEnum oldState, RfcServerStateEnum newState);
    }
}