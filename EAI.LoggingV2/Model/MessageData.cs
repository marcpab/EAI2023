using EAI.General;
using EAI.General.Extensions;
using System;
using System.Text;

namespace EAI.LoggingV2.Model
{
    public class MessageData
    {
        public string _name;
        public string _type;
        public byte[] _hash;
        public string _content;
        public string _contentType;

        public MessageData(string name, string type, string contentType, string content)
        {
            _name = name;
            _type = type;
            _contentType = contentType;
            _content = content;

            if(_content != null)
                _hash = content.CalculateSipHash();
        }
    }
}
