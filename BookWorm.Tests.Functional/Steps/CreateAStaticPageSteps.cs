using BookWorm.Tests.Functional.Pages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Steps
{
    [Binding]
    public class CreateAStaticPageSteps : TechTalk.SpecFlow.Steps
    {
        [When(@"I go to Create New Static Page view")]
        public void WhenIGoToCreateNewStaticPageView()
        {
            var homePage = ScenarioContext.Current.Get<HomePage>();
            var createStaticPage = homePage.NavigateToCreateStaticPageView();
            ScenarioContext.Current.Set(createStaticPage);
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
            pageDetailsPage.Title.Should().Be("My page title");
        }
    }
}
