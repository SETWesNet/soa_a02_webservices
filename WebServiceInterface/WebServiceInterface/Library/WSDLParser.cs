/* 
 *  
 *  Filename: WSDLParser.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLParser class
 * 
 */

using System.Xml;
using System.Linq;
using System.Collections.Generic;
using WebServiceInterface.Library;
using WebServiceInterface.Library.WSDL;
using WebServiceInterface.Extension_Methods;

namespace WebServiceInterface
{
    /// <summary>
    /// A WSDL Parser holds all the parsing logic for dealing with WSDLs. It has the ability to generate TypeInformation for a wsdl and WSDLInformation
    /// </summary>
    class WSDLParser
    {
        /* 
         * These are XmlDocument node collections that we use for parsing every section of the WSDL schema.
         * All of the names line up with a wsdl:$type node in the document.
         */
        private XmlDocument _wsdlRaw;
        private XmlNodeList _portNodes;
        private XmlNodeList _bindingNodes;
        private XmlNodeList _portTypeNodes;
        private XmlNodeList _messageNodes;
        private XmlNodeList _complexTypeNodes;
        private XmlNodeList _schemaNodes;



        /// <summary>
        /// Initializes a new instance of the <see cref="WSDLParser"/> class.
        /// </summary>
        /// <param name="wsdl">The WSDL.</param>
        public WSDLParser(XmlDocument wsdl)
        {
            _wsdlRaw = wsdl;
            _portNodes = _wsdlRaw.GetElementsByTagName("wsdl:port");
            _bindingNodes = _wsdlRaw.GetElementsByTagName("wsdl:binding");
            _portTypeNodes = _wsdlRaw.GetElementsByTagName("wsdl:portType");
            _messageNodes = _wsdlRaw.GetElementsByTagName("wsdl:message");
            _complexTypeNodes = _wsdlRaw.GetElementsByTagName("s:complexType");
            _schemaNodes = _wsdlRaw.GetElementsByTagName("s:schema");
        }

        /// <summary>
        /// Gets the namespace from the WSDL XmlStructure.
        /// </summary>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        public WSDLInformation GetNamespace(WSDLInformation info)
        {
            XmlNode schema = _schemaNodes.Cast<XmlNode>().FirstOrDefault();

            info.Namespace = schema.GetTextAttribute("targetNamespace", "wsdl:schema");

            return info;
        }

        /// <summary>
        /// Retrieves and parses the Port from the WSDL Xml structure
        /// </summary>
        /// <param name="info">The WSDL Information we are parsing out </param>
        /// <returns></returns>
        public WSDLInformation GetPort(WSDLInformation info)
        {
            WSDLPort port = new WSDLPort();

            XmlNode first = _portNodes.Cast<XmlNode>().First().ParentNode;

            info.Service.Name = first.GetTextAttribute("name", "wsdl:service");
            info.BaseName = info.Service.Name;

            string portKey = info.Service.Name += WSDLHelpers.SOAP_12_SUFFIX;

            XmlNode portNode = _portNodes.Cast<XmlNode>().First(p => p.GetTextAttribute("name", "wsdl:portNode") == portKey);

            info.Port.Name =    WSDLHelpers.TrimNamespace(portNode.GetTextAttribute("name", "wsdl:portNode"));
            info.Port.Binding = WSDLHelpers.TrimNamespace(portNode.GetTextAttribute("binding", "wsdl:portNode"));
            info.Port.Location = portNode.FirstChild.GetTextAttribute("location", "soap:address");

            return info;
        }
    
        
        /// <summary>
        /// Goes the WSDL operations xml structure and returns WSDLOperations  
        /// </summary>
        /// <param name="info">The WSDLInformation we are filing out</param>
        /// <returns>The filed out WSDLinformation</returns>
        public WSDLInformation GetOperations(WSDLInformation info)
        {
            // Grab the binding and find the bindng that we want 
            string binding = info.Port.Binding;
            XmlNode bindingNode = _bindingNodes.Cast<XmlNode>().First(e => e.GetTextAttribute("name", "wsdl:binding") == binding);

            // Go through each operation and create an object out of the name and the soapAction 

            info.Operations = bindingNode.ChildNodes.Cast<XmlNode>()
                               .Where(e => e.Name == "wsdl:operation")
                               .Select(e => new WSDLOperation(e.GetTextAttribute("name", "wsdl:operation"), e.FirstChild.GetTextAttribute("soapAction", "wsdl:part")))
                               .ToList();

            return info;
        }

