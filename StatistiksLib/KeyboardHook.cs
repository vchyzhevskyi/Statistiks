using System;
using System.Runtime.InteropServices;

namespace StatistiksLib
{
    internal class KeyboardHook : HookBase
    {
        internal enum KeyboardMessage : int
        {
            WM_KEYDOWN = 0x0100,
            WM_KEYUP = 0x0101,
            WM_SYSKEYDOWN = 0x0104,
            WM_SYSKEYUP = 0x0105
        }

        [StructLayout(LayoutKind.Sequential)]
        private class KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        internal class KeyboardHookEventArgs : EventArgs
        {
            public KeyboardMessage Message;
            public uint VkCode;
            public uint ScanCode;
            public bool ExtendedKey;
            public bool Injected;
            public bool AltDown;
            public bool Up;
        }

        private const int LLKHF_EXTENDED = 0x01;
        private const int LLKHF_INJECTED = 0x10;
        private const int LLKHF_ALTDOWN = 0x20;
        private const int LLKHF_UP = 0x80;

        internal delegate void KeyboardHookEventHandler(object sender, KeyboardHookEventArgs e);
        internal event KeyboardHookEventHandler EventRaised;

        internal KeyboardHook()
            : base(HookType.WH_KEYBOARD_LL)
        {
            Invoked += I;
        }

        protected void OnEventRaised(KeyboardHookEventArgs e)
        {
            if (EventRaised != null)
                EventRaised(this, e);
        }

        private void I(object sender, HookEventArgs e)
        {
            KBDLLHOOKSTRUCT st = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(KBDLLHOOKSTRUCT));
            KeyboardHookEventArgs args = new KeyboardHookEventArgs()
            {
                Message = (KeyboardMessage)e.wParam,
                VkCode = st.vkCode,
                ScanCode = st.scanCode,
                AltDown = (st.flags & LLKHF_ALTDOWN) > 0,
                ExtendedKey = (st.flags & LLKHF_EXTENDED) > 0,
                Injected = (st.flags & LLKHF_INJECTED) > 0,
                Up = (st.flags & LLKHF_UP) > 0
            };
            OnEventRaised(args);
        }
    }
}
