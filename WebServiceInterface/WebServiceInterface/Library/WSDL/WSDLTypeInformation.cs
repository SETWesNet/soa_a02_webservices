/* 
 *  
 *  Filename: WSDLTypeInformation.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLTypeInformation class
 * 
 */

using System.Collections.Generic;

namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    /// A WSDLType information represents a method parameters.
    /// </summary>
    class WSDLTypeInformation
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public List<WSDLType> Types { get; set; } 
    }
}
