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
        private XmlNodeList _schemaNodes;


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
            _schemaNodes = _wsdlRaw.GetElementsByTagName("s:schema");
        }

        private string TrimNamespace(string s)
        {
            return s.Replace("tns:", "");
        }

        public WSDLInformation GetNamespace(WSDLInformation info)
        {
            XmlNode node = _schemaNodes.Cast<XmlNode>().FirstOrDefault();

            info.Namespace = node.Attributes["targetNamespace"].InnerText;

            return info;
        }

        /// <summary>
        /// Gets the SOAP12 port for 
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
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
                    info.Port.Name =    TrimNamespace(portNode.Attributes["name"].Value);
                    info.Port.Binding = TrimNamespace(portNode.Attributes["binding"].InnerText);

                    if (portNode.FirstChild.Attributes["location"] != null)
                    {
                        info.Port.Location = portNode.FirstChild.Attributes["location"].InnerText;
                    }
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

                XmlNode documentationNode = o.ChildNodes.Cast<XmlNode>().FirstOrDefault(e => e.Name == "wsdl:documentation");
                if (operation.Documentation != null)
                {
                    operation.Documentation = documentationNode.InnerText;
                }

                operation.Input  =        o.ChildNodes.Cast<XmlNode>()
                                          .First(e => e.Name == "wsdl:input")
                                          .Attributes["message"].InnerText;

                operation.Output  = o.ChildNodes.Cast<XmlNode>()
                                          .First(e => e.Name == "wsdl:output")
                                          .Attributes["message"].InnerText;
            }

            return info;
        }

        /// <summary>
        /// Get all wsdl:messages and apply them to predefined operations 
        /// </summary>
        /// <param name="info"> The WSDLInformation that we modifying </param>
        /// <returns></returns>
        public WSDLInformation GetMessages(WSDLInformation info)
        {
            List<XmlNode> soapMessages = _messageNodes.Cast<XmlNode>()
                                           .Where(n => n.Attributes["name"].InnerText.Contains("Soap"))
                                           .ToList();

            foreach(XmlNode message in soapMessages)
            {
                // Grab the name of the message and find the type (whether its an input or output type message).
                string name = message.Attributes["name"].InnerText;

                string type = info.GetParameterTypeByName(name);

                // Find the operation that we are dealing with
                WSDLOperation operation = info.FindOperationByParameter(name);

                // Grab the first part.
                XmlNode partNode = message.FirstChild;

                // Determine if its an input or output message and store accordingly.
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

        /// <summary>
        /// Since types need to be handled differently then other nodes in the tree. We build the type ifnormation seperately returns a list of WSDLTypeInformation 
        /// </summary>
        /// <returns></returns>
        private List<WSDLTypeInformation> BuildTypeInformation()
        {
            List<WSDLTypeInformation> result = new List<WSDLTypeInformation>();

            foreach(XmlNode complexType in _complexTypeNodes)
            {
                WSDLTypeInformation typeInformation = new WSDLTypeInformation();

                // Grab the parent of the complex node. If there is a parent it is an s:element which has the name.
                XmlNode parent = complexType.ParentNode;

                if (parent == null)
                {
                    typeInformation.Name = complexType.Attributes["name"].InnerText;
                }
                else
                {
                    // If it deosnt have a parent then the name is purely on s:complexNode
                    typeInformation.Name = parent.Attributes["name"].InnerText; 
                }

                // Grab the first sequence in the complex node.
                XmlNode sequence = complexType.ChildNodes.Cast<XmlNode>().FirstOrDefault(e => e.Name == "s:sequence");

                if (sequence != null)
                {
                    // Then select all the s:elements within that complex node and convert them into WSDLTypes
                    typeInformation.Types = sequence.ChildNodes.Cast<XmlNode>()
                               .Where(e => e.Name == "s:element")
                               .Select(e => new WSDLType(e.Attributes["name"].InnerText, e.Attributes["type"].InnerText))
                               .Cast<WSDLType>()
                               .ToList();
                }

                // We then add that to the list within the Typeinfromation so it is a list of types keyed by a name 
                result.Add(typeInformation);
            }

            return result;
        }
        
        /// <summary>
        /// Helper convience method that parses the XML document in the correct order while applying the type
        /// </summary>
        /// <returns></returns>
        public WSDLInformation BuildWSDLInformation()
        {
            WSDLInformation wsdlInformation = new WSDLInformation();

            wsdlInformation = GetNamespace(wsdlInformation);
            wsdlInformation = GetPort(wsdlInformation);
            wsdlInformation = GetOperations(wsdlInformation);
            wsdlInformation = GetPortTypes(wsdlInformation);
            wsdlInformation = GetMessages(wsdlInformation);

            List<WSDLTypeInformation> typeInfo = BuildTypeInformation();

            wsdlInformation.ApplyTypeInformation(typeInfo);

            return wsdlInformation;
        }


    }
}

