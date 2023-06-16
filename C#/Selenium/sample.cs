using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NUnitSeleniumTest
{
    public class Tests
    {
        // Browser driver
        IWebDriver webDriver = new ChromeDriver();

        // Find Login button by text
        IWebElement lnkLogin => webDriver.FindElement(By.LinkText("Login"));

        // UserName
        IWebElement txtUserName => webDriver.FindElement(By.Name("UserName"));

        /// <summary>
        /// before test run
        /// </summary>
        [SetUp]
        public void Setup()
        {
            // Navigate to site
            webDriver.Navigate().GoToUrl("http://eaapp.somee.com/");
        }


        /// <summary>
        /// test
        /// </summary>
        [Test]
        public void TestLogin()
        {
            // click on login button
            lnkLogin.Click();

            // check if username exist
            Assert.That(txtUserName.Displayed, Is.True);

            // fill UserName text box
            txtUserName.SendKeys("admin");
        }

        /// <summary>
        /// after tests
        /// </summary>
        [TearDown]
        public void TearDown() => webDriver.Close();
    }
}