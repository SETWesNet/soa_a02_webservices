using WebServiceInterface.Library.WSDL;

namespace WebServiceInterface.Library
{
    class Method
    {
        public string Name { get; set; }
        public Parameter[] Parameters { get; set; }

        public WSDLType Response { get; set; }
    }
}
