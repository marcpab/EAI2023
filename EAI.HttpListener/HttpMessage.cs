using EAI.Messaging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAI.HttpListener
{
    internal class HttpMessage : IMessageType, IMessageTransactionKey
    {
        public string _method;
        public string _uri;
        public string _version;
        public Dictionary<string, string> _headers;
        public byte[] _content;

        public string MessageType => _uri;

        public string TransactionKey => _uri;
    }
}
