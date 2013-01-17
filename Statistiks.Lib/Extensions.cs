using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Statistiks.Lib
{
    public static class Extensions
    {
        public static string KeyCodeToUnicodeString(this uint vkCode, uint scanCode, IntPtr hWndSender, string[] prevEvents)
        {
            var key = ((VkKey)vkCode).ToString();
            uint tmp = 0;
            if (!uint.TryParse(key, out tmp))
                return key;
            StringBuilder sbBuff = new StringBuilder(2);
            byte[] bKeyState = new byte[256];
            if (!GetKeyboardState(bKeyState))
                return "Unknown";
            bKeyState[(int)VkKey.Control] = prevEvents.Contains(VkKey.LControl.ToString()) || prevEvents.Contains(VkKey.RControl.ToString()) ? (byte)0x80 : bKeyState[(uint)VkKey.Control];
            bKeyState[(int)VkKey.Menu] = prevEvents.Contains(VkKey.LMenu.ToString()) || prevEvents.Contains(VkKey.RMenu.ToString()) ? (byte)0x80 : bKeyState[(uint)VkKey.Menu];
            bKeyState[(int)VkKey.Shift] = prevEvents.Contains(VkKey.LShift.ToString()) || prevEvents.Contains(VkKey.RShift.ToString()) ? (byte)0x80 : bKeyState[(uint)VkKey.Shift];
            bKeyState[(int)VkKey.Capital] = bKeyState[(uint)VkKey.Capital] == 1 ? (byte)0x01 : bKeyState[(uint)VkKey.Capital];
            ToUnicodeEx(vkCode, scanCode, bKeyState, sbBuff, 2, 0, GetKeyboardLayout(GetWindowThreadProcessId(hWndSender, (uint)0)));
            return sbBuff.ToString() != string.Empty ? sbBuff.ToString() : "Unknown";
        }

        private enum VkKey : uint
        {
            Cancel = 0x03,
            Back = 0x08,
            Tab = 0x09,
            Clear = 0x0C,
            Return = 0x0D,
            Shift = 0x10,
            Control = 0x11,
            Menu = 0x12,
            Pause = 0x13,
            Capital = 0x14,
            ImeKana = 0x15,
            ImeHanguel = 0x15,
            ImeHangul = 0x15,
            ImeJunja = 0x17,
            ImeFinal = 0x18,
            ImeHanja = 0x19,
            ImeKanji = 0x19,
            Escape = 0x1B,
            ImeConvert = 0x1C,
            ImeNonconvert = 0x1D,
            ImeAccept = 0x1E,
            ImeModechange = 0x1F,
            Space = 0x20,
            Prior = 0x21,
            Next = 0x22,
            End = 0x23,
            Home = 0x24,
            Left = 0x25,
            Up = 0x26,
            Right = 0x27,
            Down = 0x28,
            Select = 0x29,
            Print = 0x2A,
            Execute = 0x2B,
            Snapshot = 0x2C,
            Insert = 0x2D,
            Delete = 0x2E,
            Help = 0x2F,
            LWin = 0x5B,
            RWin = 0x5C,
            Apps = 0x5D,
            Sleep = 0x5F,
            NumPad0 = 0x60,
            NumPad1 = 0x61,
            NumPad2 = 0x62,
            NumPad3 = 0x63,
            NumPad4 = 0x64,
            NumPad5 = 0x65,
            NumPad6 = 0x66,
            NumPad7 = 0x67,
            NumPad8 = 0x68,
            NumPad9 = 0x69,
            Multiply = 0x6A,
            Add = 0x6B,
            Separator = 0x6C,
            Subtract = 0x6D,
            Decimal = 0x6E,
            Divide = 0x6F,
            F1 = 0x70,
            F2 = 0x71,
            F3 = 0x72,
            F4 = 0x73,
            F5 = 0x74,
            F6 = 0x75,
            F7 = 0x76,
            F8 = 0x77,
            F9 = 0x78,
            F10 = 0x79,
            F11 = 0x7A,
            F12 = 0x7B,
            F13 = 0x7C,
            F14 = 0x7D,
            F15 = 0x7E,
            F16 = 0x7F,
            F17 = 0x80,
            F18 = 0x81,
            F19 = 0x82,
            F20 = 0x83,
            F21 = 0x84,
            F22 = 0x85,
            F23 = 0x86,
            F24 = 0x87,
            Numlock = 0x90,
            Scroll = 0x91,
            LShift = 0xA0,
            RShift = 0xA1,
            LControl = 0xA2,
            RControl = 0xA3,
            LMenu = 0xA4,
            RMenu = 0xA5,
            Browser_Back = 0xA6,
            Browser_Forward = 0xA7,
            Browser_Refresh = 0xA8,
            Browser_Stop = 0xA9,
            Browser_Search = 0xAA,
            Browser_Favorites = 0xAB,
            Browser_Home = 0xAC,
            Volume_Mute = 0xAD,
            Volume_Down = 0xAE,
            Volume_Up = 0xAF,
            Media_Next_Track = 0xB0,
            Media_Prev_Track = 0xB1,
            Media_Stop = 0xB2,
            Media_Play_Pause = 0xB3,
            Launch_Mail = 0xB4,
            Launch_Media_Select = 0xB5,
            Launch_App1 = 0xB6,
            Launch_App2 = 0xB7,
            Oem_1 = 0xBA,
            Oem_Plus = 0xBB,
            Oem_Comma = 0xBC,
            Oem_Minus = 0xBD,
            Oem_Period = 0xBE,
            Oem_2 = 0xBF,
            Oem_3 = 0xC0,
            Oem_4 = 0xDB,
            Oem_5 = 0xDC,
            Oem_6 = 0xDD,
            Oem_7 = 0xDE,
            Oem_8 = 0xDF,
            Oem_102 = 0xE2,
            ImeProcessKey = 0xE5,
            Packet = 0xE7,
            Attn = 0xF6,
            Crsel = 0xF7,
            Exsel = 0xF8,
            Ereof = 0xF9,
            Play = 0xFA,
            Zoom = 0xFB,
            NoName = 0xFC,
            Pa1 = 0xFD,
            Oem_Clear = 0xFE
        }

        #region Win32 Imports
        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFalgs, IntPtr dwhkl);

        [DllImport("user32.dll")]
        private static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint idThread);
        #endregion
    }
}
