using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnSeleniumTests.Helpers
{
    public class DriverHelper
    {
        private IWebDriver driver;

        public DriverHelper()
        {
        }

        public IWebDriver CreateDriver()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            return driver;
        }
    }
}
