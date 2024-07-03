using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace activity
{
    internal class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();
        static void Main(string[] args)
        {
            Activity activity = new Activity();
            var handle = GetConsoleWindow();
            activity.addToTaskbar(handle);

            Thread consoleThread = new Thread(activity.ConsoleOperations);
            consoleThread.Start();

            Application.Run();
        }
    }
}
