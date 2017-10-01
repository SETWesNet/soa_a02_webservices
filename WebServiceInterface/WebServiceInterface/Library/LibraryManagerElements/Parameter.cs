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
    class Parameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string Regex { get; set; }
        public bool Required { get; set; }
        public string ErrorMessage { get; set; }
    }
}
