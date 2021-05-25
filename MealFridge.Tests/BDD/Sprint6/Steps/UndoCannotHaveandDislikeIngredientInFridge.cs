using MealFridge.Tests.BDD.Sprint6.Selenium.Driver;
using MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;
namespace MealFridge.Tests.BDD.Sprint6.Steps
{
    [Binding]
    class UndoCannotHaveandDislikeIngredientInFridge
    {
        private readonly InventoryObject _page;
        public UndoCannotHaveandDislikeIngredientInFridge(BrowserDriver browserDriver)
        {
            _page = new InventoryObject(browserDriver.Current);
        }

        [Given(@"A user is on the inventory page")]
        public void GivenAUserIsOnTheRecipeSearchPage()
        {
            _page.EnsureInventoryIsOpen();
        }
        [When(@"They search for an ingredient")]
        public void GivenTheySearchForAnIngredient()
        {
            _page.SearchForIngredient();
        }
        [When(@"they dislike an ingredient")]
        public void TheyDislikeAnIngredient()
        {
            _page.WaitForSearchResult();
            _page.DislikeIngredientClick();
        }
        [When(@"they cannot have an ingredient")]
        public void WhenTheyCannotHaveAnIngredient()
        {
            _page.WaitForSearchResult();
            _page.CannotHaveIngredientClick();
        }
        [When(@"they undo it")]
        public void WhenTheyUndoIt()
        {
            _page.WaitForUndoResult();
            _page.UndoButtonClick();
        }
        [When(@"they retry")]
        public void Retry()
        {
            _page.RefreshPage();
        }
        [Then(@"the ingredient should reappear in search")]
        public void TheIngredientShouldReappearInSearch()
        {
            _page.WaitForSearchResult();
            Assert.That(_page.BrothIsBackInSearch);
        }
        [Then(@"the ingredient should not reappear in search")]
        public void ShouldNotAppear()
        {
            _page.WaitForSearchResult();
            Assert.That(_page.BrothIsGoneFromSearch);
        }
    }
}
