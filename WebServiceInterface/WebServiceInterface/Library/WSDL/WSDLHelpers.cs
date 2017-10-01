/* 
 *  
 *  Filename: WSDLHelpers.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLHelpers class
 * 
 */

namespace WebServiceInterface.Library
{
    /// <summary>
    /// Holds a library of utility methods and constants for the WSDL
    /// </summary>
    public static class WSDLHelpers
    {
        public const string SOAP_SUFFIX = "Soap";
        public const string SOAP_12_SUFFIX = "Soap12";
        public const string TNS_NAMESPACE = "tns:";

        public const string IN_PARAMETER = "IN";
        public const string OUT_PARAMETER = "OUT";

        /// <summary>
        /// Trims a  namespace from an xml node element/name.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string TrimNamespace(string s)
        {
            return s.Replace(WSDLHelpers.TNS_NAMESPACE, "");
        }
    }
}
