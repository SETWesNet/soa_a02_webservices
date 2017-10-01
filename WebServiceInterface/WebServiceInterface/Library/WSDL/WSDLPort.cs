/* 
 *  
 *  Filename: WSDLPort.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLPort class
 * 
 */

namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    /// A WSDLPort is a value object which corresponds directly with the wsdl:port XmlNode
    /// </summary>
    class WSDLPort
    {
        public string Name { get; set; }
        public string Binding { get; set; }
        public string Location { get; set; }
    }
 }
