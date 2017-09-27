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

namespace WebServiceInterface
{
    class SOAPWebService
    {
        private string _serviceURL;

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
        public async Task<string> CallMethodAsync(Method method, params object[] arguments)
        {
            /* Create a webrequest to the desired SOAP service method, and create a soap envelope (with parameters) */
            HttpWebRequest webRequest = CreateSoapWebRequest(_serviceURL);
            XmlDocument soapEnvelope = CreateSoapEnvelope(method.Name, arguments);

            /* Save the soap envelope to the HTTP request */
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelope.Save(stream);
            }

            string soapResponse = "";
          
            /* Send the request and receive the response back from the web service */
            using (WebResponse webResponse = await webRequest.GetResponseAsync())
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResponse = await rd.ReadToEndAsync();
                }
            }
     
            return soapResponse;
        }
        
        /// <summary>
        /// Creates an HTTPWebRequest object with the HTTP header information configured
        /// to carry a SOAP payload using POST to the desried web service URL.
        /// </summary>
        /// <param name="serviceURL">The URL to deliver the POST request to.</param>
        /// <returns>HTTPWebRequest object configured for SOAP.</returns>
        private static HttpWebRequest CreateSoapWebRequest(string serviceURL)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
            webRequest.ContentType = "application/soap+xml";
            webRequest.Method = "POST";
            return webRequest;
        }

        private static XmlDocument CreateSoapEnvelope(string methodName, object[] arguments)
        {
            XmlDocument soapEnvelope = new XmlDocument();
            string envelopeString = "<soap12:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" " +
                                    "xmlns:soap12=\"http://www.w3.org/2003/05/soap-envelope\">" +
                                    "<soap12:Body>";
           
            envelopeString += "<GetAirportInformationByCountry xmlns=\"http://www.webserviceX.NET\">" + 
                              "<country>Canada</country>" +
                              "</GetAirportInformationByCountry>" +
                              "</soap12:Body>" +
                              "</soap12:Envelope>";

            soapEnvelope.LoadXml(envelopeString);
   
            return soapEnvelope;
        }
    }
}
