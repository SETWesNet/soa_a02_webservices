using WebServiceInterface.Library.WSDL;

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
