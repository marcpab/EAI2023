using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace EAI.PipeMessaging
{
    public class PipeException : Exception
    {
        public PipeException()
        {
        }

        public PipeException(string message) : base(message)
        {
        }

        public PipeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PipeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static PipeException Deserialize(SerializationInfo info, StreamingContext context)
        {
            return new PipeException(info, context);
        }

    }
}
