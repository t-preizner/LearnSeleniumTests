using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;

namespace LearnSeleniumTests
{
    public class Tests
    {
        IWebDriver driver;
        SeleniumHelper helper;
        string url = "http://uitestingplayground.com/";

        [SetUp]
        public void Setup()
        {
            driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl(url);
            driver.Manage().Window.Maximize();

            helper = new SeleniumHelper(driver);
        }

        [Test]
        public void DontUseDynamicIDInXpath()                                                                            // Dynamic ID
        {
            driver.FindElement(By.LinkText("Dynamic ID")).Click();

            Assert.IsTrue(helper.IsElementDisplayed(By.XPath("//button[contains(text(), 'Button with Dynamic ID')]")));
        }

        [Test]
        public void DontUseXpathRelyingOnlyOnAClassForComplicatedClassAttribute()                                        // Class Attribute
        {
            driver.FindElement(By.LinkText("Class Attribute")).Click();

            driver.FindElement(By.XPath("//button[contains(concat(' ', normalize-space(@class), ' '), ' btn-primary ')]")).Click();

            var alertText = driver.SwitchTo().Alert().Text;

            Assert.AreEqual("Primary button pressed", alertText);
        }

        [Test]
        public void CheckDispayedButNotClickableElement()                                                               // Hidden Layers
        {
            driver.FindElement(By.LinkText("Hidden Layers")).Click();

            var greenButton = By.Id("greenButton");
            driver.FindElement(greenButton).Click();

            Assert.IsTrue(driver.FindElement(greenButton).Enabled);
            Assert.IsTrue(driver.FindElement(greenButton).Displayed);
            Assert.AreEqual("Element is not clickable!", helper.IsElementClickable(greenButton));
        }

