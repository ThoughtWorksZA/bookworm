using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    public class BaseSteps : Steps
    {
        protected IWebDriver Driver;

        [BeforeScenario]
        public void Setup()
        {
            Driver = new FirefoxDriver();
        }

        [AfterScenario]
        public void TearDown()
        {
            Driver.Quit();
        }
    }
}