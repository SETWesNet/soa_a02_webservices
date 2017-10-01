/* 
 *  
 *  Filename: WSDLOperation.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the WSDLOperation class
 * 
 */


namespace WebServiceInterface.Library.WSDL
{
    /// <summary>
    /// A WSDL operation is an object which corresponds directly with the wsdl:operation xml node for parsing.
    /// </summary>
    class WSDLOperation
    {
        public string Name { get; set; }
        public string SoapAction { get; set; }
        public string Input { get; set; }
        public string InputMessage { get; set; }
        public WSDLTypeInformation InputTypeInformation { get; set; }
        public string Output { get; set; }
        public string OutputMessage { get; set; }
        public WSDLTypeInformation OutputTypeInformation { get; set; }
        public string Documentation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WSDLOperation"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="soapAction">The SOAP action.</param>
        public WSDLOperation(string name, string soapAction)
        {
            this.Name = name;
            this.SoapAction = soapAction;
            this.InputTypeInformation = new WSDLTypeInformation();
            this.OutputTypeInformation = new WSDLTypeInformation();
        }
    }
}
