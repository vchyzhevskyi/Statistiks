namespace StatistiksLib
{
    public class StatistiksLib
    {
        private KeyboardHook _kHook;
        private MouseHook _mHook;

        public StatistiksLib()
        {
            _kHook = new KeyboardHook();
            _mHook = new MouseHook();
        }

        public void Unhook()
        {
            _kHook.Unhook();
            _mHook.Unhook();
        }
    }
}
