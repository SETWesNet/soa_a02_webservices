﻿using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServiceInterface.Library;
using System.Text.RegularExpressions;
using System.Media;
using System.Windows.Forms.Extensions;
using System.Data;
using System.IO;
using System.Resources;
using System.Threading;

namespace WebServiceInterface
{
    public partial class MainForm : Form
    {
        private LibraryManager _configLibrary = new LibraryManager("config.json");
        private ResourceManager _resourceManager = new ResourceManager(typeof(MainForm));
        private ToolTip _errorTooltip = new ToolTip();


        public MainForm()
        {
            InitializeComponent();

            /* Initialize Drop Downs */
            drpdwnWebServices.DisplayMember = nameof(WebService.Name);
            drpdwnWebServices.ValueMember = nameof(WebService.Url);
            drpdwnWebServices.DataSource = _configLibrary.Services;

            drpdwnMethods.DisplayMember = nameof(Method.Name);
            drpdwnMethods.ValueMember = nameof(Method.Name);
            WebService selectedService = _configLibrary.GetService(SelectedWebServiceURL);
            drpdwnMethods.DataSource = selectedService.Methods;
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
        /// Deletes old parameter controls that are located in the flow layout and
        /// places new controls in their place, based on the parameters required for
        /// the currently selected web service method. The controls Name property will
        /// be set to the name of the parameter they take input for.
        /// </summary>
        private void CreateParameterControls()
        {
            /* Hide the tooltip if showing on any controls (bug in .NET, will
             * crash if the object it is displaying on is disposed beore fade out). */
            foreach (Control control in flwParameters.Controls)
            {
                _errorTooltip.Hide(control);
            }

            /* Dispose all controls from flow layout and get current selected method */
            flwParameters.Controls.DisposeAll();
            Method selectedMethod = _configLibrary.GetMethod(SelectedWebServiceURL, SelectedMethodName);

            /* Create input controls for parameters, assign textchanged event for validation for textbox */
            foreach (Parameter param in selectedMethod.Parameters)
            {
                Control paramInputControl = null;
                bool paramIsBoolean = string.Equals(param.Type, "bool", StringComparison.OrdinalIgnoreCase);

                if (paramIsBoolean)
                {
                    paramInputControl = new CheckBox();
                    paramInputControl.Text = string.Format("{0} ({1})", param.Name, param.Type);
                }
                else
                {
                    Label textBoxLabel = new Label();
                    textBoxLabel.Text = string.Format("{0} ({1})", param.Name, param.Type);
                    flwParameters.Controls.Add(textBoxLabel);

                    paramInputControl = new TextBox();
                    ((TextBox)paramInputControl).ShortcutsEnabled = false;
                    paramInputControl.TextChanged += TextChanged_RuleChecker;
                }

                /* Name control based on the parameter it takes input for, put into flow layout */
                paramInputControl.Name = param.Name;
                flwParameters.Controls.Add(paramInputControl);
            }
        }

        /// <summary>
        /// Iterates through the parameter flow layout controls, associating all
        /// values from textboxes and checkboxes with the parameter they were taking
        /// input for by relating the name of the controls to the name of parameters of
        /// the currently selected method.
        /// </summary>
        /// <returns>Arguments for the current selected method.</returns>
        private SOAPArgument[] CollectMethodArguments()
        {
            List<SOAPArgument> arguments = new List<SOAPArgument>();
            Method selectedMethod = _configLibrary.GetMethod(SelectedWebServiceURL, SelectedMethodName);

            /* Create argument objects by getting the value of the associated controls in the flow layout  */
            foreach (Control control in flwParameters.Controls)
            {
                if (control is TextBox)
                {
                    SOAPArgument argument = new SOAPArgument();
                    argument.Parameter = selectedMethod.Parameters.First(parameter => parameter.Name == control.Name);
                    argument.Value = control.Text;
                    arguments.Add(argument);
                }
                else if (control is CheckBox)
                {
                    SOAPArgument argument = new SOAPArgument();
                    argument.Parameter = selectedMethod.Parameters.First(parameter => parameter.Name == control.Name);
                    argument.Value = ((CheckBox)control).Checked.ToString();
                    arguments.Add(argument);
                }
            }
            return arguments.ToArray();
        }

        /// <summary>
        /// Validates the text value of a parameter textbox, ensuring that the value entered
        /// is satisfying the regex pattern for its corresponding parameter. If the regex 
        /// pattern for the parameter is not valid, an error message is displayed and the change
        /// reverted.
        /// </summary>
        /// <param name="parameterTextBox">A textbox with the name corresponding to the name of a parameter.</param>
        private void ValidateParameterTextBox(TextBox parameterTextBox)
        {
            /* Use the name of the textbox to get the corresponding parameter it is taking input for */
            Parameter parameter = _configLibrary.GetParameter(SelectedWebServiceURL,
                SelectedMethodName, parameterTextBox.Name);

            /* If there is a regex pattern for the paramater and text to validate, do so */
            if (!string.IsNullOrEmpty(parameterTextBox.Text) &&
                !string.IsNullOrEmpty(parameter.Regex) &&
                !Regex.IsMatch(parameterTextBox.Text, parameter.Regex))
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
        private async Task<string> CallWebServiceMethod()
        {
            /* Get all the arguments entered by the user */
            SOAPArgument[] arguments = CollectMethodArguments();

            /* Get the currently selected method and call the web service method using user arguments (if any) */
            Method selectedMethod = _configLibrary.GetMethod(SelectedWebServiceURL, SelectedMethodName);
            SOAPWebService webService = new SOAPWebService(@"http://www.webservicex.net/airport.asmx?"); //TODO (Kyle): Make this not hardcoded
            string soapResponse = await webService.CallMethodAsync(selectedMethod, arguments);

            return soapResponse;
        }

        /// <summary>
        /// Displays a SOAP response XML string in the forms
        /// DataGridView area.
        /// </summary>
        /// <param name="soapResponse">A SOAP response string.</param>
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

                    /* Display the response in the data grid view */
                    DataSet dataSet = new DataSet();
                    dataSet.ReadXml(new StringReader(soapResponse));
                    grdviewResponse.DataSource = dataSet.Tables[0];
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Logger.Log(ex, _resourceManager.GetString("Error_ResponseDisplay_Message"));
                MessageBox.Show(this, _resourceManager.GetString("Error_ResponseDisplay_Message"),
                _resourceManager.GetString("Error_ResponseDisplay_Caption"),
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Events Handlers

        private void TextChanged_RuleChecker(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                ValidateParameterTextBox((TextBox)sender);
            }
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string response = await CallWebServiceMethod();
            Thread t = new Thread(new ThreadStart(() => DisplaySoapResponse(response)));
            t.Start();
        }

        private void drpdwnWebServices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            WebService selectedService = _configLibrary.GetService(SelectedWebServiceURL);
            drpdwnMethods.DataSource = selectedService.Methods;
        }

        private void drpdwnMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateParameterControls();
        }

        #endregion
    }
}
