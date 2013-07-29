using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;
using Statistiks.Lib;
using Statistiks.Report;

namespace Statistiks.App
{
    public class Program : Form
    {
        [STAThread]
        static void Main()
        {
            var prog = new Program();
            SystemEvents.SessionEnding += prog.OnExit;
            Application.Run(prog);
        }

        private NotifyIcon _trayIcon;
        private ContextMenu _trayMenu;
        private StatistiksLib _stLib;
        private IReportService _reportService;
        private DateTime _seesionStart;

        public Program()
        {
            _trayMenu = new ContextMenu();
            _trayMenu.MenuItems.Add("Get report for current session", GetReportForCurrentSession);
            _trayMenu.MenuItems.Add("About", About);
            _trayMenu.MenuItems.Add("Quit", OnExit);

            _trayIcon = new NotifyIcon();
            _trayIcon.Text = "Statistiks";
            _trayIcon.Icon = new Icon(Icon.ExtractAssociatedIcon(Application.ExecutablePath), 64, 64);

            _trayIcon.ContextMenu = _trayMenu;
            _trayIcon.Visible = true;

            _seesionStart = DateTime.Now;
            _stLib = new StatistiksLib();
            _reportService = new Report.Report().GetReportService();
            AutoStart();
        }

        private void About(object sender, EventArgs e)
        {
            new AboutForm().Show();
        }

        private void SaveReport(DateTime sessionEnd, string path)
        {
            _reportService.SaveReport(_seesionStart, sessionEnd, path, _stLib.MouseEvents, _stLib.KeyboardEvents, _stLib.WindowEvents);
        }

        private void AutoStart()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (rk.GetValue("Statistiks") != null)
            {
                rk.Close();
                return;
            }
            rk.SetValue("Statistiks", Application.ExecutablePath);
            rk.Close();
        }

        private void GetReportForCurrentSession(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            DateTime timestamp = DateTime.Now;
            string format = _reportService.GetType().Name.Split('R')[0];
            sfd.Filter = string.Format("{0} Files|*.{1}", format, format.ToLower());
            if (sender is string && (string)sender == "OnExit")
            {
                var AppDataPath = string.Format(@"{0}\{1}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Statistiks");
                if (!Directory.Exists(AppDataPath))
                    Directory.CreateDirectory(AppDataPath);
                sfd.FileName = string.Format(@"{0}\{1}-{2}.{3}", AppDataPath, _seesionStart.ToString("yyyyMMddHHmmss"), timestamp.ToString("yyyyMMddHHmmss"), format.ToLower());
                SaveReport(timestamp, sfd.FileName);
                return;
            }
            sfd.FileName = string.Format(@"{0}\{1}-{2}.{3}", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), _seesionStart.ToString("yyyyMMddHHmmss"), timestamp.ToString("yyyyMMddHHmmss"), format.ToLower());
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveReport(timestamp, sfd.FileName);
                MessageBox.Show("Report saved", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
            _stLib.Unhook();
            GetReportForCurrentSession("OnExit", new EventArgs());
            Application.Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _trayIcon.Dispose();
            base.Dispose(disposing);
        }
    }
}
