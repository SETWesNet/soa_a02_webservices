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
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the SOAP action.
        /// </summary>
        /// <value>
        /// The SOAP action.
        /// </value>
        public string SoapAction { get; set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the input message.
        /// </summary>
        /// <value>
        /// The input message.
        /// </value>
        public string InputMessage { get; set; }

        /// <summary>
        /// Gets or sets the input type information.
        /// </summary>
        /// <value>
        /// The input type information.
        /// </value>
        public WSDLTypeInformation InputTypeInformation { get; set; }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        /// <value>
        /// The output.
        /// </value>
        public string Output { get; set; }

        /// <summary>
        /// Gets or sets the output message.
        /// </summary>
        /// <value>
        /// The output message.
        /// </value>
        public string OutputMessage { get; set; }

        /// <summary>
        /// Gets or sets the output type information.
        /// </summary>
        /// <value>
        /// The output type information.
        /// </value>
        public WSDLTypeInformation OutputTypeInformation { get; set; }

        /// <summary>
        /// Gets or sets the documentation.
        /// </summary>
        /// <value>
        /// The documentation.
        /// </value>
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
