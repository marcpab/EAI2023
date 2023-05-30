using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EAI.PipeMessaging
{
    public class PipeExceptionMessage
    {
        public string _originalType;
        public string _message;
        public string _stackTrace;
        public string _source;
        public string _helpLink;
        public int _hResult;
        public string _targetSite;

        public PipeExceptionMessage _innerException;

        internal static PipeExceptionMessage FromException(Exception ex)
        {
            if(ex == null) 
                return null;

            return new PipeExceptionMessage()
            {
                _originalType = ex.GetType().FullName,
                _message = ex.Message,
                _stackTrace = ex.StackTrace,
                _source = ex.Source,
                _helpLink = ex.HelpLink,
                _hResult = ex.HResult,
                _targetSite = ex.TargetSite?.ToString(),

                _innerException = FromException(ex.InnerException)
            };
        }

        internal static Exception ToException(PipeExceptionMessage ex)
        {
            if (ex == null)
                return null;

            var c = new StreamingContext(StreamingContextStates.CrossProcess);

            var si = new SerializationInfo(typeof(PipeException), new FormatterConverter());

            si.AddValue("ClassName", ex._originalType);
            si.AddValue("Message", ex._message);
            si.AddValue("InnerException", ToException(ex._innerException));
            si.AddValue("HelpURL", ex._helpLink);
            si.AddValue("StackTraceString", ex._stackTrace);
            si.AddValue("RemoteStackTraceString", ex._stackTrace);
            si.AddValue("RemoteStackIndex", 0);
            si.AddValue("ExceptionMethod", ex._targetSite);
            si.AddValue("HResult", ex._hResult);
            si.AddValue("Source", ex._source);
            si.AddValue("WatsonBuckets", null);

            return PipeException.Deserialize(si, c);
        }
    }
}
