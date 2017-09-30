using System;
using System.Xml;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebServiceInterface.Library;
using WebServiceInterface.Library.WSDL;

namespace WebServiceInterface
{
    class WSDLParser
    {
        private XmlDocument _wsdlRaw;
        private XmlNodeList _portNodes;
        private XmlNodeList _bindingNodes;
        private XmlNodeList _portTypeNodes;
        private XmlNodeList _messageNodes;
        private XmlNodeList _complexTypeNodes;


        const string SOAP_SUFFIX = "Soap";
        const string SOAP_12_SUFFIX = "Soap12";

        public WSDLParser(XmlDocument wsdl)
        {
            _wsdlRaw = wsdl;
            _portNodes =     _wsdlRaw.GetElementsByTagName("wsdl:port");
            _bindingNodes =  _wsdlRaw.GetElementsByTagName("wsdl:binding");
            _portTypeNodes = _wsdlRaw.GetElementsByTagName("wsdl:portType");
            _messageNodes = _wsdlRaw.GetElementsByTagName("wsdl:message");
            _complexTypeNodes = _wsdlRaw.GetElementsByTagName("s:complexType");
        }

        private string TrimNamespace(string s)
        {
            return s.Replace("tns:", "");
        }

        public WSDLInformation GetPort(WSDLInformation info)
        {
            WSDLPort port = new WSDLPort();

            XmlNode first = _portNodes.Cast<XmlNode>().First().ParentNode;

            if (first.Attributes["name"] != null)
            {
                info.Service.Name = first.Attributes["name"].InnerText;
                info.BaseName = info.Service.Name;
            }

            string portKey = info.Service.Name += SOAP_12_SUFFIX;

            XmlNode portNode = _portNodes.Cast<XmlNode>().First(p => p.Attributes["name"].InnerText == portKey);

            if (portNode != null)
            {
                if (portNode.Attributes["name"] != null && portNode.Attributes["binding"] != null)
                {
                    info.Port.Name = TrimNamespace(portNode.Attributes["name"].Value);
                    info.Port.Binding = TrimNamespace(portNode.Attributes["binding"].InnerText);
                }
            }
            else
            {
                string message = String.Format("Couldn't find wsdl:port {0}", portKey);
                throw new Exception(message);
            }

            return info;
        }
        
        public WSDLInformation GetOperations(WSDLInformation info)
        {
            string binding = info.Port.Binding;
            XmlNode bindingNode = _bindingNodes.Cast<XmlNode>().First(e => e.Attributes["name"].InnerText == binding);

            info.Operations =  bindingNode.ChildNodes.Cast<XmlNode>()
                               .Where(e => e.Name == "wsdl:operation")
                               .Select(e => new WSDLOperation(e.Attributes["name"].InnerText, e.FirstChild.Attributes["soapAction"].InnerText))
                               .ToList();

            return info;
        }

        public WSDLInformation GetPortTypes(WSDLInformation info)
        {
            string portTypeKey = info.BaseName += SOAP_SUFFIX;

            XmlNode portTypeNode = _portTypeNodes.Cast<XmlNode>()
                                       .First(portType => portType.Attributes["name"].InnerText == portTypeKey);

            List<XmlNode> operations = portTypeNode.ChildNodes.Cast<XmlNode>().ToList();

            foreach(XmlNode o in operations)
            {
                string operationName = o.Attributes["name"].InnerText;

                WSDLOperation operation = info.FindOperationByName(operationName);

                operation.Documentation = o.ChildNodes.Cast<XmlNode>()
                                          .First(e => e.Name == "wsdl:documentation")
                                          .InnerText;

                operation.Input  =        o.ChildNodes.Cast<XmlNode>()
                                          .First(e => e.Name == "wsdl:input")
                                          .Attributes["message"].InnerText;

                operation.Output  = o.ChildNodes.Cast<XmlNode>()
                                          .First(e => e.Name == "wsdl:output")
                                          .Attributes["message"].InnerText;
            }

            return info;
        }

        public WSDLInformation GetMessages(WSDLInformation info)
        {
            List<XmlNode> soapMessages = _messageNodes.Cast<XmlNode>()
                                           .Where(n => n.Attributes["name"].InnerText.Contains("Soap"))
                                           .ToList();

            foreach(XmlNode message in soapMessages)
            {
                string name = message.Attributes["name"].InnerText;

                string type = info.GetParameterTypeByName(name);

                WSDLOperation operation = info.FindOperationByParameter(name);

                // NOTE(c-jm): We are making an assumption that messages only have ONE part! This is the case on webServiceX

                XmlNode partNode = message.FirstChild;

                if (type == "IN")
                {
                    operation.InputMessage = partNode.Attributes["element"].InnerText;
                }
                else if (type == "OUT")
                {
                    operation.OutputMessage = partNode.Attributes["element"].InnerText;
                }
            }

            return info; 
        }

        public List<WSDLTypeInformation> BuildTypeInformation()
        {
            List<WSDLTypeInformation> result = new List<WSDLTypeInformation>();

            foreach(XmlNode complexType in _complexTypeNodes)
            {
                WSDLTypeInformation typeInformation = new WSDLTypeInformation();
                XmlNode parent = complexType.ParentNode;

                if (parent == null)
                {
                    typeInformation.Name = complexType.Attributes["name"].InnerText;
                }
                else
                {
                    typeInformation.Name = parent.Attributes["name"].InnerText; 
                }

                typeInformation.Types = complexType.ChildNodes.Cast<XmlNode>()
                           .First(e => e.Name == "s:sequence")
                           .ChildNodes.Cast<XmlNode>()
                           .Where(e => e.Name == "s:element")
                           .Select(e => new WSDLType(e.Attributes["name"].InnerText, e.Attributes["type"].InnerText))
                           .Cast<WSDLType>()
                           .ToList();

                result.Add(typeInformation);
            }

            return result;
        }
    }
}

