using MealFridge.Tests.BDD.Sprint6.Selenium.Driver;
using MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using System;
using TechTalk.SpecFlow;

namespace MealFridge.Tests.BDD.Sprint6.Steps
{
    [Binding]
    public class SearchForRecipesByCuisineSteps
    {
        private readonly SearchPageObject _page;

        public SearchForRecipesByCuisineSteps(BrowserDriver browserDriver)
        {
            _page = new SearchPageObject(browserDriver.Current);
        }

        [Given(@"A user is on the recipe search page")]
        public void GivenAUserIsOnTheRecipeSearchPage()
        {
            _page.EnsureSearchIsOpen();
        }
        
        [When(@"they select a single cuisine")]
        public void WhenTheySelectASingleCuisine()
        {
            _page.AddCuisine("American");
        }
        
        [When(@"They select a cuisine as an exclusion")]
        public void WhenTheySelectACuisineAsAnExclusion()
        {
            _page.ExcludeCuisine("American");
        }
        
        [Then(@"recipes of that type should be the only result")]
        public void ThenRecipesOfThatTypeShouldBeTheOnlyResult()
        {
            _page.CuisineIncludes("American");
        }
        
        [Then(@"recipes of that type should not show\.")]
        public void ThenRecipesOfThatTypeShouldNotShow_()
        {
            Assert.IsTrue(_page.CuisineExcludes("American"));
        }
    }
}
