using System;
using System.Collections.Generic;
using System.Linq;

namespace Statistiks.Lib
{
    public class StatistiksLib
    {
        private KeyboardHook _kHook;
        private MouseHook _mHook;
        private WindowHook _wHook;

        #region Data
        private Dictionary<string, ulong> _keyboardEvents;
        private Dictionary<MouseMessage, double> _mouseEvents;
        private Dictionary<string, ulong> _windowEvents;
        private IntPtr _activeWindow;
        #endregion

        #region Properties
        public Dictionary<string, ulong> KeyboardEvents { get { return _keyboardEvents; } }
        public Dictionary<MouseMessage, double> MouseEvents { get { return _mouseEvents; } }
        public Dictionary<string, ulong> WindowEvents { get { return _windowEvents; } }
        #endregion

        public StatistiksLib()
        {
            _keyboardEvents = new Dictionary<string, ulong>();
            _mouseEvents = new Dictionary<MouseMessage, double>();
            _windowEvents = new Dictionary<string, ulong>();
            _activeWindow = IntPtr.Zero;
            _kHook = new KeyboardHook();
            _kHook.EventRaised += _kHookEventRaised;
            _mHook = new MouseHook();
            _mHook.EventRaised += _mHookEventRaised;
            _wHook = new WindowHook();
            _wHook.EventRaised += _wHookEventRaised;
        }

        private void _wHookEventRaised(object sender, WindowEventArgs e)
        {
            _activeWindow = e.hWnd;
            if (_windowEvents.ContainsKey(e.ExePath))
                _windowEvents[e.ExePath] += 1;
            else
                _windowEvents.Add(e.ExePath, 1);
        }

        private void _mHookEventRaised(object sender, MouseEventArgs e)
        {
            if (_mouseEvents.ContainsKey(e.Message))
                _mouseEvents[e.Message] += e.Message == MouseMessage.WM_MOUSEMOVE ? e.MovePath : 1;
            else
                _mouseEvents.Add(e.Message, e.Message == MouseMessage.WM_MOUSEMOVE ? e.MovePath : 1);
        }

        private void _kHookEventRaised(object sender, KeyboardHookEventArgs e)
        {
            if (e.Message == KeyboardMessage.WM_KEYDOWN || e.Message == KeyboardMessage.WM_SYSKEYDOWN)
            {
                var key = e.VkCode.KeyCodeToUnicodeString(e.ScanCode, _activeWindow, _keyboardEvents.Keys.Skip(_keyboardEvents.Count - 3).Take(3).ToArray());
                if (key == "RButton" || key == "LButton" || key == "MButton" || key == "XButton1" || key == "XButton2")
                    return; // TODO: decide what need to do with mouse keys
                if (_keyboardEvents.ContainsKey(key))
                    _keyboardEvents[key] += 1;
                else
                    _keyboardEvents.Add(key, 1);
            }
        }

        public void Unhook()
        {
            _kHook.Unhook();
            _mHook.Unhook();
            _wHook.Unhook();
        }
    }
}
