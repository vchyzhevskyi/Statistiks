using Ninject;
using Ninject.Parameters;

namespace Statistiks.Report
{
    public class Report
    {
        IKernel _diKernel;

        public Report()
        {
            _diKernel = new StandardKernel();
            _diKernel.Bind<IReportService>().To<XmlReportService>();
        }

        public IReportService GetReportService()
        {
            return _diKernel.Get<IReportService>(new IParameter[0]);
        }
    }
}
