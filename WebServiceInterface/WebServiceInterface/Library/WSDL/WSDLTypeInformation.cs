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
    /// A WSDLType information represents parameters to a particular method.
    /// </summary>
    class WSDLTypeInformation
    {
        public string Name { get; set; }
        public List<WSDLType> Types { get; set; } 
    }
}
