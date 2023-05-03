using System;
using System.Runtime.Serialization;

namespace EAI.General.Exceptions
{

    public class AzureException : Exception
    {
        public AzureException() :
            base()
        {
        }

        public AzureException(string message) :
            base(message)
        {
        }

        protected AzureException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }

        public AzureException(string message, Exception innerException) :
            base(message, innerException)
        {
        }

    }
}