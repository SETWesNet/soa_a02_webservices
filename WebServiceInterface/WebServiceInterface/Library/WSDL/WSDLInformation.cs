/*
 * 
 *  Filename: WSDLInformation.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLInformation class
 * 
 */

using System.Linq;
using System.Collections.Generic;

namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    ///  The WSDLInformation class represents a WSDL as an object. It is returned from a WSDLParser which parses it from the XmlDocumentl.
    /// </summary>
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

        /// <summary>
        /// Finds a WSDLInformation's operation by name
        /// </summary>
        /// <param name="name"> Operation </param>
        /// <returns></returns>
        public WSDLOperation FindOperationByName(string name)
        {
            return Operations.Cast<WSDLOperation>().First(o => o.Name == name) ;
        }

        /// <summary>
        /// Finds a WSDLInformation's operation by message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public WSDLOperation FindOperationByMessage(string message)
        {
            WSDLOperation result = null;

            result = message.Contains("Response") ? Operations.Cast<WSDLOperation>().FirstOrDefault(o => WSDLHelpers.TrimNamespace(o.OutputMessage) == message) : 
                                                    Operations.Cast<WSDLOperation>().FirstOrDefault(o => WSDLHelpers.TrimNamespace(o.InputMessage) == message);

            return result;
        }

        /// <summary>
        /// Gets the name of the parameter type by checking to see if its an input or output parameter.
        /// </summary>
        /// <param name="param">The parameter.</param>
        /// <returns></returns>
        public string GetParameterTypeByName(string param)
        {
            string result = "";

            if (param.Contains("SoapOut") || param.Contains("Response"))
            {
                result = WSDLHelpers.OUT_PARAMETER;
            }
            else
            {
                result = WSDLHelpers.IN_PARAMETER;
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
                result = Operations.Cast<WSDLOperation>().First(o => WSDLHelpers.TrimNamespace(o.Input) == parameter);
            }
            else if (parameter.Contains("SoapOut"))
            {
                result = Operations.Cast<WSDLOperation>().First(o => WSDLHelpers.TrimNamespace(o.Output) == parameter);
            }

            return result;
        }

        /// <summary>
        ///  Goes through WSDL operations and applies stores the type information in relation to that operation.
        /// <param name="typeInformation"> The list of type information to iterate through</param>
        /// <returns></returns>
        public void ApplyTypeInformation( List<WSDLTypeInformation> typeInformation)
        {
            // For every type we have find the operation and fill out the input/output parameters
            foreach(WSDLTypeInformation current in typeInformation)
            {
                WSDLOperation currentInfo = null;

                // Find the operation, if we dont have one continue.
                currentInfo = FindOperationByMessage(current.Name);
                if (currentInfo == null)
                {
                    continue;
                }

                string type = GetParameterTypeByName(current.Name);
                if (type == WSDLHelpers.IN_PARAMETER)
                {
                    currentInfo.InputTypeInformation = current;
                }
                else if (type == WSDLHelpers.OUT_PARAMETER)
                {
                    currentInfo.OutputTypeInformation = current;

                }
            }
        }
    }
}
