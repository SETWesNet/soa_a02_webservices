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
using System.Threading.Tasks;
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
            return _services.First(service => service.Url == serviceURL);
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

        public Parameter GetParameter(string serviceURL, string methodName, string parameterName)
        {
            return _services.First(service => service.Url == serviceURL)
                .Methods.First(method => method.Name == methodName)
                .Parameters.First(parameter => parameter.Name == parameterName);
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
            foreach(WebService currentService in _services)
            {
                // We find the service where we have that service url.
                if (CheckIfSameUrl(currentService.Url, serviceUrl))
                {
                    // Then we get the index of the method we are looking for and set it.
                    for(int j = 0; j < currentService.Methods.Count(); ++j)
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
        /// Loads the WSDLs asynchronous and creates information for each one..
        /// </summary>
        /// <returns></returns>
        public async Task LoadWSDLsAsync()
        {
            /* Apply WSDL information to each service */
            foreach (WebService service in _services)
            {
                // Genereate xml document
                XmlDocument wsdlXml = await SOAPWebService.GetWSDLAsync(service.Url);

                // Apply WSDL information to our library of configured methods
                WSDLParser wsdlParser = new WSDLParser(wsdlXml);
                WSDLInformation wsdlInfo = wsdlParser.BuildWSDLInformation();
                MergeWithWSDL(wsdlInfo);
            }
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

                foreach (Parameter parameter in parameters)
                {
                    WSDLType parameterTypeInfo = operation.InputTypeInformation.Types.Cast<WSDLType>().FirstOrDefault(type => type.Name == parameter.Name);

                    if (parameterTypeInfo != null)
                    {
                        parameter.Type = parameterTypeInfo.Type;
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
