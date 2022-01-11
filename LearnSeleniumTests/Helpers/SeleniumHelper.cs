using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace LearnSeleniumTests
{
    public class SeleniumHelper
    {
        public IWebDriver _driver;
        
        public SeleniumHelper(IWebDriver driver)
        {
            _driver = driver;
        }

        public bool IsElementDisplayed(By by)
        {
            try
            {
                var el = _driver.FindElement(by);
                return el.Displayed;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public string IsElementClickable(By by)
        {
            try
            {
                _driver.FindElement(by).Click();
                return "Element is clickable!";
            }
            catch (Exception e)
            {
                if (e.Message.Contains("is not clickable at point"))
                {
                    return "Element is not clickable!";
                }
                else
                {
                    return "Look at the exeption!";
                }
            }
        }

        public int GetNumberOfElement(ReadOnlyCollection<IWebElement> collectionOfWebElements, string stringForSearch, int numberOfElement)
        {
            for (int i = 0; i < collectionOfWebElements.Count; i++)
            {
                if (collectionOfWebElements[i].Text.StartsWith(stringForSearch))
                {
                    numberOfElement = i;
                    break;
                }
            }
            return numberOfElement;
        }
    }
}
