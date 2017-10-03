/* 
 *  
 *  Filename: MainForm.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the MainForm class
 * 
 */

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Resources;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Windows.Forms.Extensions;
using System.Xml;
using WebServiceInterface.Library;

namespace WebServiceInterface
{
    public partial class MainForm : Form
    {
        private LibraryManager _libraryManager = null;
        private ParameterManager _parameterManager = new ParameterManager(null);

        private ResourceManager _resourceManager = new ResourceManager(typeof(MainForm));
        private ToolTip _errorTooltip = new ToolTip();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #region Properties

        /// <summary>
        /// Gets the web service URL for the web service currently
        /// selected in the web services dropdown.
        /// </summary>
        private string SelectedWebServiceURL
        {
            get => (string)drpdwnWebServices.SelectedValue;
        }

        /// <summary>
        /// Gets the name of the method currently selected in the
        /// method dropdown.
        /// </summary>
        private string SelectedMethodName
        {
            get => (string)drpdwnMethods.SelectedValue;
        }

        #endregion

        /// <summary>
        /// Initializes the form by loading in WSDL information into the libraryManager,
        /// applying information to the web service and method dropdowns, all while showing
        /// a loading status.
        /// </summary>
        /// <returns>Task</returns>
        private async Task InitializeForm()
        {
            try
            {
                _libraryManager = new LibraryManager("config.json");
            }
            catch(Exception e) when (e is FileNotFoundException)
            {
                Logger.Log(e);
                MessageBox.Show(e.Message);
                Application.Exit();
            }

            /* Show loading status */
            txtbrdStatus.Text = _resourceManager.GetString("Load_ServiceInformation_Message");
            DisplayLoadStatus(true);

            /* Load WSDL information for each service in library */
            bool loadSuccess = await _libraryManager.LoadWSDLsAsync();

            if (!loadSuccess)
            {
                MessageBox.Show(this, _resourceManager.GetString("Error_wsdlLoad_Message"),
                    _resourceManager.GetString("Error_wsdlLoad_Caption"),
                     MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            /* Initialize Drop Downs */
            drpdwnWebServices.DisplayMember = nameof(WebService.Name);
            drpdwnWebServices.ValueMember = nameof(WebService.Url);
            drpdwnWebServices.DataSource = _libraryManager.Services;

            drpdwnMethods.DisplayMember = nameof(Method.Name);
            drpdwnMethods.ValueMember = nameof(Method.Name);

            /* Apply methods to method dropdown, only if services are loaded */
            WebService selectedService = _libraryManager.GetService(SelectedWebServiceURL);
            if (selectedService != null)
            {
                drpdwnMethods.DataSource = selectedService.Methods;
            }

            /* Hide loading status and enable send button */
            DisplayLoadStatus(false);
            btnSend.Enabled = true;
        }

        /// <summary>
        /// Deletes old parameter controls that are located in the flow layout and
        /// places new controls in their place, based on the parameters required for
        /// the currently selected web service method.
        /// </summary>
        private void CreateParameterControls()
        {
            /* Hide the tooltip if showing on any controls (bug in .NET, will
             * crash if the object it is displaying on is disposed beore fade out). */
            foreach (Control control in flwParameters.Controls)
            {
                _errorTooltip.Hide(control);
            }

            /* Clear all current parameter controls */
            flwParameters.Controls.DisposeAll();

            /* Create a control for each parameter required by the method and add to UI flow layout */
            Method selectedMethod = _libraryManager.GetMethod(SelectedWebServiceURL, SelectedMethodName);
            Control[] parameterControls = _parameterManager.CreateParameterControls();
            flwParameters.Controls.AddRange(parameterControls);

            /* Assign event to each textbox for validation */
            foreach (Control textBox in parameterControls)
            {
                if (textBox is TextBox)
                {
                    textBox.TextChanged += TextChanged_RuleChecker;
                }
            }
        }

        /// <summary>
        /// Validates the value of a parameter textbox against the rules for the parameter it
        /// represents. Upon failure, the last change is reverted and an error displayed.
        /// </summary>
        /// <param name="parameterTextBox">A textbox created by <see cref="ParameterManager.CreateParameterControls"/></param>
        private void ValidateParameterTextBox(TextBox parameterTextBox)
        {
            /* Use the name of the textbox to get the corresponding parameter it is taking input for */
            Parameter parameter = _libraryManager.GetParameter(SelectedWebServiceURL,
                SelectedMethodName, parameterTextBox.Name);

            /* Check to see if value in textbox is valid */
            bool isValid = _parameterManager.ValidateRegex(parameterTextBox);

            if (!isValid)
            {
                /* If invalid data was entered, remove last invalid character and keep cursor position */
                int originalSelectionStart = parameterTextBox.SelectionStart - 1;
                parameterTextBox.Text = parameterTextBox.Text.Remove(parameterTextBox.SelectionStart - 1, 1);
                parameterTextBox.SelectionStart = originalSelectionStart;

                SystemSounds.Exclamation.Play();
                _errorTooltip.Show(parameter.ErrorMessage, parameterTextBox,
                    parameterTextBox.Width, parameterTextBox.Height / 2, 2000);
            }
        }

        /// <summary>
        /// Calls the currently selected web service method using the
        /// arguments entered by the user, returning the SOAP response
        /// as XML.
        /// </summary>
        /// <returns>The SOAP response XML.</returns>
        /// <exception cref="SoapException">Thrown when a server side SOAP fault occurs.</exception>
        /// <exception cref="HttpRequestException">Thrown when an issue occurs communicating with the web service.</exception>
        private async Task<string> CallWebServiceMethod()
        {
            /* Get all the arguments entered by the user */
            Method selectedMethod = _libraryManager.GetMethod(SelectedWebServiceURL, SelectedMethodName);
            SOAPArgument[] arguments = _parameterManager.CollectParameterArguments(flwParameters.Controls.ToArray());

            string soapResponse = "";

            /* Call the web service method using user arguments (if any) */
            if (selectedMethod != null)
            {
                SOAPWebService webService = new SOAPWebService(SelectedWebServiceURL);
                soapResponse = await webService.CallMethodAsync(selectedMethod, arguments);
            }

            return soapResponse;
        }

        /// <summary>
        /// Displays the contents of a SOAP responses
        /// result node in the datagridview area.
        /// </summary>
        /// <param name="soapResponse">The contents of a soap result node.</param>
        private void DisplaySoapResponse(string soapResponse)
        {
            try
            {
                if (!string.IsNullOrEmpty(soapResponse))
                {
                    /* Put the reponse into a table, if not already */
                    if (!soapResponse.Contains("<Table>"))
                    {
                        soapResponse = soapResponse.Insert(0, "<Table>");
                        soapResponse = soapResponse.Insert(soapResponse.Length, "</Table>");
                    }

                    /* Convert XML table into DataTable using dataset, bind to grid for display */
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(new StringReader(soapResponse));
                    grdviewResponse.DataSource = dataSet.Tables[0];
                }
            }
            catch (Exception ex) when (ex is IndexOutOfRangeException || ex is XmlException)
            {
                Logger.Log(ex, soapResponse);
                MessageBox.Show(this, _resourceManager.GetString("Error_ResponseDisplay_Message"),
                _resourceManager.GetString("Error_ResponseDisplay_Caption"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// Processes a SOAP transaction from start to finish by calling the web
        /// service method and then displaying the resulting response. A loading
        /// status is shown to alert the user of the progress and the send button
        /// is disabled until finished.
        /// </summary>
        /// <returns>Task</returns>
        private async Task ProcessSOAPTransaction()
        {
            try
            {
                /* Ensure all required controls are filled out */
                bool validParams = _parameterManager.ValidateRequired(flwParameters.Controls.ToArray());

                if (validParams)
                {
                    /* Clear data grid of current data and disable send button */
                    grdviewResponse.DataSource = null;
                    btnSend.Enabled = false;

                    /* Show TextBoard with current status and call web method */
                    DisplayLoadStatus(true);
                    txtbrdStatus.Text = _resourceManager.GetString("Load_RetrieveResponse_Message");
                    string response = await CallWebServiceMethod();

                    /* Update status and begin loading results into grid */
                    txtbrdStatus.Text = _resourceManager.GetString("Load_LoadingResults_Message");
                    txtbrdStatus.Update();
                    DisplaySoapResponse(response);
                }
                else
                {
                    MessageBox.Show(this, _resourceManager.GetString("Notify_RequiredParam_Message"),
                        _resourceManager.GetString("Notify_RequiredParam_Caption"),
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (SoapException ex)
            {
                Logger.Log(ex);
                MessageBox.Show(this, _resourceManager.GetString("Error_SoapFault_Message"),
                _resourceManager.GetString("Error_SoapFault_Caption"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException ex)
            {
                Logger.Log(ex);
                MessageBox.Show(this, _resourceManager.GetString("Error_HttpRequest_Message"),
                _resourceManager.GetString("Error_HttpRequest_Caption"),
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                /* Re-enable send button and hide the status box */
                btnSend.Enabled = true;
                DisplayLoadStatus(false);
            }
        }

        /// <summary>
        /// Shows or hides a textboard and wait cursor over the grid view, used
        /// to show the user that loading is in progress.
        /// </summary>
        /// <param name="show">if set to <c>true</c> [show].</param>
        private void DisplayLoadStatus(bool show)
        {
            txtbrdStatus.UseWaitCursor = show;
            grdviewResponse.UseWaitCursor = show;
            txtbrdStatus.Visible = show;
        }

        #region Events Handlers

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Load" /> event, which initializes
        /// the form.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            await InitializeForm();
        }

        /// <summary>
        /// Called when text is changed on a textbox. This validates the
        /// dynamically created parameter controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TextChanged_RuleChecker(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                ValidateParameterTextBox((TextBox)sender);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSend control, and starts
        /// the SOAP transaction.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private async void btnSend_Click(object sender, EventArgs e)
        {
            await ProcessSOAPTransaction();
        }

        /// <summary>
        /// Handles the SelectionChangeCommitted event of the drpdwnWebServices control, and updates
        /// the method dropdown to show the methods associated with the now chosen web service.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void drpdwnWebServices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WebService selectedService = _libraryManager.GetService(SelectedWebServiceURL);
            drpdwnMethods.DataSource = selectedService.Methods;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the drpdwnMethods control, which will 
        /// create parameter controls in the flow layout for user input.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void drpdwnMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            _parameterManager.ActiveMethod = _libraryManager.GetMethod(SelectedWebServiceURL, SelectedMethodName);
            txtBoxMethodDescription.Text = _parameterManager.ActiveMethod.Documentation;
            CreateParameterControls();
        }

        #endregion
    }
}
