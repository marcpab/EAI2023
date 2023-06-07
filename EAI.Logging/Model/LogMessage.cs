using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;

namespace EAI.Logging.Model
{
    public class LogMessage
    {
        public string Content { get; private set; }
        public string Operation { get; private set; }
        public LogMessageType MsgType { get; private set; }

        public LogMessage(LogMessageType messageType, string operation, string content)
        {
            MsgType = messageType;
            Operation = string.IsNullOrWhiteSpace(operation) ? null : operation;
            Content = string.IsNullOrWhiteSpace(content) ? null : content;
        }

        private static (LogMessageType MsgType, string Content) DeductMessageType(object content)
        {
            LogMessageType msgType = LogMessageType.RAW;
            string contentString = EAI.Texts.Properties.NULL;

            if (content == null)
            {
                return (msgType, contentString);
            }

            var xElement = content as XElement;
            if (xElement != null)
            {
                msgType = LogMessageType.XML;
                contentString = xElement.ToString();
                return (msgType, contentString);
            }

            var xmlNode = content as XmlNode;
            if (xmlNode != null)
            {
                msgType = LogMessageType.XML;
                contentString = xmlNode.OuterXml;
                return (msgType, contentString);
            }

            var jToken = content as JToken;
            if (jToken != null)
            {
                msgType = LogMessageType.JSON;
                contentString = jToken.ToString(Newtonsoft.Json.Formatting.Indented);
                return (msgType, contentString);
            }

            var stringMessage = content as string;
            if (!string.IsNullOrWhiteSpace(stringMessage))
            {
                msgType = LogMessageType.TEXT;
                contentString = stringMessage;
            }

            try
            {
                xElement = XElement.Parse(stringMessage);
                if (xElement != null)
                {
                    msgType = LogMessageType.XML;
                    contentString = xElement.ToString();
                    return (msgType, contentString);
                }

                jToken = JToken.Parse(stringMessage);
                {
                    msgType = LogMessageType.JSON;
                    contentString = jToken.ToString();
                    return (msgType, contentString);
                }
            }
            catch { }

            return (msgType, contentString);
        }

        public LogMessage(string operation, object content)
        {
            Operation = string.IsNullOrWhiteSpace(operation) ? null : operation;
            (MsgType, Content) = DeductMessageType(content);            
        }
    }
}
