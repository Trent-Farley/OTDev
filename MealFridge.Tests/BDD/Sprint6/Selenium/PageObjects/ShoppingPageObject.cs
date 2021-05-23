using System;
using System.Collections.Generic;
using System.Text;

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace MealFridge.Tests.BDD.Sprint6.Selenium.PageObjects
{
    class ShoppingPageObject
    {
        private const string ShoppingUrl = "https://localhost:5001/Shopping";

        private readonly IWebDriver _webDriver;
        public const int DefaultWaitInSeconds = 1;

        public ShoppingPageObject(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }
        
        private IWebElement AddObtainEle => _webDriver.FindElement(By.Id("add-obtained"));
        private IWebElement RemoveObtainEle => _webDriver.FindElement(By.Id("remove-obtained"));
        private IReadOnlyCollection<IWebElement> Checkboxes => _webDriver.FindElements(By.ClassName("custom-checkbox"));
        private int ItemAmount = 0;
        public void ClickAdd()
        {
            AddObtainEle.Click();
        }
        public void ClickRemove()
        {
            RemoveObtainEle.Click();
        }

        public bool CheckList()
        {
            return Checkboxes.Count > 0;
        }
        public void ClickAllCheckboxes()
        {
            ItemAmount = Checkboxes.Count;
            var clicker = Checkboxes.GetEnumerator();
            do
            {
                clicker.Current.Click();
            } while (clicker.MoveNext());
        }

        public void ClickFirstTwoCheckboxs()
        {
            var clicker = Checkboxes.GetEnumerator();
            clicker.Current.Click();
            if (clicker.MoveNext())
                clicker.Current.Click();
        }

        public void EnsureShoppingListIsOpen()
        {
            if(_webDriver.Url != ShoppingUrl)
            {
                _webDriver.Navigate().GoToUrl(ShoppingUrl);
            }
        }
        public int WaitForResult()
        {
            var wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(DefaultWaitInSeconds));
            return wait.Until(driver =>
            {
                var result = Checkboxes.Count;
                if (result >= ItemAmount)
                    return default;

                return result;
            });
        }

    }
}
