using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Statistiks.Lib;
using Statistiks.Report;
using Microsoft.Win32;

namespace Statistiks.App
{
    public class Program : Form
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new Program());
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

        private void SaveReport(string path)
        {
            _reportService.SaveReport(path, _stLib.MouseEvents, _stLib.KeyboardEvents, _stLib.WindowEvents);
        }

        private void AutoStart()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
            if (rk.GetValue("Statistiks") != null)
                return;
            rk.SetValue("Statistiks", Application.ExecutablePath);
            rk.Close();
        }

        private void GetReportForCurrentSession(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            DateTime timestamp = DateTime.Now;
            string format = _reportService.GetType().Name.Split('R')[0];
            sfd.Filter = string.Format("{0} Files|*.{1}", format, format.ToLower());
            sfd.FileName = string.Format(@"{12}\{0}{1}{2}{3}{4}{5}-{6}{7}{8}{9}{10}{11}.{13}", _seesionStart.Year, _seesionStart.Month, _seesionStart.Day, _seesionStart.Hour, _seesionStart.Minute, _seesionStart.Second, timestamp.Year, timestamp.Month, timestamp.Day, timestamp.Hour, timestamp.Minute, timestamp.Second, Environment.GetFolderPath(Environment.SpecialFolder.Desktop), format.ToLower());
            if (sender is string && (string)sender == "OnExit")
                SaveReport(sfd.FileName);
            else if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SaveReport(sfd.FileName);
                MessageBox.Show("Report saved", "Report", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
            }
        }

        private void OnExit(object sender, EventArgs e)
        {
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
