﻿using BookWorm.Tests.Functional.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TechTalk.SpecFlow;

namespace BookWorm.Tests.Functional.Steps
{
    [Binding]
    internal class CreateBookSteps : TechTalk.SpecFlow.Steps
    {
        [Given(@"I am logged in as an admin")]
        public void IAmLoggedInAsAnAdmin()
        {
            Given("I navigate to the login page");
            When("I enter my credentials");
        }

        [When(@"I go to Create New Book page")]
        public void IGoToCreateNewBookPage()
        {
            var homePage = new HomePage();
            var createBookPage = homePage.NavigateToCreateBookPage();
            ScenarioContext.Current.Set(createBookPage);
        }

        [When(@"I click create after filling the form")]
        public void IClickSaveAfterFillingTheForm()
        {
            var createBookPage = ScenarioContext.Current.Get<CreateBookPage>();
            var bookDetailsPage = createBookPage.FillForm("My new title").ClickSaveButton();
            ScenarioContext.Current.Set(bookDetailsPage);
        }

        [Then(@"I see the details of the newly created book")]
        public void ISeeTheDetailsOfTheNewlyCreatedBook()
        {
            var bookDetailsPage = ScenarioContext.Current.Get<BookDetailsPage>();
            Assert.IsTrue(bookDetailsPage.IsCurrentPage("My new title"));
        }
    }
}
