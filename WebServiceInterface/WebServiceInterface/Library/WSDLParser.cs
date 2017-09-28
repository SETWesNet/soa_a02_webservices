using System;
using System.Xml;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebServiceInterface.Library.WSDL;

namespace WebServiceInterface
{
    enum MessageType
    {
        IN,
        OUT
    }

    class WSDLParser
    {
        private XmlDocument _wsdlRaw;
        private XmlNodeList _operationNodes;
        private XmlNodeList _messageNodes;
        const string SOAP_IN_MESSAGE = "SoapIn";
        const string SOAP_OUT_MESSAGE = "SoapOut";

        public WSDLParser(XmlDocument wsdl)
        {
            _wsdlRaw = wsdl;
            _operationNodes = _wsdlRaw.GetElementsByTagName("wsdl:operation");
            _messageNodes   = _wsdlRaw.GetElementsByTagName("wsdl:message");
        }

        private bool IsSoapNode(XmlNode node)
        {
            string name = node.Attributes["name"].InnerText;

            return (name.Contains(SOAP_IN_MESSAGE) || name.Contains(SOAP_OUT_MESSAGE));
        }


        public List<Part> TransformParts(XmlNode messageNode)
        {
            List<Part> result = new List<Part>();

            XmlNodeList parts = messageNode.ChildNodes;

            foreach(XmlNode node in parts)
            {
                Part part = new Part();

                part.name = node.Attributes["name"].InnerText;
                part.type = node.Attributes["element"].InnerText;

                result.Add(part);
            }

            return result;
        }
        
        private string TrimWSDLSoapMessage(string s)
        {
            string result = "";

            if (s.Contains(SOAP_IN_MESSAGE))
            {
                result = s.Replace(SOAP_IN_MESSAGE, "");
            }
            else if (s.Contains(SOAP_OUT_MESSAGE))
            {
                result = s.Replace(SOAP_OUT_MESSAGE, "");
            }

            return result;
        }

        /// <summary>
        /// Transform message structure 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, MessageContainer> TransformMessages()
        {
            Dictionary<string, MessageContainer> messages = new Dictionary<string, MessageContainer>();

            foreach (XmlNode node in _messageNodes)
            {
                if (!IsSoapNode(node))
                    continue;

                Message message = null;
                string name = node.Attributes["name"].InnerText;
                string key = TrimWSDLSoapMessage(name);
                MessageContainer container = messages.ContainsKey(key) ? messages[key] : new MessageContainer();

                if (name.Contains(SOAP_IN_MESSAGE))
                {
                    message = container.ParameterMessage = new Message(name, MessageType.IN);
                }
               else if (name.Contains(SOAP_OUT_MESSAGE))
                {
                    message = container.ParameterMessage = new Message(name, MessageType.OUT);
                }

                message.parts = TransformParts(node);

                if (! messages.ContainsKey(key))
                {
                    messages.Add(key, container);
                }
            }

            return messages;
        }
    }
}


