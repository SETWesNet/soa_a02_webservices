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
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the binding.
        /// </summary>
        /// <value>
        /// The binding.
        /// </value>
        public string Binding { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }
    }
 }
