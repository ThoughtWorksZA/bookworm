﻿namespace BookWorm.Tests.Functional.Specs.Pages
{
    public class BookDetailsPage :BasePage
    {
        public bool IsCurrentPage(string title)
        {
            return Browser.Title == title;
        }
    }
}
