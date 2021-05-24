using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects
{
    class SearchPageObject
    {
        private const string Url = "https://localhost:5001/Search";
        private readonly IWebDriver _webDriver;

        private IWebElement SearchInput => _webDriver.FindElement(By.Id("sbn"));
        private IWebElement RecipeInfo => _webDriver.FindElement(By.ClassName("btn-outline-primary"));
        private IWebElement CartButton => _webDriver.FindElement(By.Id("button-cart"));
        private IWebElement CuisineString => _webDriver.FindElement(By.Id("cuisineList"));
        private IWebElement CuisineButton;
        public SearchPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }
        public void EnsureSearchIsOpen()
        {
            if (_webDriver.Url != Url)
            {
                _webDriver.Navigate().GoToUrl(Url);
            }
        }

        internal void AddCuisine(string v)
        {
            CuisineButton = _webDriver.FindElement(By.Id(v));
            CuisineButton.Click();
        }

        internal void ExcludeCuisine(string v)
        {
            CuisineButton = _webDriver.FindElement(By.Id(v));
            CuisineButton.Click();
            CuisineButton.Click();
        }

        public void AddRecipeToShoppingList()
        {
            EnsureSearchIsOpen();
            SearchInput.SendKeys("Steak" + Keys.Enter);
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
            wait.Until(driver =>
            {
                return RecipeInfo.Displayed;
            });
            RecipeInfo.Click();
            wait.Until(driver =>
            {
                return CartButton.Displayed;
            });
            CartButton.Click();
        }

        internal bool CuisineExcludes(string v)
        {
            SearchInput.SendKeys("Fish" + Keys.Enter);
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
            wait.Until(driver =>
            {
                return RecipeInfo.Displayed;
            });
            RecipeInfo.Click();
            wait.Until(driver =>
            {
                return CuisineString.Displayed;
            });
            return !CuisineString.Text.Contains(v);
        }

        internal bool CuisineIncludes(string v)
        {
            SearchInput.SendKeys("Fish" + Keys.Enter);
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5));
            wait.Until(driver =>
            {
                return RecipeInfo.Displayed;
            });
            RecipeInfo.Click();
            wait.Until(driver =>
            {
                return CuisineString.Displayed;
            });
            return CuisineString.Text.Contains(v);
        }
    }
}
