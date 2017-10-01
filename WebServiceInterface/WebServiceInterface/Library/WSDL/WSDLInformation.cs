using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServiceInterface.Library.WSDL
{
    class WSDLInformation
    {
        public string Namespace { get; set; }
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

        public WSDLOperation FindOperationByMessage(string message)
        {
            WSDLOperation result = null;

            result = message.Contains("Response") ? Operations.Cast<WSDLOperation>().First(o => o.OutputMessage.Replace("tns:", "") == message) : 
                                                    Operations.Cast<WSDLOperation>().First(o => o.InputMessage.Replace("tns:", "") == message);

            return result;
        }

        public string GetParameterTypeByName(string param)
        {
            string result = "";

            if (param.Contains("SoapOut") || param.Contains("Response"))
            {
                result = "OUT";
            }
            else
            {
                result = "IN";
            }

            return result;
        }

        /// <summary>
        /// Finds an operation by its parameter name
        /// </summary>
        /// <param name="input">The name of the </param>
        /// <returns></returns>
        public WSDLOperation FindOperationByParameter(string parameter)
        {

            WSDLOperation result = null;

            if (parameter.Contains("SoapIn"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Input.Replace("tns:", "") == parameter);
            }
            else if (parameter.Contains("SoapOut"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => o.Output.Replace("tns:", "") == parameter);
            }

            return result;
        }

        /// <summary>
        ///  Goes through WSDL operations and applies stores the type information in relation to that operation.
        /// <param name="typeInformation"> The list of type information to iterate through</param>
        /// <returns></returns>
        public void ApplyTypeInformation( List<WSDLTypeInformation> typeInformation)
        {
            foreach(WSDLTypeInformation current in typeInformation)
            {
                WSDLOperation currentInfo = null;

                currentInfo = FindOperationByMessage(current.Name);

                string type = GetParameterTypeByName(current.Name);

                if (type == "IN")
                {
                    currentInfo.InputTypeInformation = current;
                }
                else if (type == "OUT")
                {
                    currentInfo.OutputTypeInformation = current;

                }
            }
        }
    }
}
