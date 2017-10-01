using System.Linq;

namespace System.Windows.Forms.Extensions
{
    /// <summary>
    /// Contains extension methods that act upon ControlCollection objects.
    /// </summary>
    public static class ControlCollectionExtensions
    {
        /// <summary>
        /// Clears the entire control collection by removing each control
        /// and disposing it.
        /// </summary>
        /// <param name="controlCollection">The control collection to modify.</param>
        public static void DisposeAll(this Control.ControlCollection controlCollection)
        {
            /* Get a list of all current parameter controls */
            Control[] listControls = controlCollection.ToArray();

            /* Remove each parameter control from the flow layout */
            foreach (Control control in listControls)
            {
                control.Dispose();
            }
        }

        /// <summary>
        /// Returns the control collection as an array of controls.
        /// </summary>
        /// <param name="controlCollection">The control collection.</param>
        /// <returns>An array of controls contained within the collection.</returns>
        public static Control[] ToArray(this Control.ControlCollection controlCollection)
        {
            return controlCollection.Cast<Control>().ToArray();
        }
    }
}
