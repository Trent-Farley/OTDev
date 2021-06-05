using TastyMeals.Tests.BDD.Sprint6.Selenium.Driver;
using TastyMeals.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace TastyMeals.Tests.BDD.Sprint6
{
    [Binding]
    public class CutomAmountOfDaysSteps
    {
        private MealPlanningPage _mealPage;

        public CutomAmountOfDaysSteps(BrowserDriver driver)
        {
            _mealPage = new MealPlanningPage(driver.Current);
        }

        [When(@"a user specifies (.*) worth of days to generate")]
        public void WhenAUserSpecifiesWorthOfDaysToGenerate(int p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            _mealPage.GetMealPlanForAnyDays(p0.ToString());
        }

        [Then(@"a meal plan should be generated with (.*) worth of meals")]
        public void ThenAMealPlanShouldBeGeneratedWithWorthOfMeals(int p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.That(_mealPage.GetTotalDays(), Is.EqualTo(p0));
        }
    }
}