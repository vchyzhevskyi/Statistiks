using Statistiks.Lib;
using System.Collections.Generic;

namespace Statistiks.Report
{
    public interface IReportService
    {
        void SaveReport(string path, Dictionary<MouseMessage, ulong> mouseUsage, Dictionary<string, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage);
    }
}
