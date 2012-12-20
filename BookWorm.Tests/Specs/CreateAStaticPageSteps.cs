using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Specs
{
    [Binding]
    public class CreateAStaticPageSteps : BaseSteps
    {
        private CreateStaticPageView createStaticPageView;

        // TODO Extract same method from CreateBookSteps and delete this
        [Given(@"Page:I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            HomePage = HomePage.NavigateTo(Driver);
            HomePage = HomePage.ClickOnLogin().LoginAdmin();
        }

        [When(@"I go to Create New Static Page view")]
        public void WhenIGoToCreateNewStaticPageView()
        {
            createStaticPageView = HomePage.NavigateToCreateStaticPageView();
        }
        
        [Then(@"I see Create New Static Page view")]
        public void ThenISeeCreateNewStaticPageView()
        {
            Assert.IsTrue(createStaticPageView.IsCurrentPage());
            HomePage.LogOut();
        }
    }
}
