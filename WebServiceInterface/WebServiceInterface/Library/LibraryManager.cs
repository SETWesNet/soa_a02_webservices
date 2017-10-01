/* 
 *  
 *  Filename: WSDLTypeInformation.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the LibraryManager class 
 * 
 */

using System.IO;
using Newtonsoft.Json;
using System.Linq;
using WebServiceInterface.Library.WSDL;
using System.Xml;
using System;
using System.Collections.Generic;
using WebServiceInterface.Exceptions;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace WebServiceInterface.Library
{
    class LibraryManager
    {
        /// <summary>
        /// The raw content
        /// </summary>
        private string _rawContent;

        /// <summary>
        /// The services
        /// </summary>
        private WebService[] _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryManager"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public LibraryManager(string fileName)
        {
            _rawContent = File.ReadAllText(fileName);
            _services = JsonConvert.DeserializeObject<WebService[]>(_rawContent);
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        /// <value>
        /// The services.
        /// </value>
        public WebService[] Services
        {
            get
            {
                return _services;
            }
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <param name="serviceURL">The service URL.</param>
        /// <returns></returns>
        public WebService GetService(string serviceURL)
        {
            return _services.FirstOrDefault(service => service.Url == serviceURL);
        }

        /// <summary>
        /// Gets the specified method at that service url and methodname.
        /// </summary>
        /// <param name="serviceURL">The service URL.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public Method GetMethod(string serviceURL, string methodName)
        {
            /* Find the webservice from the config that matches the WSDL */
            WebService service = _services.FirstOrDefault(s => CheckIfSameUrl(s.Url, serviceURL));

            Method result = null;

            // If we have a service  then find the method
            if (service != null)
            {
                result = service.Methods.FirstOrDefault(m => m.Name == methodName);
            }

            return result;
        }

        public Parameter GetParameter(string serviceURL, string methodName, string parameterName)
        {
            return _services.First(service => service.Url == serviceURL)
                .Methods.First(method => method.Name == methodName)
                .Parameters.First(parameter => parameter.Name == parameterName);
        }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        /// <param name="serviceURL">The service URL.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns></returns>
        public Parameter[] GetParameters(string serviceURL, string methodName)
        {
            Method method = GetMethod(serviceURL, methodName);
            return method.Parameters;
        }


        /// <summary>
        /// Sets the method by index. 
        /// </summary>
        /// <param name="newMethod">The new method.</param>
        /// <param name="serviceUrl">The service URL.</param>
        /// <param name="methodName">Name of the method.</param>
        public void SetMethod(Method newMethod, string serviceUrl, string methodName)
        {
            //NOTE(c-jm): Have to use the for loop here because we are keeping track of which method to set.
            int methodIndex = 0;
            WebService service = null;

            // Through each service
            foreach (WebService currentService in _services)
            {
                // We find the service where we have that service url.
                if (CheckIfSameUrl(currentService.Url, serviceUrl))
                {
                    // Then we get the index of the method we are looking for and set it.
                    for (int j = 0; j < currentService.Methods.Count(); ++j)
                    {
                        if (currentService.Methods[j].Name == methodName)
                        {
                            methodIndex = j;
                            break;
                        }
                    }

                    service = currentService;
                    break;
                }
            }
            service.Methods.SetValue(newMethod, methodIndex);
        }

        /// <summary>
        /// Loads the WSDL file for each webservice entered in the library
        /// config file. When a WSDL is loaded, it adds its extended information to
        /// the method and parameter objects. (namespace for method, parameter types
        /// for parameter). This needs to be called in order to have sufficient information
        /// to fufill a soap request (using <see cref="SOAPWebService"/>.
        /// </summary>
        /// <returns>True if all WSDL's loaded successfully, false otherwise</returns>
        public async Task<bool> LoadWSDLsAsync()
        {
            bool allWsdlsLoaded = true;

            /* Apply WSDL information to each service */
            foreach (WebService service in _services)
            {
                bool loadSuccess = await LoadWSDLAsync(service);

                /* If a wsdl failed to load, set allWsdlsLoaded to false */
                if (!loadSuccess && allWsdlsLoaded)
                {
                    allWsdlsLoaded = false;
                }
            }
            return allWsdlsLoaded;
        }

        /// <summary>
        /// Loads the a WSDL for the selected web service, and modifies the
        /// webservices method and parameter objects to add in additional information
        /// about the service (parameter types and method namespace). If a WSDL fails to
        /// load, the service is removed from the library.
        /// </summary>
        /// <param name="service">The service to load the WSDL for.</param>
        /// <returns>Bool if load successful, false otherwise.</returns>
        private async Task<bool> LoadWSDLAsync(WebService service)
        {
            bool loadSuccess = false;

            try
            {
                // Genereate xml document
                XmlDocument wsdlXml = await SOAPWebService.GetWSDLAsync(service.Url);

                // Apply WSDL information to our library of configured methods
                WSDLParser wsdlParser = new WSDLParser(wsdlXml);
                WSDLInformation wsdlInfo = wsdlParser.BuildWSDLInformation();
                MergeWithWSDL(wsdlInfo);

                loadSuccess = true;
            }
            catch (Exception ex) when (ex is WSDLNodeNotFoundException || ex is HttpRequestException || ex is XmlException)
            {
                Logger.Log(ex);
                List<WebService> serviceList = _services.ToList();
                serviceList.Remove(service);
                _services = serviceList.ToArray();
            }
            return loadSuccess;
        }

        /// <summary>
        /// Merges the  WSDL with the configuration library already present.
        /// </summary>
        /// <param name="info">The information.</param>
        private void MergeWithWSDL(WSDLInformation info)
        {
            foreach (WSDLOperation operation in info.Operations)
            {
                Method method = GetMethod(info.Port.Location, operation.Name);

                if (method == null)
                {
                    continue;
                }

                method.Response = operation.OutputTypeInformation.Types.Cast<WSDLType>().First();
                method.Namespace = info.Namespace;

                Parameter[] parameters = method.Parameters;

                /* Process parameters only if this method has them */
                if (parameters != null)
                {
                    foreach (Parameter parameter in parameters)
                    {
                        WSDLType parameterTypeInfo = operation.InputTypeInformation.Types.Cast<WSDLType>().FirstOrDefault(type => type.Name == parameter.Name);

                        if (parameterTypeInfo != null)
                        {
                            parameter.Type = parameterTypeInfo.Type;
                        }
                    }
                }

                SetMethod(method, info.Port.Location, operation.Name);
            }
        }

        /// <summary>
        /// Checks if two URL's are the same by comparing them without the
        /// http:// or https:// headers.
        /// </summary>
        /// <param name="firstUrl">The first URL.</param>
        /// <param name="secondUrl">The second URL.</param>
        /// <returns>True if same URL, false otherwise.</returns>
        private static bool CheckIfSameUrl(string firstUrl, string secondUrl)
        {
            const string httpPattern = "https?:\\/\\/";

            /* Remove the HTTP header from the URL's */
            string basefirstUrl = Regex.Replace(firstUrl, httpPattern, "");
            string baseSecondUrl = Regex.Replace(secondUrl, httpPattern, "");

            bool isSame = false;

            if (basefirstUrl == baseSecondUrl)
            {
                isSame = true;
            }
            return isSame;
        }
    }
}
