using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Statistiks.Lib
{
    internal class WindowEventArgs : EventArgs
    {
        public string Title;
        public string ExePath;
    }

    internal class WindowHook
    {
        private Timer _tTimer;

        internal delegate void WindowHookEventHandler(object sender, WindowEventArgs e);
        internal event WindowHookEventHandler EventRaised;

        internal class Window
        {
            public string Title;
            public string ExePath;
        }

        public WindowHook()
        {
            _tTimer = new Timer(TimerCallback, null, 1000, 1000);
        }

        private void OnEventRaised(WindowEventArgs e)
        {
            if (EventRaised != null)
                EventRaised(this, e);
        }

        private void TimerCallback(object state)
        {
            var wData = GetForegroundWindowInfo();
            OnEventRaised(new WindowEventArgs()
            {
                Title = wData.Title,
                ExePath = wData.ExePath
            });
        }

        private Window GetForegroundWindowInfo()
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
