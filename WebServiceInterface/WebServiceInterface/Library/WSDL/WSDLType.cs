﻿/* 
 *  
 *  Filename: WSDLType.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLType class
 * 
 */

namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    /// A WSDLType object represents the s:element in a XMLNode s:complexType. 
    /// </summary>
    class WSDLType
    {
        public string Name { get; set; }

        public string Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WSDLType"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        public WSDLType(string name, string type)
        {
            this.Name = name;
            this.Type = type;
        }
    }
}
