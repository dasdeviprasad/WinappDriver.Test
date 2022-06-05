using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;


namespace SendX.Test.Reporting
{
    internal class ReportingHelper
    {
        private static readonly ExtentReports _report;
        private ExtentTest _test;

        static ReportingHelper()
        {
            if (_report == null)
            {
                _report = new ExtentReports();
            }

            var executableDir = Directory.GetParent(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath).FullName;
            var fileName = $"Report_{DateTime.Now.ToString("yyyyMMdd_hhss")}.html";
            var reporter = new ExtentV3HtmlReporter(Path.Combine(executableDir, fileName));
            
            reporter.Config.DocumentTitle = "Regression Testing Report";
            reporter.Config.ReportName = "Regression Testing";
            reporter.Config.Theme = AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;

            _report.AttachReporter(reporter);
            _report.AddSystemInfo("Environment", "QA");
            _report.AddSystemInfo("OS", Environment.OSVersion.VersionString);
        }

        public void CreateTest(string name)
        {
            _test = _report.CreateTest(name);
        }

        public void Log(string message)
        {
            _test.Log(Status.Info, message);
        }

        public void Pass()
        {
            _test.Pass("Test Executed Successfully");
        }

        public void Fail()
        {
            _test.Fail($"<p><strong>Test Failed!!</strong></p>", 
                MediaEntityBuilder.CreateScreenCaptureFromBase64String("base64String").Build());
        }

        public void Close()
        {
            _report.Flush();
        }
    }
}
