using System;
using System.Collections.Generic;
using System.Linq;
using Config = Statistiks.Configuration.Configuration;

namespace Statistiks.Lib
{
    public class StatistiksLib
    {
        private ScreenshotTaker _screenshotTaker;
        private KeyboardHook _keyboardHook;
        private MouseHook _mouseHook;
        private WindowHook _windowHook;

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
            if (Config.Instance.KeyboardCapturingEnabled)
            {
                _keyboardEvents = new Dictionary<string, ulong>();
                _keyboardHook = new KeyboardHook();
                _keyboardHook.EventRaised += _kHookEventRaised;
            }
            if (Config.Instance.MouseCapturingEnabled)
            {
                _mouseEvents = new Dictionary<MouseMessage, double>();
                _mouseHook = new MouseHook();
                _mouseHook.EventRaised += _mHookEventRaised;
            }
            if (Config.Instance.WindowCapturingEnabled)
            {
                _windowEvents = new Dictionary<string, ulong>();
                _activeWindow = IntPtr.Zero;
                _windowHook = new WindowHook();
                _windowHook.EventRaised += _wHookEventRaised;
            }
            if (Config.Instance.ScreenshotTakerEnabled)
            {
                _screenshotTaker = new ScreenshotTaker(900000);
            }
        }

        private void _wHookEventRaised(object sender, WindowEventArgs e)
        {
            _activeWindow = e.hWnd;
            e.ExePath = string.IsNullOrEmpty(e.ExePath) ? "LockScreen" : e.ExePath;
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
            if (Config.Instance.ScreenshotTakerEnabled)
            {
                _screenshotTaker.Stop();
            }
            if (Config.Instance.KeyboardCapturingEnabled)
            {
                _keyboardHook.Unhook();
            }
            if (Config.Instance.MouseCapturingEnabled)
            {
                _mouseHook.Unhook();
            }
            if (Config.Instance.WindowCapturingEnabled)
            {
                _windowHook.Unhook();
            }
        }
    }
}
