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
        private const string HomeUrl = "https://localhost:5001";

        private readonly IWebDriver _webDriver;
        public const int DefaultWaitInSeconds = 10;
        
        private IWebElement IngredSearch => _webDriver.FindElement(By.Id("ingredSearch"));
        private IWebElement IngredSearchConfirm => _webDriver.FindElement(By.Id("ingredSearchConfirm"));
        private IWebElement CannotHaveBtn => _webDriver.FindElement(By.Id("banIngredBtn"));
        private IWebElement DisklikeBtn => _webDriver.FindElement(By.Id("hideIngredBtn"));
        private IWebElement UndoButton => _webDriver.FindElement(By.Id("undoFavButton"));
        private IWebElement FirstCardTitlte => _webDriver.FindElement(By.ClassName("card-title"));
        private int CardCount => _webDriver.FindElements(By.ClassName("card-title")).Count;



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
        public void RefreshPage()
        {
            _webDriver.Navigate().GoToUrl(HomeUrl);
            _webDriver.Navigate().GoToUrl(InventoryUrl);
        }
        public void SearchForIngredient()
        {
            IngredSearch.SendKeys("beef" + Keys.Enter);
        }
        public void DislikeIngredientClick()
        {
            WaitForSearchResult();
            var temp = _webDriver.FindElement(By.Id("hideIngredBtn"));
            temp.Click();
        }
        public void CannotHaveIngredientClick()
        {
            WaitForSearchResult();
            var temp = _webDriver.FindElement(By.Id("banIngredBtn"));
            temp.Click();
        }
        public void UndoButtonClick()
        {
            WaitForSearchResult();
            UndoButton.Click();
        }
        public bool BrothIsBackInSearch()
        {
            WaitForSearchResult();
            return CardCount  >= 16;
        }
        public bool BrothIsGoneFromSearch()
        {
            WaitForSearchResult();
            return CardCount < 17;
        }
        public void WaitForSearchResult()
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            wait.Until(driver =>
            {
                if (CannotHaveBtn == null)
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
