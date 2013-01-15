using System.Collections.Generic;

namespace Statistiks.Lib
{
    public class StatistiksLib
    {
        private KeyboardHook _kHook;
        private MouseHook _mHook;
        private WindowHook _wHook;

        #region Data
        private Dictionary<int, ulong> _keyboardEvents;
        private Dictionary<MouseMessage, ulong> _mouseEvents;
        private Dictionary<string, ulong> _windowEvents;
        #endregion

        #region Properties
        public Dictionary<int, ulong> KeyboardEvents { get { return _keyboardEvents; } }
        public Dictionary<MouseMessage, ulong> MouseEvents { get { return _mouseEvents; } }
        public Dictionary<string, ulong> WindowEvents { get { return _windowEvents; } }
        #endregion

        public StatistiksLib()
        {
            _keyboardEvents = new Dictionary<int, ulong>();
            _mouseEvents = new Dictionary<MouseMessage, ulong>();
            _windowEvents = new Dictionary<string, ulong>();

            _kHook = new KeyboardHook();
            _kHook.EventRaised += _kHookEventRaised;
            _mHook = new MouseHook();
            _mHook.EventRaised += _mHookEventRaised;
            _wHook = new WindowHook();
            _wHook.EventRaised += _wHookEventRaised;
        }

        private void _wHookEventRaised(object sender, WindowEventArgs e)
        {
            if (_windowEvents.ContainsKey(e.ExePath))
                _windowEvents[e.ExePath] += 1;
            else
                _windowEvents.Add(e.ExePath, 1);
        }

        private void _mHookEventRaised(object sender, MouseEventArgs e)
        {
            if (_mouseEvents.ContainsKey(e.Message))
                _mouseEvents[e.Message] += 1;
            else
                _mouseEvents.Add(e.Message, 1);
        }

        private void _kHookEventRaised(object sender, KeyboardHookEventArgs e)
        {
            if (e.Up && _keyboardEvents.ContainsKey((int)e.VkCode))
                _keyboardEvents[(int)e.VkCode] += 1;
            else if (e.Up)
                _keyboardEvents.Add((int)e.VkCode, 1);
        }

        public void Unhook()
        {
            _kHook.Unhook();
            _mHook.Unhook();
        }
    }
}
