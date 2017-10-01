/* 
 *  
 *  Filename: WebService.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WebService class
 * 
 */
namespace WebServiceInterface.Library
{
    /// <summary>
    /// Represents a web service within our configuration library/file
    /// </summary>
    class WebService
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the methods.
        /// </summary>
        /// <value>
        /// The methods.
        /// </value>
        public Method[] Methods { get; set; }
    }
}
