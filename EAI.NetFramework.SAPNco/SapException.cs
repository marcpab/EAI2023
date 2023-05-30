using System;
using System.Runtime.Serialization;

namespace EAI.NetFramework.SAPNco
{
    [Serializable]
    internal class SapException : Exception
    {
        public SapException()
        {
        }

        public SapException(string message) : base(message)
        {
        }

        public SapException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected SapException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}