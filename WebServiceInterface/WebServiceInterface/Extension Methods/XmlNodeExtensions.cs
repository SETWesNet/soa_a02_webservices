/* 
 *  
 *  Filename: Parameter.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the XMLNodeExtensions class
 * 
 */

using System.Xml;
using WebServiceInterface.Exceptions;

namespace WebServiceInterface.Extension_Methods
{
    /// <summary>
    /// An extension methods class for the XmlNode object.
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Gets a specific attribute off of an XMLNode's attributes. If it can't find it it throws a WSDLNodeNotFoundException.
        /// </summary>
        /// <param name="node">The node we are looking to get the attribute from.</param>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="wsdlNodeType"> Purely for error purposes. The name of the WSDL element where there could be an error </param>
        /// <returns> The InnerText of the attribute if it exists </returns>
        /// <exception cref="WSDLNodeNotFoundException"></exception>
        public static string GetTextAttribute(this XmlNode node, string attributeName, string wsdlNodeType)
        {
            string result = "";

            // We check to see if the node exists and it has an attribute of that name. If not then we throw the exception
            if (node == null || node.Attributes[attributeName] == null)
            {
                string message = string.Format("Node: {0} doesnt have attribute: {1}", wsdlNodeType, attributeName);
                throw new WSDLNodeNotFoundException(message);
            }
            else
            {
                result = node.Attributes[attributeName].InnerText;
            }

            // If everything is successful we return the innertext of the attribute
            return result;
        }
    }
}
