using System;
using System.Collections.Generic;
using Statistiks.Lib;

namespace Statistiks.Report
{
    public interface IReportService
    {
        void SaveReport(DateTime sessionStart, DateTime sessionEnd, string path, Dictionary<MouseMessage, double> mouseUsage, Dictionary<string, ulong> keyboardUsage, Dictionary<string, ulong> windowUsage);
    }
}
