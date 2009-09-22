using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace Twarkee
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (ApplicationIsAlreadyRunning())
            {
                Application.Exit();
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
        
        private static bool ApplicationIsAlreadyRunning()
        {
            Process currentProcesses = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcesses.ProcessName);
            return processes.Length > 1;
        }
    }
}