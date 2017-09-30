using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class WSDLInformation
    {
        public string BaseName { get; set; }

        public WSDLService Service { get; set; }
        public WSDLPort Port { get; set; }
        public List<WSDLOperation> Operations { get; set; }

        public WSDLInformation()
        {
            Service = new WSDLService();
            Port = new WSDLPort();
            Operations = new List<WSDLOperation>();
        }

        public WSDLOperation FindOperationByName(string name)
        {
            return Operations.Cast<WSDLOperation>().First(o => o.Name == name) ;
        }

        public string GetParameterTypeByName(string param)
        {
            string result = "";

            if (param.Contains("SoapIn"))
            {
                result = "IN";
            }
            else if (param.Contains("SoapOut"))
            {
                result = "OUT";
            }

            return result;
        }

        public WSDLOperation FindOperationByParameter(string input)
        {

            WSDLOperation result = null;

            if (input.Contains("SoapIn"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Input.Replace("tns:", "") == input);
            }
            else if (input.Contains("SoapOut"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Output.Replace("tns:", "") == input);
            }

            // TODO(c-jm): Replace this with a helper method in a static class
            return result;
        }
               
    }

}
