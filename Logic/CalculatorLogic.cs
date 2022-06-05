using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendX.Test.Driver;
using SendX.Test.Reporting;

namespace SendX.Test.Logic
{
    internal class CalculatorLogic
    {
        private readonly TestDriver _driver;
        public CalculatorLogic(TestDriver driver)
        {
            _driver = driver;
        }
        public string Add(string operand1, string operand2)
        {
            _driver.SetTextByAutomationId("CalculatorResults", operand1);
            _driver.ClickByName("Plus");
            _driver.SetTextByAutomationId("CalculatorResults", operand2);
            _driver.ClickByName("Equals");

            return _driver.GetTextByAutomationId("CalculatorResults");
        }

        public string Subtract(string operand1, string operand2)
        {
            _driver.SetTextByAutomationId("CalculatorResults", operand1);
            _driver.ClickByName("Minus");
            _driver.SetTextByAutomationId("CalculatorResults", operand2);
            _driver.ClickByName("Equals");

            return _driver.GetTextByAutomationId("CalculatorResults");
        }
    }
}
