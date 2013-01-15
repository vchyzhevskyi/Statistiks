using Statistiks.Lib;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Statistiks.Report
{
    public class XmlReportService : IReportService
    {
        public void SaveReport(string path, Dictionary<MouseMessage, ulong> mouseUsage, Dictionary<int, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage)
        {
            XDocument xDoc = new XDocument(new XDeclaration("1.0", "", ""),
                new XElement("StatistiksReport",
                    new XElement("Keyboard",
                        from x in keyboardUsage
                        select new XElement(x.Key.ToString(),
                            new XAttribute("count", x.Value))),
                    new XElement("Mouse",
                        from x in mouseUsage
                        select new XElement(x.Key.ToString(),
                            new XAttribute("count", x.Value))),
                    new XElement("Window",
                        from x in windowUsage
                        select new XElement(x.Key,
                            new XAttribute("count", x.Value)))));
            xDoc.Save(path);
        }
    }
}
