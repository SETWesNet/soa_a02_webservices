/* 
 *  
 *  Filename: SoapArgument.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the SoapArgument class
 * 
 */

namespace WebServiceInterface.Library
{
    /// <summary>
    /// Represents an argument to a web service's SOAP method
    /// </summary>
    class SOAPArgument
    {
        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public Parameter Parameter { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
    }
}
