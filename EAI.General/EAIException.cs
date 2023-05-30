using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EAI.General
{
    public class EAIException : Exception
    {
        public EAIException()
        {
        }

        public EAIException(string message) : base(message)
        {
        }

        public EAIException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected EAIException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
