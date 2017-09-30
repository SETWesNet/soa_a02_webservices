using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebServiceInterface.Library;
using System.Text.RegularExpressions;
using System.Web.Services.Protocols;

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
        /// Invokes a method on the active SOAP web serice, returning all the data
        /// contained within the "Result" tag of the SOAP response.
        /// </summary>
        /// <param name="methodName">The name of the method to call on the web service.</param>
        /// <param name="arguments">The arguments to hand into the web method.</param>
        /// <returns>Contents of the SOAP method response tag..</returns>
        /// <exception cref="SoapException">Thrown when a server side SOAP fault occurs.</exception>
        /// <exception cref="XmlException">Thrown if SOAP response is not in expected format (missing result tag).</exception>
        /// <exception cref="HttpRequestException">Thrown when an issue occurs communicating with the web service or the server can't be resolved.</exception>
        public async Task<string> CallMethodAsync(Method method, params SOAPArgument[] arguments)
        {
            /* Create a soap envelope containing the method to call and parameter arguments */
            string soapEnvelope = CreateSoapEnvelope(method, arguments);
            string resultContents = "";

            /* Send envelope to web service and process the response, getting back the contents of the result tag*/
            using (StringContent content = new StringContent(soapEnvelope, Encoding.UTF8, "application/soap+xml"))
            {
                using (HttpResponseMessage response = await _httpClient.PostAsync(_serviceURL, content))
                {
                    resultContents = await ProcessSoapResponse(response, method);
                }
            }
            return resultContents;
        }

        /// <summary>
        /// Processes a SOAP response by checking to see if a valid result
        /// was returned. If a valid result was received, then the contents
        /// of the result tag will be returned. If a soap fault occurrs, a SOAP
        /// exception will be thrown.
        /// </summary>
        /// <param name="response">Response object from a SOAP request.</param>
        /// <param name="method">The method responsible for the response.</param>
        /// <returns>Contents of the Result tag of the SOAP response.</returns>
        /// <exception cref="SoapException">Thrown when a server side SOAP fault occurs.</exception>
        /// <exception cref="HttpRequestException">Thrown when an issue occurs communicating with the web service.</exception>
        /// <exception cref="XmlException">Thrown if SOAP response is not in expected format (missing result tag).</exception>
        private async Task<string> ProcessSoapResponse(HttpResponseMessage response, Method method)
        {
            XmlDocument responseXml = new XmlDocument();
       
            /* Throw exception if an error occurred communicating with the web service */
            if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.InternalServerError)
            {
                throw new HttpRequestException("An issue occurred communicating with the web service. Error: "
                    + response.StatusCode.ToString());
            }

            /* If response successful, get the SOAP response, otherwise, throw SOAP exception */
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.InternalServerError)
            {
                responseXml.LoadXml(await response.Content.ReadAsStringAsync());

                if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    throw new SoapException(responseXml.InnerText, XmlQualifiedName.Empty);
                }
            }

            /* Parse out the contents of the result tag in the SOAP response */
            string resultContents = ParseSoapResultContents(method, responseXml);

            return resultContents;
        }


        private static string CreateSoapEnvelope(Method method, SOAPArgument[] arguments)
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

        /// <summary>
        /// Parses a soap response and returns the contents of the (methodname)Result
        /// node. If the data within the node is just a value without tags, a Result tag
        /// will be added around the value.
        /// </summary>
        /// <param name="method">The method call that invoked the SOAP response.</param>
        /// <param name="soapResponse">The SOAP response that was returned based on a method call.</param>
        /// <returns></returns>
        /// <exception cref="XmlException">SOAP response XML not in expected format. Missing Result tag.</exception>
        private string ParseSoapResultContents(Method method, XmlDocument soapResponse)
        {
            /* Get result node, containing the primary SOAP payload */
            XmlNodeList resultNode = soapResponse.GetElementsByTagName(method.Name + "Result");

            if (resultNode.Count != 1)
            {
                throw new XmlException("SOAP response XML not in expected format. Missing Result tag named: "
                    + method.Name + "Result");
            }

            string response = resultNode[0].InnerText;

            /* If the response innertext doesn't have a tag, give it one */
            if (!Regex.IsMatch(response, "<?\\/*.>"))
            {
                response = response.Insert(0, "<Result>");
                response = response.Insert(response.Length, "</Result>");
            }
            return response;
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
                normalizedUrl += DOT_NET_WSDL_SUFFIX;
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
