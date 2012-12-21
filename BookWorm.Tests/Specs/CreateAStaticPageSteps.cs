using System;
using BookWorm.Tests.Specs.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
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

        [Given(@"I am on Create Static Page view")]
        public void GivenIAmOnCreateStaticPageView()
        {
            Given("I am logged in as an admin");
            WhenIGoToCreateNewStaticPageView();
        }

        [When(@"I click create after filling the new page form")]
        public void WhenIClickCreateAfterFillingTheNewPageForm()
        {
            var createStaticPageView = ScenarioContext.Current.Get<CreateStaticPageView>();
            var createStaticPageDetailsPage = createStaticPageView.FillForm("My page title").ClickSaveButton();
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
