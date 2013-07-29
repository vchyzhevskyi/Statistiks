using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Statistiks.Lib
{
    public enum MouseMessage : int
    {
        WM_LBUTTONDOWN = 0x0201,
        WM_LBUTTONUP = 0x0202,
        WM_MOUSEMOVE = 0x0200,
        WM_MOUSEWHEEL = 0x020A,
        WM_MOUSEHWHEEL = 0x020E,
        WM_RBUTTONDOWN = 0x0204,
        WM_RBUTTONUP = 0x0205
    }

    internal class MouseEventArgs : EventArgs
    {
        public MouseMessage Message;
        public MousePoint Point;
        public long WheelDelta;
        public IntPtr ExtraInfo;
        public double MovePath;
    }

    [StructLayout(LayoutKind.Sequential)]
    internal class MousePoint
    {
        public int X;
        public int Y;
    }

    internal class MouseHook : HookBase
    {
        [StructLayout(LayoutKind.Sequential)]
        private class MSLLHOOKSTRUCT
        {
            public MousePoint pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        };

        private readonly int _dpi;
        private MousePoint _prevMPoint;
        internal delegate void MouseEventHandler(object sender, MouseEventArgs e);
        internal event MouseEventHandler EventRaised;

        internal MouseHook()
            : base(HookType.WH_MOUSE_LL)
        {
            _dpi = (int)Registry.CurrentConfig.OpenSubKey(@"Software\Fonts").GetValue("LogPixels");
            Invoked += I;
        }

        private void OnEventRaised(MouseEventArgs e)
        {
            if (EventRaised != null)
                EventRaised(this, e);
        }

        private void I(object sender, HookEventArgs e)
        {
            MSLLHOOKSTRUCT st = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(e.lParam, typeof(MSLLHOOKSTRUCT));
            _prevMPoint = _prevMPoint ?? st.pt;
            OnEventRaised(new MouseEventArgs()
            {
                Message = (MouseMessage)e.wParam,
                Point = st.pt,
                ExtraInfo = st.dwExtraInfo,
                WheelDelta = (MouseMessage)e.wParam == MouseMessage.WM_MOUSEWHEEL ? st.mouseData >> 16 & 0xFF : 0,
                MovePath = (MouseMessage)e.wParam == MouseMessage.WM_MOUSEMOVE ? Math.Sqrt(Math.Pow(_prevMPoint.X - st.pt.X, 2) + Math.Pow(_prevMPoint.Y - st.pt.Y, 2)) / (_dpi / 2.54) : 0
            });
            _prevMPoint = st.pt;
        }
    }
}
