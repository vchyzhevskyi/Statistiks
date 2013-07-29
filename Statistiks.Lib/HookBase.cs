using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Statistiks.Lib
{
    internal class HookEventArgs
    {
        public int code;
        public IntPtr wParam;
        public IntPtr lParam;
    }

    internal enum HookType : int
    {
        WH_KEYBOARD_LL = 13,
        WH_MOUSE_LL = 14
    }

    internal class HookBase
    {
        private readonly HookType _hType;
        private readonly HookProc _hProc;
        private readonly IntPtr _hHook;
        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        protected delegate void HookEventHandler(object sender, HookEventArgs e);
        protected event HookEventHandler Invoked;

        internal HookBase(HookType type)
        {
            _hType = type;
            _hProc = new HookProc(HookProcBase);
            using (Process p = Process.GetCurrentProcess())
            using (ProcessModule pm = p.MainModule)
                _hHook = SetWindowsHookEx((int)_hType, _hProc, GetModuleHandle(pm.ModuleName), 0);
        }

        private void OnInvoked(HookEventArgs e)
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

        internal int Unhook()
        {
            return UnhookWindowsHookEx(_hHook);
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
