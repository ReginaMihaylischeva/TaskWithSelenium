using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using Xunit;

namespace Selenium
{
    public class SuiteTests : IDisposable
    {
        public IWebDriver driver { get; private set; }
        public IDictionary<String, Object> vars { get; private set; }
        public IJavaScriptExecutor js { get; private set; }
        public SuiteTests()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<String, Object>();
        }
        public void Dispose()
        {
            driver.Quit();
        }
        [Fact]
        public void First()
        {
            driver.Navigate().GoToUrl("https://gemsdev.ru/");
            driver.Manage().Window.Size = new System.Drawing.Size(1056, 664);
            driver.FindElement(By.LinkText("Продукты")).Click();
            {
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".other-products > .container"));
                Assert.True(elements.Count > 0);
            }
            {
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".urban-analytics"));
                Assert.True(elements.Count > 0);
            }
            {
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.CssSelector(".gos-system"));
                Assert.True(elements.Count > 0);
            }
            {
                IReadOnlyCollection<IWebElement> elements = driver.FindElements(By.XPath("//section"));
                Assert.True(elements.Count > 0);
            }
        }
        [Fact]
        public void Second()
        {
            driver.Navigate().GoToUrl("https://gemsdev.ru/");
            driver.Manage().Window.Size = new System.Drawing.Size(1056, 664);
            driver.FindElement(By.LinkText("Продукты")).Click();
            {
                var element = driver.FindElement(By.XPath("//a[contains(.,\'Перейти на сайт продукта\')]"));
                string attribute = element.GetAttribute("href");
                vars["x"] = attribute;
            }
            Assert.Equal("https://xn--c1aaceme9acfqh.xn--p1ai/", vars["x"].ToString());
        }
    }
}
