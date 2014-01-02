using BookWorm.Tests.Functional.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Specs
{
    [Binding]
    public class CreateAStaticPageSteps : Steps
    {
        [When(@"I go to Create New Static Page view")]
        public void WhenIGoToCreateNewStaticPageView()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createStaticPage = homePage.NavigateToCreateStaticPageView();
            ScenarioContext.Current.Set(createStaticPage);
        }
        
        [Then(@"I see Create New Static Page view")]
        public void ThenISeeCreateNewStaticPageView()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createStaticPage = ScenarioContext.Current.Get<CreateStaticPage>();
            Assert.IsTrue(createStaticPage.IsCurrentPage());
            homePage.LogOut();
        }

        [Given(@"I am on Create Static Page view")]
        public void GivenIAmOnCreateStaticPageView()
        {
            Given("I am logged in as an admin");
            WhenIGoToCreateNewStaticPageView();
        }

        [When(@"I click create after filling the new page form")]
        public void WhenIClickCreateAfterFillingTheNewPageForm()
        {
            var createStaticPage = ScenarioContext.Current.Get<CreateStaticPage>();
            var createStaticPageDetailsPage = createStaticPage.FillForm("My page title").ClickSaveButton();
            ScenarioContext.Current.Set(createStaticPageDetailsPage);
        }

        [Then(@"I see the details of the newly created page")]
        public void ThenISeeTheDetailsOfTheNewlyCreatedPage()
        {
            var pageDetailsPage = ScenarioContext.Current.Get<StaticPageDetailsPage>();
            var homePage = ScenarioContext.Current.Get<HomePage>();
            Assert.IsTrue(pageDetailsPage.IsCurrentPage("My page title"));
            homePage.LogOut();
        }
    }
}
