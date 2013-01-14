using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace StatistiksLib
{
    internal class HookBase
    {
        private readonly HookType _hookType;
        private readonly HookProc _hook;
        private readonly IntPtr _hHook;
        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        internal delegate void HookEventHandler(object sender, HookEventArgs e);
        internal event HookEventHandler Invoked;

        public HookBase(HookType type)
        {
            _hookType = type;
            _hook = new HookProc(HookProcBase);
            using (Process p = Process.GetCurrentProcess())
            using (ProcessModule pm = p.MainModule)
                _hHook = SetWindowsHookEx((int)_hookType, _hook, GetModuleHandle(pm.ModuleName), 0);
        }

        protected void OnInvoked(HookEventArgs e)
        {
            if (Invoked != null)
                Invoked(this, e);
        }

        private int HookProcBase(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
                return CallNextHookEx(_hHook, nCode, wParam, lParam);
            HookEventArgs e = new HookEventArgs()
            {
                code = nCode,
                wParam = wParam,
                lParam = lParam
            };
            OnInvoked(e);
            return CallNextHookEx(_hHook, nCode, wParam, lParam);
        }

        #region Win32 Imports
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr hook, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern int UnhookWindowsHookEx(IntPtr hhook);
        #endregion
    }
}
