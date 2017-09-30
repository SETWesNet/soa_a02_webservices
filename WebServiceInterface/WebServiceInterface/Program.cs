using System;
using System.Windows.Forms;

namespace WebServiceInterface
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Called when an uncaught exception was thrown. This thread logs the exception
        /// and cleanly closes the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Threading.ThreadExceptionEventArgs"/> instance containing the event data.</param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Logger.Log(e.Exception);
            MessageBox.Show(@"An unknown error has occurred and the application
               needs to shut down. Please see logs for more information.", "Fatal Error",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }
    }
}
