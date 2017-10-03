/* 
 *  
 *  Filename: Method.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the Method class
 * 
 */

using WebServiceInterface.Library.WSDL;

namespace WebServiceInterface.Library
{
    /// <summary>
    /// A Method represents a Web Service method within our config library/file. 
    /// </summary>
    class Method
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>
        /// The parameters.
        /// </value>
        public Parameter[] Parameters { get; set; }


        /// <summary>
        /// Gets or sets the response.
        /// </summary>
        /// <value>
        /// The response.
        /// </value>
        public WSDLType Response { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>
        /// The namespace.
        /// </value>
        public string Namespace { get; set; }
       
        public string Documentation { get; set; }
    }
}
