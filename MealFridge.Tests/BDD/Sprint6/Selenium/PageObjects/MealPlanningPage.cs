using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TastyMeals.Tests.BDD.Sprint6.Selenium.PageObjects
{
    public class MealPlanningPage
    {
        private const string MealPlanningUrl = "https://localhost:5001/MealPlan";

        private readonly IWebDriver _webDriver;

        public IWebElement FindById(string id)
        {
            return _webDriver.FindElement(By.Id(id));
        }

        public MealPlanningPage(IWebDriver driver)
        {
            _webDriver = driver;
        }

        public int GetAverageForWeek(string id)
        {
            var all = _webDriver.FindElements(By.Id(id));
            var total = 0;
            foreach (var ele in all)
                total += int.Parse(ele.Text);
            total /= 7;
            return (int)Math.Floor((double)total);
        }

        public int GetTotalDays()
        {
            return _webDriver.FindElements(By.Id("meals")).Count - 1; // 1 For the row with the name of the meal
        }

        public int GetMealPlanForAnyDays(string days)
        {
            var daysButton = FindById("days");
            var select = new SelectElement(daysButton);
            select.SelectByText(days);
            var submit = FindById("filteredSubmit");

            submit.Click();
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(2)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

            return _webDriver.FindElements(By.ClassName("row")).Count;
        }

        public void ClickCheckbox(string id)
        {
            FindById(id).Click();
        }

        public void AddValueToFilter(string id, int amount)
        {
            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(50)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            var element = FindById(id);
            element.Clear();
            element.SendKeys(amount.ToString());
        }

        public void UnCheckMeals()
        {
            var b = FindById("b");
            var l = FindById("l");
            var d = FindById("d");
            b.Click();
            l.Click();
            d.Click();
        }

        public void ClickDinner()
        {
            UnCheckMeals();
            var d = FindById("d");
            d.Click();
        }

        public void ClickLucnh()
        {
            UnCheckMeals();
            var l = FindById("l");
            l.Click();
        }

        public void ClickBreakfast()
        {
            var b = FindById("b");
            b.Click();
        }

        public int GetCountByClassName(string clsName)
        {
            return _webDriver.FindElements(By.ClassName(clsName)).Count;
        }

        public void SubmitWithFilters()
        {
            var submit = FindById("filteredSubmit");

            submit.Click();
        }

        public void ClickFilterButton()
        {
            var filterOpen = FindById("filter-button");
            filterOpen.Click();
        }

        public void EnsureOnMealPlanner()
        {
            var login = new LoginPageObject(_webDriver);
            login.Login();
            _webDriver.Navigate().GoToUrl(MealPlanningUrl);

            new WebDriverWait(_webDriver, TimeSpan.FromSeconds(5)).Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
        }
    }
}