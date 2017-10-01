/* 
 *  
 *  Filename: WSDLService.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLService class
 * 
 */

namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    /// The WSDLService class is a value object which corresponds directly with the wsdl:service XMLNode
    /// </summary>
    class WSDLService
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
