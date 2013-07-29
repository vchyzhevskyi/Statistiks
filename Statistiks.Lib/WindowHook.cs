﻿using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Statistiks.Lib
{
    internal class WindowEventArgs : EventArgs
    {
        public string Title;
        public string ExePath;
        public IntPtr hWnd;
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
            public IntPtr hWnd;
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
                ExePath = wData.ExePath,
                hWnd = wData.hWnd
            });
        }

        private Window GetForegroundWindowInfo()
        {
            IntPtr hWnd = GetForegroundWindow();
            StringBuilder winCaption = new StringBuilder(GetWindowTextLength(hWnd) + 1);
            GetWindowText(hWnd, winCaption, winCaption.Capacity);
            uint pid = 0;
            GetWindowThreadProcessId(hWnd, out pid);
            StringBuilder exePath = new StringBuilder(1024);
            int size = exePath.Capacity;
            QueryFullProcessImageName(OpenProcess(0x1000, false, (int)pid), 0, exePath, out size);
            return new Window()
            {
                Title = winCaption.ToString(),
                ExePath = exePath.ToString(),
                hWnd = hWnd
            };
        }

        internal void Unhook()
        {
            _tTimer = null;
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

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        private static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, out int size);
        #endregion
    }
}
