using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    public class CreateAStaticPageSteps : BaseSteps
    {
        [When(@"I go to Create New Static Page view")]
        public void WhenIGoToCreateNewStaticPageView()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createStaticPageView = homePage.NavigateToCreateStaticPageView();
            ScenarioContext.Current.Set(createStaticPageView);
        }
        
        [Then(@"I see Create New Static Page view")]
        public void ThenISeeCreateNewStaticPageView()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createStaticPageView = ScenarioContext.Current.Get<CreateStaticPageView>();
            Assert.IsTrue(createStaticPageView.IsCurrentPage());
            homePage.LogOut();
        }
    }
}
