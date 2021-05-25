using System;
using System.Collections.Generic;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects
{
    class InventoryObject
    {
        private const string InventoryUrl = "https://localhost:5001/Fridge";

        private readonly IWebDriver _webDriver;
        public const int DefaultWaitInSeconds = 10;
        
        private IWebElement IngredSearch => _webDriver.FindElement(By.Id("ingredSearch"));
        private IWebElement IngredSearchConfirm => _webDriver.FindElement(By.Id("ingredSearchConfirm"));
        private IWebElement BeefBrothCannotHave => _webDriver.FindElement(By.Id("banId-6008"));
        private IWebElement BeefBrothDislike => _webDriver.FindElement(By.Id("hideId-6008"));
        private IWebElement UndoButton => _webDriver.FindElement(By.Id("undoFavButton"));


        public InventoryObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }
        public void EnsureInventoryIsOpen()
        {
            if (_webDriver.Url != InventoryUrl)
            {
                _webDriver.Navigate().GoToUrl(InventoryUrl);
            }
        }
        public void SearchForIngredient()
        {
            IngredSearch.SendKeys("beef");
            IngredSearchConfirm.Click();
        }
        public void DislikeIngredientClick()
        {
            BeefBrothDislike.Click();
        }
        public void CannotHaveIngredientClick()
        {
            BeefBrothCannotHave.Click();
        }
        public void UndoButtonClick()
        {
            UndoButton.Click();
        }
        public bool BrothIsBackInSearch()
        {
            return BeefBrothCannotHave != null;
        }
        public void WaitForSearchResult()
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            wait.Until(driver =>
            {
                if (BeefBrothCannotHave == null)
                    return false;

                return true;
            });
        }
        public void WaitForUndoResult()
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            wait.Until(driver =>
            {
                if (UndoButton == null)
                    return false;

                return true;
            });
        }
    }
}
