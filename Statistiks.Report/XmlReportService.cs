﻿using Statistiks.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Statistiks.Report
{
    public class XmlReportService : IReportService
    {
        public void SaveReport(DateTime sessionStart, DateTime sessionEnd, string path, Dictionary<MouseMessage, double> mouseUsage, Dictionary<string, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "", ""),
                new XElement("Report",
                    new XAttribute("SessionStart", sessionStart.ToString("yyyyMMddHHmmss")),
                    new XAttribute("SessionEnd", sessionEnd.ToString("yyyyMMddHHmmss")),
                    new XElement("KeyboardEvents",
                        from x in keyboardUsage.Where(x => ((int)x.Key[0]).IsValidXmlChar())
                        select new XElement("Event",
                            new XAttribute("Key", x.Key),
                            new XAttribute("Count", x.Value))),
                    new XElement("MouseEvents",
                        from x in mouseUsage.Where(x => x.Key == MouseMessage.WM_LBUTTONUP || x.Key == MouseMessage.WM_RBUTTONUP || x.Key == MouseMessage.WM_MOUSEMOVE) // only left or right mouse button events or path length of mouse move in cm
                        select new XElement("Event",
                            new XAttribute("Code", x.Key.ToString()),
                            new XAttribute("Count", x.Key == MouseMessage.WM_MOUSEMOVE ? Math.Round(x.Value, 2) : (ulong)x.Value))),
                    new XElement("WindowsEvents",
                        from x in windowUsage
                        select new XElement("Event",
                            new XAttribute("ExeName", x.Key.Split('\\').Last()),
                            new XAttribute("ExePath", x.Key),
                            new XAttribute("Count", x.Value)))));
            xDoc.Save(path);
        }
    }
}
