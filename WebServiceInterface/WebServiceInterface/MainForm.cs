using System;
using System.Windows.Forms;
using System.Linq;
using System.Collections.Generic;
using WebServiceInterface.Library;
using System.Xml;

namespace WebServiceInterface
{
    public partial class MainForm : Form
    {
        private LibraryManager configLibrary;
        private string configPath;

        public MainForm()
        {
            InitializeComponent();
            configPath = "config.json";
            configLibrary = new LibraryManager(configPath);
        }

        /// <summary>
        /// Creates the parameter controls.
        /// </summary>
        private void CreateParameterControls()
        {
            /* Get a list of all current parameter controls */
            List<Control> listControls = flwParameters.Controls.Cast<Control>().ToList();

            /* Remove each parameter control from the flow layout */
            foreach (Control control in listControls)
            {
                flwParameters.Controls.Remove(control);
                control.Dispose();
            }

            /* Get the current web service method being targeted */
            Method[] methods = (Method[])drpdwnMethods.DataSource;
            Method selectedMethod = methods.First(m => m.Name == (string)drpdwnMethods.SelectedValue);

            /* Create controls for parameter value input in the flow layout */
            foreach (Parameter param in selectedMethod.Parameters)
            {
                Label paramLabel = new Label();
                paramLabel.Text = param.Name;

                TextBox paramTextBox = new TextBox();
                paramTextBox.Name = param.Name;

                flwParameters.Controls.Add(paramLabel);
                flwParameters.Controls.Add(paramTextBox);
            }
        }

        #region Events Handlers

        private void MainForm_Load(object sender, EventArgs e)
        {
            drpdwnWebServices.DisplayMember = nameof(WebService.Name);
            drpdwnWebServices.ValueMember = nameof(WebService.Url);
            drpdwnWebServices.DataSource = configLibrary.Services;

            drpdwnMethods.DisplayMember = nameof(Method.Name);
            drpdwnMethods.ValueMember = nameof(Method.Name);
            drpdwnMethods.DataSource = configLibrary.GetAvailableMethods((string)drpdwnWebServices.SelectedValue);
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            SOAPWebService webService = new SOAPWebService(@"http://www.webservicex.net/airport.asmx");

            // NOTE(c-jm): There was a bug here when trying to call method async. I think it is because we are sending null. Look into it tomorrow.
            //richtxtReturnValue.Text = await webService.CallMethodAsync(null);
        }

        private void drpdwnWebServices_SelectionChangeCommitted(object sender, EventArgs e)
        {
            drpdwnMethods.DataSource = configLibrary.GetAvailableMethods((string)drpdwnWebServices.SelectedValue);
        }

        private void drpdwnMethods_SelectedIndexChanged(object sender, EventArgs e)
        {
            CreateParameterControls();
        }

        #endregion
    }
}
