using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Config = Statistiks.Configuration.Configuration;
using Statistiks.Lib;

namespace Statistiks.Report
{
    public class XmlReportService : IReportService
    {
        public void SaveReport(DateTime sessionStart, DateTime sessionEnd, string path, Dictionary<MouseMessage, double> mouseUsage, Dictionary<string, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "", ""),
                new XElement("Report",
                    new XAttribute("SessionStart", sessionStart.ToString("yyyyMMddHHmmss")),
                    new XAttribute("SessionEnd", sessionEnd.ToString("yyyyMMddHHmmss"))));
            if (Config.Instance.KeyboardCapturingEnabled)
                xDoc.Root.Add(new XElement("KeyboardEvents",
                    keyboardUsage.Where(x => ((int)x.Key[0]).IsValidXmlChar())
                        .Select(x => new XElement("Event",
                                            new XAttribute("Key", x.Key),
                                            new XAttribute("Count", x.Value)))));
            if (Config.Instance.MouseCapturingEnabled)
                xDoc.Root.Add(new XElement("MouseEvents",
                    mouseUsage.Where(x => x.Key == MouseMessage.WM_LBUTTONUP || x.Key == MouseMessage.WM_RBUTTONUP || x.Key == MouseMessage.WM_MOUSEMOVE)
                        .Select(x => new XElement("Event",
                                            new XAttribute("Code", x.Key.ToString()),
                                            new XAttribute("Count", x.Key == MouseMessage.WM_MOUSEMOVE ? Math.Round(x.Value, 2) : (ulong)x.Value)))));
            if (Config.Instance.WindowCapturingEnabled)
                xDoc.Root.Add(new XElement("WindowsEvents",
                    windowUsage.Select(x => new XElement("Event",
                                                    new XAttribute("ExeName", x.Key.Split('\\').Last()),
                                                    new XAttribute("ExePath", x.Key), new XAttribute("Count", x.Value)))));

            xDoc.Save(path);
        }
    }
}
