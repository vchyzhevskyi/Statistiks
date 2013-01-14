using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace StatistiksLib
{
    internal class Window
    {
        public string Title;
        public string ExePath;
    }

    internal class WindowHook
    {
        internal Window GetForegroundWindowInfo()
        {
            IntPtr hWnd = GetForegroundWindow();
            StringBuilder winCaption = new StringBuilder(GetWindowTextLength(hWnd) + 1);
            GetWindowText(hWnd, winCaption, winCaption.Capacity);
            uint pid = 0;
            GetWindowThreadProcessId(hWnd, out pid);
            return new Window()
            {
                Title = winCaption.ToString(),
                ExePath = Process.GetProcessById((int)pid).MainModule.FileName
            };
        }

        #region Win32 Imports
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int maxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        #endregion
    }
}
