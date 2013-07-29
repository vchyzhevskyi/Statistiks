using System;
using System.IO;
using System.Xml.Linq;

namespace Statistiks.Configuration
{
    public class Configuration
    {
        private bool _MouseHook;

        public bool MouseCapturingEnabled
        {
            get { return _MouseHook; }
            internal set { _MouseHook = value; }
        }

        private bool _KeyboardHook;

        public bool KeyboardCapturingEnabled
        {
            get { return _KeyboardHook; }
            internal set { _KeyboardHook = value; }
        }

        private bool _Screenshots;

        public bool ScreenshotTakerEnabled
        {
            get { return _Screenshots; }
            internal set { _Screenshots = value; }
        }

        private bool _WindowHook;

        public bool WindowCapturingEnabled
        {
            get { return _WindowHook; }
            internal set { _WindowHook = value; }
        }

        private string _configFilePath;

        private static Configuration _instance;

        public static Configuration Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Configuration();
                }
                return _instance;
            }
        }

        private Configuration()
        {
            _configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Statistiks\\config.xml");
            this.LoadConfiguration(_configFilePath);
        }

        ~Configuration()
        {
            this.SaveConfiguration(_configFilePath);
        }

        private void SaveConfiguration(string configFilePath)
        {
            XDocument xConfig = new XDocument(new XDeclaration("1.0", "", ""),
                                              new XElement("Configuration"));
            xConfig.Root.Add(new XElement("MouseCapturingEnabled", this.MouseCapturingEnabled));
            xConfig.Root.Add(new XElement("WindowCapturingEnabled", this.WindowCapturingEnabled));
            xConfig.Root.Add(new XElement("ScreenshotTakerEnabled", this.ScreenshotTakerEnabled));
            xConfig.Root.Add(new XElement("KeyboardCapturingEnabled", this.KeyboardCapturingEnabled));
            xConfig.Save(configFilePath);
        }

        private void LoadConfiguration(string configFilePath)
        {
            try
            {
                XDocument xConfig = XDocument.Load(configFilePath);
                XElement xConfigRoot = xConfig.Root;
                this.MouseCapturingEnabled = bool.Parse(xConfigRoot.Element("MouseCapturingEnabled").Value);
                this.WindowCapturingEnabled = bool.Parse(xConfigRoot.Element("WindowCapturingEnabled").Value);
                this.ScreenshotTakerEnabled = bool.Parse(xConfigRoot.Element("ScreenshotTakerEnabled").Value);
                this.KeyboardCapturingEnabled = bool.Parse(xConfigRoot.Element("KeyboardCapturingEnabled").Value);
            }
            catch (Exception)
            {
                this.MouseCapturingEnabled = true;
                this.WindowCapturingEnabled = true;
                this.ScreenshotTakerEnabled = false;
                this.KeyboardCapturingEnabled = true;
            }
        }
    }
}
