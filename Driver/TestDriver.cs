using System;
using System.IO;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Service;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Support.UI;
using SendX.Test.Reporting;

namespace SendX.Test.Driver
{
    internal class TestDriver
    {
        private readonly WindowsDriver<WindowsElement> _driver;
        private readonly AppiumLocalService _service;
        private readonly ReportingHelper _report;
        public TestDriver(ReportingHelper report, string logFileDir, string appPath)
        {
            _report = report;

            var logFile = new FileInfo(Path.Combine(logFileDir, $"{Guid.NewGuid()}.txt"));
            _service = new AppiumServiceBuilder()
               .UsingPort(4723)
               .WithLogFile(logFile)
               .Build();
            _service.Start();


            var option = new AppiumOptions();
            option.AddAdditionalCapability("app", appPath);
            //@"C:\Windows\System32\Notepad.exe"); // "Microsoft.ZuneVideo_8wekyb3d8bbwe!");

            _driver = new WindowsDriver<WindowsElement>(_service, option);
        }

        public WindowsElement FindElementById(string id)
        {
            return _driver.FindElementById(id);
        }

        public WindowsElement FindElementByName(string name)
        {
            _report.Log($"Find element by name: {name}");
            return _driver.FindElementByName(name);
        }

        public void Click(WindowsElement element)
        {
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 2, 0));
            wait.Until(x => element.Displayed);

            _report.Log($"Clicked element: {element.GetAttribute("Name")}");
            element.Click();
        }

        public void ClickByName(string name)
        {
            _report.Log($"Finding element by name: {name}");
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 2, 0));
            var element = wait.Until(x => ((WindowsDriver<WindowsElement>)x).FindElementByName(name));

            _report.Log($"Clicked element: {name}");
            element.Click();
        }

        public string GetTextByAutomationId(string automationId)
        {
            _report.Log($"Finding element by automation id: {automationId}");
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 2, 0));
            var element = wait.Until(x => ((WindowsDriver<WindowsElement>)x).FindElementByAccessibilityId(automationId));

            _report.Log($"Got the text to be: {element?.Text}");
            return element?.Text;
        }

        public void SetTextByAutomationId(string automationId, string text)
        {
            _report.Log($"Finding element by automation id: {automationId}");
            var wait = new WebDriverWait(_driver, new TimeSpan(0, 2, 0));
            var element = wait.Until(x => ((WindowsDriver<WindowsElement>)x).FindElementByAccessibilityId(automationId));

            _report.Log($"Set the text to: {text}");
            element?.SendKeys(text);
        }

        public void Close()
        {
            _driver.Close();
            _service.Dispose();
        }
    }
}
