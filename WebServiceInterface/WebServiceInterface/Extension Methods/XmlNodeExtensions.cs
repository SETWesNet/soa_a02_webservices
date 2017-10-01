using System.Xml;
using WebServiceInterface.Exceptions;

namespace WebServiceInterface.Extension_Methods
{
    public static class XmlNodeExtensions
    {
        public static string GetTextAttribute(this XmlNode node, string attributeName, string wsdlNodeType)
        {
            string result = "";

            if (node == null || node.Attributes[attributeName] == null)
            {
                string message = string.Format("Node: {0} doesnt have attribute: {1}", wsdlNodeType, attributeName);
                throw new WSDLNodeNotFoundException(message);
            }
            else
            {
                result = node.Attributes[attributeName].InnerText;
            }

            return result;
        }
    }
}
