using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class WSDLOperation
    {
        public string Name { get; set; }
        public string SoapAction { get; set; }

        public string Input { get; set; }
        public string InputMessage { get; set; }
        public string Output { get; set; }
        public string OutputMessage { get; set; }
        public string Documentation { get; set; }

        public WSDLOperation(string name, string soapAction)
        {
            this.Name = name;
            this.SoapAction = soapAction;
            this.Input = "";
            this.Output = "";
        }
    }
}
