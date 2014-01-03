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
            var homePage = new HomePage();
            homePage.NavigateToCreateStaticPageView();
        }

        [When(@"I click create after filling the new page form")]
        public void WhenIClickCreateAfterFillingTheNewPageForm()
        {
            var createStaticPage = new CreateStaticPagePage();
            createStaticPage.FillForm("My page title").ClickSaveButton();
        }

        [Then(@"I see the details of the newly created page")]
        public void ThenISeeTheDetailsOfTheNewlyCreatedPage()
        {
            var pageDetailsPage = new StaticPageDetailsPage();
            pageDetailsPage.Title.Should().Be("My page title");
        }
    }
}
