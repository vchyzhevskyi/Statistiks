using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
        }

        private void GetReportForCurrentSession(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnExit(object sender, EventArgs e)
        {
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
