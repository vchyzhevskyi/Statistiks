using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace Statistiks.Lib
{
    internal class ScreenshotTaker
    {
        private Timer _tTimer;

        public ScreenshotTaker(uint delay)
        {
            _tTimer = new Timer(TimerCallback, null, 1000, delay);
        }

        private void TimerCallback(object state)
        {
            try
            {
                var scrsht = new Bitmap(Screen.AllScreens.Sum(x => x.Bounds.Width), Screen.AllScreens.Max(x => x.Bounds.Height));
                Graphics.FromImage(scrsht)
                        .CopyFromScreen(0, 0, 0, 0,
                                        new Size(Screen.AllScreens.Sum(x => x.Bounds.Width), Screen.AllScreens.Max(x => x.Bounds.Height)));
                if (
                    !Directory.Exists(string.Format(@"{0}\Statistiks\Screenshots\",
                                                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData))))
                    Directory.CreateDirectory(string.Format(@"{0}\Statistiks\Screenshots\",
                                                            Environment.GetFolderPath(
                                                                Environment.SpecialFolder.ApplicationData)));
                scrsht.Save(
                    string.Format(@"{0}\Statistiks\Screenshots\{1}.png",
                                  Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                  DateTime.Now.ToString("yyyyMMddHHmmss")), ImageFormat.Png);
            }
            catch (Exception)
            {
            }
        }

        public void Stop()
        {
            _tTimer = null;
        }
    }
}