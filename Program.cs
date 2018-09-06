using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelegramReader
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            InitializeExceptionHandler();
            Application.Run(new MainForm());
        }

        private static void Form1_UIThreadException(object sender, ThreadExceptionEventArgs e)
        {
            Log.Error("Fatal", e.Exception.Message);
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Log.Error("Fatal", e.ExceptionObject.ToString());
        }

        private static void InitializeExceptionHandler()
        {
            // Add the event handler for handling UI thread exceptions to the event.
            Application.ThreadException += new
            ThreadExceptionEventHandler(Form1_UIThreadException);

            // Set the unhandled exception mode to force all Windows Forms errors
            // to go through our handler.
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // Add the event handler for handling non-UI thread exceptions to the event. 
            AppDomain.CurrentDomain.UnhandledException += new
            UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

        }
    }
}
