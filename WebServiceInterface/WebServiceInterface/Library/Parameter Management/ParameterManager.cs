/* 
 *  
 *  Filename: WSDLTypeInformation.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the ParameterManager class
 * 
 */

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WebServiceInterface.Library
{
    /// <summary>
    /// Responsible for dynamically creating, validating, and managing
    /// controls that are used for entering parameter information for a 
    /// web service.
    /// </summary>
    class ParameterManager
    {
        Method _activeMethod;

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterManager"/> class.
        /// </summary>
        /// <param name="activeMethod">The active method.</param>
        public ParameterManager(Method activeMethod)
        {
            _activeMethod = activeMethod;
        }

        #endregion  

        #region Properties

        public Method ActiveMethod
        {
            get => _activeMethod;
            set => _activeMethod = value;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates controls that can be used to input arguments for the active web methods
        /// parameters. Each controls name property is matched to the name of the parameter
        /// it represents.
        /// </summary>
        /// <returns>Array of controls.</returns>
        public Control[] CreateParameterControls()
        {
            List<Control> controlList = new List<Control>();

            if (_activeMethod.Parameters != null)
            {
                /* Create input controls for parameters */
                foreach (Parameter param in _activeMethod.Parameters)
                {
                    Control paramInputControl = null;
                    bool paramIsBoolean = param.Type.Contains("bool");

                    if (paramIsBoolean)
                    {
                        paramInputControl = new CheckBox();
                        paramInputControl.Text = CreateParameterString(param);
                    }
                    else
                    {
                        Label textBoxLabel = new Label();
                        textBoxLabel.Size = new Size(200, textBoxLabel.Size.Height);
                        textBoxLabel.Text = CreateParameterString(param);
                        controlList.Add(textBoxLabel);

                        paramInputControl = new TextBox();
                        ((TextBox)paramInputControl).ShortcutsEnabled = false;
                        ((TextBox)paramInputControl).MaxLength = 256;
                    }

                    /* Name control based on the parameter it takes input for, put into flow layout */
                    paramInputControl.Name = param.Name;
                    controlList.Add(paramInputControl);
                }
            }
            return controlList.ToArray();
        }

        /// <summary>
        /// Iterates through a list of parameter controls created by <see cref="ParameterManager.CreateParameterControls"/>
        /// and collects the arguments entered by the user for each control. The arguments are associated with their parameter
        /// and returned as a <see cref="SOAPArgument"/> object.
        /// </summary>
        /// <param name="parameterControls">Controls created by the <see cref="ParameterManager.CreateParameterControls"/> method.</param>
        /// <returns>SOAPArgument objects containing the parameter and associated argument.</returns>
        public SOAPArgument[] CollectParameterArguments(Control[] parameterControls)
        {
            List<SOAPArgument> arguments = new List<SOAPArgument>();

            /* Create argument objects by getting the value of the associated controls in the flow layout  */
            foreach (Control control in parameterControls)
            {
                if (control is TextBox)
                {
                    SOAPArgument argument = new SOAPArgument();
                    argument.Parameter = _activeMethod.Parameters.First(parameter => parameter.Name == control.Name);
                    argument.Value = control.Text;
                    arguments.Add(argument);
                    control.Text = "";
                }
                else if (control is CheckBox)
                {
                    SOAPArgument argument = new SOAPArgument();
                    argument.Parameter = _activeMethod.Parameters.First(parameter => parameter.Name == control.Name);
                    argument.Value = ((CheckBox)control).Checked.ToString();
                    arguments.Add(argument);
                }
            }
            return arguments.ToArray();
        }

        /// <summary>
        /// Validates a textbox control created by <see cref="ParameterManager.CreateParameterControls"/>
        /// and ensures that the text is valid based on the parameter it represents.
        /// </summary>
        /// <returns>true if valid, false otherwise</returns>
        public bool ValidateRegex(TextBox parameterTextBox)
        {
            bool valid = true;

            /* Get parameter associated with the textbox */
            Parameter parameter = _activeMethod.Parameters.FirstOrDefault(p => p.Name == parameterTextBox.Name);

            /* Validate if the text in the textbox matches parameter REGEX rule */
            if (!string.IsNullOrEmpty(parameterTextBox.Text) &&
                !string.IsNullOrEmpty(parameter.Regex) &&
                !Regex.IsMatch(parameterTextBox.Text, parameter.Regex))
            {
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Validates a list of parameter controls created by <see cref="ParameterManager.CreateParameterControls"/>
        /// by checking if they require a value. If any control in the passed array is required but doesn't have
        /// any input associated with it, this method returns false.
        /// </summary>
        /// <param name="parameterControls">The parameter controls.</param>
        /// <returns></returns>
        public bool ValidateRequired(Control[] parameterControls)
        {
            bool allValid = true;

            foreach (Control control in parameterControls)
            {
                if (control is TextBox)
                {
                    /* Find parameter associated with this control */
                    Parameter param = _activeMethod.Parameters.FirstOrDefault(p => p.Name == control.Name);

                    /* If required and field is blank, not valid */
                    if (param.Required && string.IsNullOrWhiteSpace(control.Text))
                    {
                        allValid = false;
                        break;
                    }
                }
            }
            return allValid;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a string that contains the name of the parameter,
        /// it's type (in brackets), and an asterisk at the end if the
        /// parameter is required.
        /// </summary>
        /// <param name="param">Parameter to make string for.</param>
        /// <returns>string</returns>
        private static string CreateParameterString(Parameter param)
        {
            /* Create string containing the name of the parameter and its type */
            string caption = string.Format("{0} ({1})", param.Name, param.Type);

            /* Add asterisk if field is required */
            if (param.Required)
            {
                caption += '*';
            }

            return caption;
        }
    
        #endregion  
    }
}
