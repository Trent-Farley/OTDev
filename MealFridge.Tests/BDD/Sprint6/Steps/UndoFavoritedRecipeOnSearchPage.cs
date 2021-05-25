using MealFridge.Tests.BDD.Sprint6.Selenium.Driver;
using MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace MealFridge.Tests.BDD.Sprint6.Steps
{
    [Binding]
    class UndoFavoritedRecipeOnSearchPage
    {
        private readonly SearchPageObject _page;

        public UndoFavoritedRecipeOnSearchPage(BrowserDriver browserDriver)
        {
            _page = new SearchPageObject(browserDriver.Current);
        }
        [When(@"they search for a recipe")]
        public void WhenTheySearchForARecipe()
        {
            _page.SearchForRecipe();
        }
        [When(@"they favorite a recipe")]
        public void WhenTheyFavoriteARecipe()
        {
            _page.FavClick();
        }
        [When(@"they undo the favorite")]
        public void WhenTheyUndoTheFavorite()
        {
            _page.UndoFavClick();
        }
        [Then (@"the undo should be successful")]
        public void TheUndoShouldBeSuccessful()
        {
            _page.undoSuccess();
        }
    }
}
