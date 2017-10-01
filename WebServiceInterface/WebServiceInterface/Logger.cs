/* 
 *  
 *  Filename: Logger.cs
 *  
 *  Date: 2017-10-01
 *  
 *  Name: Colin Mills, Kyle Kreutzer
 *  
 * Description:
 * Holds the definition of the Logger class
 * 
 */

using System;
using System.IO;

namespace WebServiceInterface
{
    /// <summary>
    /// Used for logging exceptions to a file.
    /// </summary>
    static class Logger
    {
        private static Object logLock = new Object();

        /// <summary>
        /// Logs the specified exception to a logs.log file, with each
        /// entry being seperated with a line of the '-' character. This
        /// method is thread safe.
        /// </summary>
        /// <param name="ex">The Exception to log.</param>
        /// <param name="additionalInfo">Additional text information to associate with the exception/param>
        public static void Log(Exception ex, string additionalInfo = null)
        {
            lock (logLock)
            {
                string logText = ex.ToString();

                if (!string.IsNullOrEmpty(additionalInfo))
                {
                    logText += "\nAdditonalInfo:\n" + additionalInfo;
                }

                using (StreamWriter w = File.AppendText("logs.log"))
                {
                    w.WriteLine(DateTime.Now);
                    w.WriteLine(logText);
                    w.WriteLine(new string('-', 160));
                }
            }
        }

        /// <summary>
        /// Logs a pure text message to the log file seperated by '-'. This method is threadsafe.
        /// </summary>
        /// <param name="logText">The log text.</param>
        public static void LogMessage(string logText)
        {
            lock (logLock)
            {
                using (StreamWriter w = File.AppendText("logs.log"))
                {
                    w.WriteLine(DateTime.Now);
                    w.WriteLine(logText);
                    w.WriteLine(new string('-', 160));
                }
            }
        }
    }
}
