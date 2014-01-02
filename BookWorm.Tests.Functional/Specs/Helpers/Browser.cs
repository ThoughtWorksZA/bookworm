using OpenQA.Selenium;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Specs.Helpers
{
    public static class Browser
    {
        public static IWebDriver Driver
        {
            get
            {
                return ScenarioContext.Current.Get<IWebDriver>();
            }
            set
            {
                ScenarioContext.Current.Set(value);
            }
        }
    }
}
