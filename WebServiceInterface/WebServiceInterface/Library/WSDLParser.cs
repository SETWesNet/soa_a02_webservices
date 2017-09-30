using System;
using System.Xml;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebServiceInterface.Library;

namespace WebServiceInterface
{
    class WSDLInformation
    {
        public string BaseName { get; set; }

        public WSDLService Service { get; set; }
        public WSDLPort Port { get; set; }
        public List<WSDLOperation> Operations { get; set; }

        public WSDLInformation()
        {
            Service = new WSDLService();
            Port = new WSDLPort();
            Operations = new List<WSDLOperation>();
        }

        public WSDLOperation FindOperationByName(string name)
        {
            return Operations.Cast<WSDLOperation>().First(o => o.Name == name) ;
        }

        public string GetParameterTypeByName(string param)
        {
            string result = "";

            if (param.Contains("SoapIn"))
            {
                result = "IN";
            }
            else if (param.Contains("SoapOut"))
            {
                result = "OUT";
            }

            return result;
        }

        public WSDLOperation FindOperationByParameter(string input)
        {

            WSDLOperation result = null;

            if (input.Contains("SoapIn"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Input.Replace("tns:", "") == input);
            }
            else if (input.Contains("SoapOut"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Output.Replace("tns:", "") == input);
            }

            // TODO(c-jm): Replace this with a helper method in a static class
            return result;
        }
               
    }
    class WSDLOperation
    {
        public string Name { get; set; }
        public string SoapAction { get; set; }

        public string Input { get; set; }
        public string InputMessage { get; set; }
        public string Output { get; set; }
        public string OutputMessage { get; set; }
        public string Documentation { get; set; }

        public WSDLOperation(string name, string soapAction)
        {
            this.Name = name;
            this.SoapAction = soapAction;
            this.Input = "";
            this.Output = "";
        }
    }
    class WSDLService
    {
        public string Name { get; set; }
    }
    class WSDLPort
    {
        public string Name { get; set; }
        public string Binding { get; set; }
    }

    class WSDLParser
    {
        private XmlDocument _wsdlRaw;
        private XmlNodeList _portNodes;
        private XmlNodeList _bindingNodes;
        private XmlNodeList _portTypeNodes;
        private XmlNodeList _messageNodes;


        const string SOAP_SUFFIX = "Soap";
        const string SOAP_12_SUFFIX = "Soap12";

        public WSDLParser(XmlDocument wsdl)
        {
            _wsdlRaw = wsdl;
            _portNodes =     _wsdlRaw.GetElementsByTagName("wsdl:port");
            _bindingNodes =  _wsdlRaw.GetElementsByTagName("wsdl:binding");
            _portTypeNodes = _wsdlRaw.GetElementsByTagName("wsdl:portType");
            _messageNodes = _wsdlRaw.GetElementsByTagName("wsdl:message");
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
    }
}