        [Test]
        public void CheckLoadingOfElementFromServerSideUsingImplicitWait()                                               // Load Delay
        {
            driver.FindElement(By.LinkText("Load Delay")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            var buttonText = driver.FindElement(By.XPath("//button[@class='btn btn-primary']")).Text;

            Assert.AreEqual("Button Appearing After Delay", buttonText);
        }

        [Test]
        public void CheckLoadingOfElementFromServerSideUsingExplicitWait()                                               // AJAX Data
        {
            driver.FindElement(By.LinkText("AJAX Data")).Click();

            driver.FindElement(By.Id("ajaxButton")).Click();

            var expectedText = "Data loaded with AJAX get request.";
            
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 20));
            var actual = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"//p[contains(text(), '{expectedText}')]")));
            var actualText = actual.Text;

            Assert.AreEqual(expectedText, actualText);
        }


        [Test]
        public void CheckLoadingOfElementFromClientSideUsingExplicitWait()                                               // Client Side Delay
        {
            driver.FindElement(By.LinkText("Client Side Delay")).Click();

            driver.FindElement(By.XPath("//button[@id='ajaxButton']")).Click();

            var expectedText = "Data calculated on the client side.";

            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 15));
            var actual = wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath($"//p[contains(text(), '{expectedText}')]")));
            var actualText = actual.Text;

            Assert.AreEqual(expectedText, actualText);
        }

        [Test]
        public void CheckEmulatingOfMouseClick()                                                                          // Click
        {
            driver.FindElement(By.LinkText("Click")).Click();

            var button = driver.FindElement(By.CssSelector(".btn.btn-primary"));                            
            button.Click();

            var actual = button.GetAttribute("class");
            var expected = "btn btn-success";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckEmulatingOfTextInputFromKeyboard()                                                               // Text Input
        {
            driver.FindElement(By.LinkText("Text Input")).Click();

            string expectedText = "test text";

            driver.FindElement(By.Id("newButtonName")).SendKeys(expectedText);
            var button = driver.FindElement(By.Id("updatingButton"));
            button.Click();

            var actualText = button.Text;

            Assert.AreEqual(expectedText, actualText);
        }

        [Test]
        public void Scrollbars()                            // розібратися зі скроллами
        {
            driver.FindElement(By.LinkText("Scrollbars")).Click();

            var block = driver.FindElement(By.XPath("/html/body/section/div/div"));                 // замінити XPath

            block.Click();

            Actions actions = new Actions(driver);
            actions.SendKeys(Keys.PageDown).Build().Perform();                  // розібратися зі скроллом елементів!!
            actions.SendKeys(Keys.ArrowRight).Build().Perform();

            /*
            IJavaScriptExecutor js = driver as IJavaScriptExecutor;
            //System.Threading.Thread.Sleep(5000);

            js.ExecuteScript("document.getElementById('').scrollBy(0,300);");
            */

            var button = driver.FindElement(By.XPath("//button[@id='hidingButton']"));

            Assert.That(button.Displayed, Is.True);
        }

        [Test]
        public void CheckCellValueInDynamicTable()                                                                         // Dynamic Table
        {
            driver.FindElement(By.LinkText("Dynamic Table")).Click();

            var columnHeaders = driver.FindElements(By.XPath("//div/span[@role='columnheader']"));
            int columnNumber = 0;

            columnNumber = helper.GetNumberOfElement(columnHeaders, "CPU", columnNumber);

            var rowHeaders = driver.FindElements(By.XPath("//div[@role='row']"));
            int rowNumber = 0;

            rowNumber = helper.GetNumberOfElement(rowHeaders, "Chrome", rowNumber);

            var actual = driver.FindElements(By.XPath($"//div[@role='rowgroup'][2]/div[@role='row'][{rowNumber}]/span[{columnNumber + 1}]"))[0].Text;
            var expected = driver.FindElement(By.ClassName("bg-warning")).Text.Split(" ").GetValue(2);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CheckTexUsingNormalizeSpace()                                                                          // Verify Text
        {
            driver.FindElement(By.LinkText("Verify Text")).Click();

            var actual = driver.FindElement(By.XPath("//span[normalize-space(.)='Welcome UserName!']"));
            var actualText = actual.Text;

            Assert.That(actual.Displayed, Is.True);
            Assert.AreEqual("Welcome UserName!", actualText);
        }

        [Test]
        public void ProgressBar()                           // тест нестабільний
        {
            driver.FindElement(By.LinkText("Progress Bar")).Click();

            driver.FindElement(By.XPath("//button[@id='startButton']")).Click();

            var progressBar = driver.FindElement(By.XPath($"//div[@id='progressBar']"));

            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.TextToBePresentInElement(progressBar, "75%"));

            driver.FindElement(By.XPath("//button[@id='stopButton']")).Click();

            var resultString = driver.FindElement(By.XPath("//p[@id='result']")).Text.Split(",");
            var resultValue = resultString[0].Split(" ");

            Assert.AreEqual("0", resultValue[1]);
        }

        [Test]
        public void CheckDifferentCasesOfElementsVisibilityOnWebPages()                                                    // Visibility
        {
            driver.FindElement(By.LinkText("Visibility")).Click();

            driver.FindElement(By.Id("hideButton")).Click();

            Assert.IsFalse(helper.IsElementDisplayed(By.Id("removedButton")));
            Assert.IsFalse(helper.IsElementDisplayed(By.Id("zeroWidthButton")));
            Assert.IsTrue(helper.IsElementDisplayed(By.Id("overlappedButton")));
            Assert.IsFalse(helper.IsElementDisplayed(By.Id("transparentButton")));
            Assert.IsFalse(helper.IsElementDisplayed(By.Id("invisibleButton")));
            Assert.IsFalse(helper.IsElementDisplayed(By.Id("notdisplayedButton")));
            Assert.IsFalse(helper.IsElementDisplayed(By.Id("offscreenButton")));
        }

        [Test]
        public void SampleApp()                            // ok
        {
            driver.FindElement(By.LinkText("Sample App")).Click();

            string login = "test";
            string password = "pwd";

            driver.FindElement(By.XPath("//input[@type='text']")).SendKeys(login);
            driver.FindElement(By.XPath("//input[@type='password']")).SendKeys(password);
            driver.FindElement(By.XPath("//button[@class='btn btn-primary']")).Click();

            var actual = driver.FindElement(By.Id("loginstatus")).Text;

            Assert.AreEqual($"Welcome, {login}!", actual);
        }

        [Test]
        public void MouseOver()                            // ok
        {
            driver.FindElement(By.LinkText("Mouse Over")).Click();

            var link = driver.FindElement(By.ClassName("text-primary"));

            Actions actions = new Actions(driver);
            actions.MoveToElement(link).Build().Perform();
            
            driver.FindElement(By.ClassName("text-warning")).Click();   // розібратися
            driver.FindElement(By.ClassName("text-warning")).Click();
            
            var actual = driver.FindElement(By.Id("clickCount")).Text;

            Assert.AreEqual("2", actual);
        }

        [Test]
        public void CheckNonBreakingSpaceInString()                                                                         // Non-Breaking Space
        {
            driver.FindElement(By.LinkText("Non-Breaking Space")).Click();

            var nbsp = "\u00a0";

            Assert.IsTrue(helper.IsElementDisplayed(By.XPath($"//button[text()='My{nbsp}Button']")), "Oops!");
        }

        [Test]
        public void OverlappedElement()                    // to do
        {
            driver.FindElement(By.LinkText("Overlapped Element")).Click();

            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}