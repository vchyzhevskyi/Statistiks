using System;
using System.Threading;
namespace StatistiksLib
{
    public class StatistiksLib
    {
        private KeyboardHook _kHook;
        private MouseHook _mHook;
        private WindowHook _wHook;
        private readonly Timer _tTimer;

        public StatistiksLib()
        {
            _kHook = new KeyboardHook();
            _mHook = new MouseHook();
            _wHook = new WindowHook();
            _tTimer = new Timer(TimerCallback, null, 0, 1000);
        }

        private void TimerCallback(object state)
        {
            var data = _wHook.GetForegroundWindowInfo();
            Console.WriteLine("Window: {0} {1}", data.Title, data.ExePath);
        }

        public void Unhook()
        {
            _kHook.Unhook();
            _mHook.Unhook();
        }
    }
}