        /// <summary>
        /// Retrieves and parses the portTypes XML structure from the WSDL
        /// </summary>
        /// <param name="info">The info to fill out</param>
        /// <returns></returns>
        public WSDLInformation GetPortTypes(WSDLInformation info)
        {
            string portTypeKey = info.BaseName += WSDLHelpers.SOAP_SUFFIX;

            XmlNode portTypeNode = _portTypeNodes.Cast<XmlNode>()
                                       .First(portType => portType.GetTextAttribute("name", "wsdl:portType") == portTypeKey);

            List<XmlNode> operations = portTypeNode.ChildNodes.Cast<XmlNode>().ToList();

            foreach(XmlNode o in operations)
            {
                string operationName = o.GetTextAttribute("name", "wsdl:operation");

                // Find the operation for this tag.
                WSDLOperation operation = info.FindOperationByName(operationName);

                // Grab the documentation tag, it is optiona.
                XmlNode documentationNode = o.ChildNodes.Cast<XmlNode>().FirstOrDefault(e => e.Name == "wsdl:documentation");
                if (operation.Documentation != null)
                {
                    operation.Documentation = documentationNode.InnerText;
                }

                // Get the input node and fill it out with whatever is in the tag.
                XmlNode inputNode = o.ChildNodes.Cast<XmlNode>().First(e => e.Name == "wsdl:input");
                operation.Input = inputNode.GetTextAttribute("message", "wsdl:input");


                // Get the output node and fill it out with whatever is in the tag.
                XmlNode outputNode = o.ChildNodes.Cast<XmlNode>().First(e => e.Name == "wsdl:output");
                operation.Output   = outputNode.GetTextAttribute("message", "wsdl:input");

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
                                           .Where(n => n.GetTextAttribute("name", "wsdl:message").Contains("Soap"))
                                           .ToList();


            // Loop through all the messages we get back.
            foreach(XmlNode message in soapMessages)
            {
                // Grab the name of the message and find the type (whether its an input or output type message).
                string name = message.GetTextAttribute("name", "wsdl:message");

                string type = info.GetParameterTypeByName(name);

                // Find the operation that we are dealing with
                WSDLOperation operation = info.FindOperationByParameter(name);

                // Grab the first part we are assuming we only have one part.
                XmlNode partNode = message.FirstChild;

                // Determine if its an input or output message and store accordingly.
                if (type == "IN")
                {
                    operation.InputMessage = partNode.GetTextAttribute("element", "wsdl:part");
                }
                else if (type == "OUT")
                {
                    operation.OutputMessage = partNode.GetTextAttribute("element", "wsdl:part");
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

            // Now for each complex type we want to build a WSDLTYpe out of that.
            foreach(XmlNode complexType in _complexTypeNodes)
            {
                WSDLTypeInformation typeInformation = new WSDLTypeInformation();

                // Grab the parent of the complex node. If there is a parent it is an s:element which has the name.
                XmlNode parent = complexType.ParentNode;

                if (parent == null || parent.Attributes["name"] == null)
                {
                    if (complexType.Attributes["name"] == null)
                    {
                        typeInformation.Name = "N/A";
                    }
                    else
                    {
                        typeInformation.Name = complexType.GetTextAttribute("name", "wsdl:complexType");
                    }
                }
                else
                {
                    // If it deosnt have a parent then the name is purely on s:complexNode
                    typeInformation.Name = parent.GetTextAttribute("name", "wsdl:complexType"); 
                }

                // Grab the first sequence in the complex node.
                XmlNode sequence = complexType.ChildNodes.Cast<XmlNode>().FirstOrDefault(e => e.Name == "s:sequence");

                if (sequence != null)
                {
                    // Then select all the s:elements within that complex node and convert them into WSDLTypes
                    typeInformation.Types = sequence.ChildNodes.Cast<XmlNode>()
                               .Where(e => e.Name == "s:element")
                               .Select(e => new WSDLType(e.GetTextAttribute("name", "s:element"), e.GetTextAttribute("type", "wsdl:selement")))
                               .Cast<WSDLType>()
                               .ToList();
                }
                else
                {
                    typeInformation.Types = new List<WSDLType>(); 
                }

                // We then add that to the list within the Typeinfromation so it is a list of types keyed by a name 
                result.Add(typeInformation);
            }

            return result;
        }
        
        /// <summary>
        /// Helper convience method that parses the XML document in the correct order. 
        /// </summary>
        /// <returns>A WSDLInformation object based on the wsdl parsed</returns>
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

