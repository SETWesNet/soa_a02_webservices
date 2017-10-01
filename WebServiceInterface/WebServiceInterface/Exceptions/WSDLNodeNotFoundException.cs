using System;
using System.Xml;

namespace WebServiceInterface.Exceptions
{
    class WSDLNodeNotFoundException : Exception
    {
        public WSDLNodeNotFoundException(string message) : base(message)
        {
        }
    }
}
