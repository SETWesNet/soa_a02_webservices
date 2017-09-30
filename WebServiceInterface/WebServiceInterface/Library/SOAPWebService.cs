using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;
using System.Web.Services.Protocols;
using WebServiceInterface.Library;
using System.IO;
using System.Net.Http;

namespace WebServiceInterface
{
    class SOAPWebService
    {
        private string _serviceURL;
        const string DOT_NET_WSDL_SUFFIX = "?WSDL";
        private static HttpClient _httpClient = new HttpClient();


        /// <summary>
        /// Initializes a new instance of the <see cref="SOAPWebService"/> class, which
        /// is able to invoke multiple methods at a given SOAP web service URL.
        /// </summary>
        /// <param name="serviceURL">The URL of a web service.</param>
        public SOAPWebService(string serviceURL)
        {
            _serviceURL = serviceURL;
        }

        /// <summary>
        /// Gets or sets the URL of the active SOAP web service.
        /// </summary>
        /// <value>
        /// The URL of the active SOAP web service.
        /// </value>
        public string ServiceURL
        {
            get
            {
                return _serviceURL;
            }
            set
            {
                _serviceURL = value;
            }
        }

        /// <summary>
        /// Invokes a method on the active SOAP web serice, returning its response.
        /// </summary>
        /// <param name="methodName">The name of the method to call on the web service.</param>
        /// <param name="arguments">The arguments to hand into the web method.</param>
        /// <returns>XML response from the soap web method.</returns>
        public async Task<string> CallMethodAsync(Method method, params SOAPArgument[] arguments)
        {
            string soapEnvelope = CreateSoapEnvelope(method.Name, arguments);
            XmlDocument responseXml = new XmlDocument();

            using (StringContent content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml"))
            {
                using (HttpResponseMessage response = await _httpClient.PostAsync(_serviceURL, content))
                {
                   responseXml.LoadXml(await response.Content.ReadAsStringAsync());
                }
            }
            return responseXml.InnerText;
        }


        private static string CreateSoapEnvelope(string methodName, SOAPArgument[] arguments)
        {
            string envelopeString = "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                                    "xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" +
                                    "<soap12:Body>";

            envelopeString += "<GetAirportInformationByCountry xmlns=\"http://www.webserviceX.NET\">" +
                              "<country>Canada</country>" +
                              "</GetAirportInformationByCountry>" +
                              "</soap12:Body>" +
                              "</soap12:Envelope>";

            return envelopeString;
        }

        #region WSDL

        /// <summary>
        /// A method to "normalize" WSDL urls. If you pass it a page with .asmx it will append a "?WSDL" to the end. If another service is chosen it uses the .wsdl convention.
        /// </summary>
        /// <param name="serviceUrl">The service url we are normalizing</param>
        /// <returns></returns>
        private string NormalizeWSDLUrl(string serviceUrl)
        {
            string[] parts = serviceUrl.Split('/');

            // TODO(c-jm): This may need some work, do testing with "www.abc.com/service.wsdl" services
            string normalizedUrl = serviceUrl;

            if (parts.Last().EndsWith(".asmx"))
            {
                normalizedUrl += "?WSDL";
            }

            return normalizedUrl;
        }

        /// <summary>
        /// Create a WebRequest which represents a get request to get a WSDL
        /// </summary>
        /// <param name="serviceUrl"> The service that we are trying to recieve the WSDL for. </param>
        /// <returns></returns>
        private HttpWebRequest CreateWSDLWebRequest(string serviceUrl)
        {
            string normalizedUrl = NormalizeWSDLUrl(serviceUrl);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(normalizedUrl);
            request.Method = "GET";

            return request;
        }

        /// <summary>
        /// Specify a URL and get a WSDL for that service.
        /// </summary>
        /// <param name="serviceUrl"> The URL that we are specifying, in this case the URL will be normalized for WSDL services. (?WSDL will be appended for .NET etc) </param>
        /// <returns></returns>
        public async Task<XmlDocument> GetWSDLAsync(string serviceUrl)
        {
            HttpWebRequest request = CreateWSDLWebRequest(serviceUrl);
            WebResponse response = await request.GetResponseAsync();
            XmlDocument wsdl = new XmlDocument();

            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                string wsdlString = await sr.ReadToEndAsync();
                wsdl.LoadXml(wsdlString);
            }

            return wsdl;
        }

        #endregion
    }
}
