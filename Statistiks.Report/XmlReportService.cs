using Statistiks.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Statistiks.Report
{
    public class XmlReportService : IReportService
    {
        public void SaveReport(string path, Dictionary<MouseMessage, ulong> mouseUsage, Dictionary<string, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "", ""),
                new XElement("StatistiksReport",
                    new XElement("KeyboardEvents",
                        from x in keyboardUsage
                        select new XElement("Event",
                            new XAttribute("Key", x.Key),
                            new XAttribute("Count", x.Value))),
                    new XElement("MouseEvents",
                        from x in mouseUsage.Where(x => x.Key == MouseMessage.WM_LBUTTONUP || x.Key == MouseMessage.WM_RBUTTONUP) // only left or right mouse button events
                        select new XElement("Event",
                            new XAttribute("Code", x.Key.ToString()),
                            new XAttribute("Count", x.Value))),
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
