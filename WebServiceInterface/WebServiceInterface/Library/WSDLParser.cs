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

    class Type
    {
        public string Name { get; set; }
        public string Signature { get; set; }
    }

    class TypeContainer
    {
        public string Key { get; set; }
        public List<Type> Items;
    }

    class WSDLParser
    {
        private XmlDocument _wsdlRaw;
        private XmlNodeList _operationNodes;
        private XmlNodeList _messageNodes;
        private XmlNodeList _typeNodes;


        const string SOAP_IN_MESSAGE = "SoapIn";
        const string SOAP_OUT_MESSAGE = "SoapOut";

        public WSDLParser(XmlDocument wsdl)
        {
            _wsdlRaw = wsdl;
            _operationNodes = _wsdlRaw.GetElementsByTagName("wsdl:operation");
            _messageNodes   = _wsdlRaw.GetElementsByTagName("wsdl:message");
            _typeNodes      = _wsdlRaw.GetElementsByTagName("s:schema");
        }

        private bool IsSoapNode(XmlNode node)
        {
            string name = node.Attributes["name"].InnerText;

            return (name.Contains(SOAP_IN_MESSAGE) || name.Contains(SOAP_OUT_MESSAGE));
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
        /// Transforms a wsdl:part XML structure into a WSDL.Part object.
        /// </summary>
        /// <param name="partXML"> The part we are transforming</param>
        /// <returns></returns>
        private Part TransformPartXmlToObject(XmlNode partXML)
        {
            Part part = new Part();

            // TODO(c-jm): Add error handling here.
            part.Name = partXML.Attributes["name"].InnerText;
            part.Element = partXML.Attributes["element"].InnerText;

            return part;
        }
        /// <summary>
        /// Takes an XML node and transforms into a Message structure
        /// </summary>
        /// <param name="messageXML"> The node we are transforming</param>
        /// <returns></returns>
        private Message TransformMessageXmlToObject(XmlNode messageXML)
        {
            Message result = new Message();


            // TODO(c-jm): Add error handling here.
            result.name = messageXML.Attributes["name"].InnerText;

            {
                /* We map over all the child nodes of the message. (Which we know are wsdl:parts) and transform them into wsdl:part Objects */
                result.parts = messageXML.ChildNodes.Cast<XmlNode>().Select(x => TransformPartXmlToObject(x)).ToList();

            }
            
            return result;
        }

        private Type TransformTypeXmlToObject(XmlNode type)
        {
            Type result = new Type();

            result.Name = type.Attributes["name"].InnerText;
            result.Signature = type.Attributes["type"].InnerText;
            
            return result;
        }

        private TypeContainer TransformComplexTypeXmlToObject(XmlNode sElement)
        {
            TypeContainer result = new TypeContainer();

            result.Key = sElement.Attributes["name"].InnerText;

            XmlNode sequence = sElement.ChildNodes.Cast<XmlNode>().First(e => IsComplexType(e))
                                   .ChildNodes.Cast<XmlNode>().First(e => IsSSequence(e));

            result.Items = sequence.ChildNodes.Cast<XmlNode>().Select(e => TransformTypeXmlToObject(e)).ToList();

            return result;
        }


        private bool IsSSequence(XmlNode n)
        {
            const string S_ELEMENT_IDENTIFIER = "s:sequence";
            return n.Name == S_ELEMENT_IDENTIFIER;
        }
        private bool IsSElement(XmlNode n)
        {
            const string S_ELEMENT_IDENTIFIER = "s:element";
            return n.Name == S_ELEMENT_IDENTIFIER;
        }

        private bool IsSSchema(XmlNode n)
        {
            const string S_SCHEMA_IDENTIFIER = "s:schema";
            return n.Name == S_SCHEMA_IDENTIFIER;
        }
        private bool IsComplexType(XmlNode n)
        {
            const string COMPLEX_TYPE_IDENTIFIER = "s:complexType";
            return n.Name == COMPLEX_TYPE_IDENTIFIER;
        }


        private List<TypeContainer> TransformTypes()
        {
            XmlNode schemaNode = _typeNodes.Cast<XmlNode>().First(e => IsSSchema(e));

            IEnumerable<XmlNode> sElements = schemaNode.ChildNodes
                                                .Cast<XmlNode>().Where(e => IsSElement(e))  ;

            List<TypeContainer> typeContainers = sElements.Select(e => TransformComplexTypeXmlToObject(e)).ToList();

            return typeContainers;
        }
        
        /// <summary>
        /// Transform message structure 
        /// </summary>
        /// <returns></returns>
        public List<MessageContainer> TransformMessages()
        {
            /*
             * First we filter the list down to just soap nodes. This goes through and checks each wsdl:message node to see if it is a soap node.
             * This is important because web services offer a variety of different ways of interacting with their services.
             * As we are purely a SOAP/WSDL based Web Service consumption tool, we only need to concern ourselves with the SOAP value.
             */
            IEnumerable<XmlNode> messageNodeCollection = _messageNodes.Cast<XmlNode>().Where(xmlNode => IsSoapNode(xmlNode));

            /*
             * Once we get a collection of MessageXML nodes we then have to generate a datastructure that represents them
             * We map over the values applying hte TransformMessageXmlToObject function to each member of the collection.
             * This will give us back a collection of Messages that is traversable. 
             */
            IEnumerable<Message> messageCollection = messageNodeCollection.Select(messageXml => TransformMessageXmlToObject(messageXml));

            /*
             * We then have to group the messages together. This is due to the WSDL/SOAP format splitting up the typing based on a In parameter and a response.
             * It would be easier in our datastructure if the messages were grouped by their key or name.
             * As such we do a groupBy that goes through  TrimWSDLSoapMessage, which will remove the SoapIn Or SoapOut suffix. 
             * The reason it works for grouping our elements together is that if we have a Trimmed message that is the same it knows
             * that these are in the same category as it returns truthy. Which it groups by, we then cast our resultant groups into MessageContainers which have 
             * a list of messages as well as a key themselves.
             */
            List<MessageContainer> messageContainers = messageCollection.GroupBy(message => TrimWSDLSoapMessage(message.name)).Select(g => new  MessageContainer(g.Key, g.ToList())).ToList();

            TransformTypes();


            return  messageContainers;
        }
    }
}