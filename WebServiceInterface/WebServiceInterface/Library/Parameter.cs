/* 
 *  
 *  Filename: Parameter.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the Parameter class
 * 
 */

namespace WebServiceInterface.Library
{
    /// <summary>
    /// A parameter object represents a Parameter in our config library. Their type information is applied dynamically from the WSDL at runtime.
    /// </summary>
    class Parameter
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the regex.
        /// </summary>
        /// <value>
        /// The regex.
        /// </value>
        public string Regex { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }
        
    }
}
