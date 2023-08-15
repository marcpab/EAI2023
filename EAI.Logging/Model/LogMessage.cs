using System;
using System.Text;
using System.Xml.Linq;
using System.Xml;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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

        public LogMessage(string operation, object content)
        {
            Operation = string.IsNullOrWhiteSpace(operation) ? null : operation;
            (MsgType, Content) = DeductMessageType(content);
        }

        private static (LogMessageType MsgType, string Content) DeductMessageType(object content)
        {
            LogMessageType msgType = LogMessageType.RAW;
            string contentString = EAI.Texts.Properties.NULL;

            if (content is null)
            {
                return (msgType, contentString);
            }

            if (content is XElement xElement)
            {
                msgType = LogMessageType.XML;
                contentString = xElement.ToString();
                return (msgType, contentString);
            }

            if (content is XmlNode xmlNode)
            {
                msgType = LogMessageType.XML;
                contentString = xmlNode.OuterXml;
                return (msgType, contentString);
            }

            if (content is JToken jToken)
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

                try
                {
                    xElement = XElement.Parse(stringMessage);
                    if (xElement != null)
                    {
                        msgType = LogMessageType.XML;
                        contentString = xElement.ToString();
                        return (msgType, contentString);
                    }
                }
                catch { }

                try
                {
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


            if(msgType == LogMessageType.RAW)
            {
                try
                {
                    msgType = LogMessageType.JSON;
                    contentString = JsonConvert.SerializeObject(content);
                }
                catch { }
            }

            return (msgType, "[Unkown Message]");
        }

    }
}
