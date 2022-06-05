using SendX.Test.Attributes;
using Xunit;
using SendX.Test.Driver;
using System;
using SendX.Test.Reporting;
using System.Reflection;
using SendX.Test.Logic;
using Microsoft.Extensions.Configuration;

namespace SendX.Test
{
    public class CalculatorTest : IDisposable
    {
        private readonly TestDriver _driver;
        private readonly ReportingHelper _report;
        private readonly CalculatorLogic _logic;

        public CalculatorTest()
        {
            var config = new ConfigurationBuilder()
                            .AddJsonFile("appSettings.json")
                            .Build();
            var appPath = config["appSettings:app"];
            var logDirectory = config["appSettings:logDirectory"];

            _report = new ReportingHelper();
            _driver = new TestDriver(_report, logDirectory, appPath);
            _logic = new CalculatorLogic(_driver);
        }

        [Theory]
        [ExcelDataSource(@"Dataset\CalculatorData.xls", "select * from [Sheet1$A1:C4]")]
        public void AddTest(string operand1, string operand2, string output)
        {
            _report.CreateTest(MethodBase.GetCurrentMethod().Name);
            
            var result = _logic.Add(operand1, operand2);
            _report.Log($"Expected {output}, got {result}");
            if (result.Equals($"Display is {output}", StringComparison.OrdinalIgnoreCase))
            {
                _report.Pass();
            }
            else
            {
                _report.Fail();
            }
        }

        //[Theory]
        //[ExcelDataSource(@"Dataset\Calculator.xls", "select * from [Sheet1$A1:C5]")]
        //public void SubtractTest(string x, string y, string z)
        [Fact]
        public void SubtractTest()
        {
            _report.CreateTest(MethodBase.GetCurrentMethod().Name);
            _report.Pass();
        }

        public void Dispose()
        {
            _driver.Close();
            _report.Close();
        }
    }
}