using TastyMeals.Tests.BDD.Sprint6.Selenium.Driver;
using TastyMeals.Tests.BDD.Sprint6.Selenium.PageObjects;
using NUnit.Framework;
using System;
using System.Threading;
using TechTalk.SpecFlow;

namespace TastyMeals.Tests.BDD.Sprint6.Steps
{
    [Binding]
    public class FilteredMealPlanningSteps
    {
        private MealPlanningPage _mealPage;

        public FilteredMealPlanningSteps(BrowserDriver driver)
        {
            _mealPage = new MealPlanningPage(driver.Current);
        }

        [Given(@"the meal planner page is active")]
        public void GivenTheMealPlannerPageIsActive()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.EnsureOnMealPlanner();
        }

        [When(@"fat is set to (.*)")]
        public void WhenFatIsSetTo(int p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.AddValueToFilter("fat", p0);
        }

        [When(@"the generate meal plan button is clicked")]
        public void WhenTheGenerateMealPlanButtonIsClicke()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.SubmitWithFilters();
        }

        [When(@"the add filters button is clicked")]
        public void WhenTheAddFiltersButtonIsClicked()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            _mealPage.ClickFilterButton();
        }

        [When(@"only dinner is checked")]
        public void WhenOnlyDinnerIsChecked()
        {
            _mealPage.ClickDinner();
        }

        [When(@"(.*) calories is entered")]
        public void WhenCaloriesIsEntered(string p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.AddValueToFilter("calories", int.Parse(p0));
        }

        [When(@"the very healthy box is checked")]
        public void WhenTheVeryHealthyBoxIsChecked()
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.ClickCheckbox("veryhealthy");
        }

        [Then(@"a customized meal plan should be created with (.*) days worth of meals")]
        public void ThenACustomizedMealPlanShouldBeCreatedWithDaysWorthOfMeals(string p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            var days = _mealPage.GetTotalDays();
            Assert.That(days.ToString(), Is.EqualTo(p0));
        }

        [Then(@"a meal plan with a total less then (.*) calories per day should be generated")]
        public void ThenAMealPlanWithATotalLessThenCaloriesPerDayShouldBeGenerated(int p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));

            _mealPage.GetAverageForWeek("totalCalories");
        }

        [Then(@"a meal plan with only (.*) dinners should be generated")]
        public void ThenAMealPlanWithOnlyDinnersShouldBeGenerated(int p0)
        {
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Assert.That(_mealPage.GetCountByClassName("bg-primary"), Is.EqualTo(0));
            Assert.That(_mealPage.GetCountByClassName("bg-success"), Is.EqualTo(0));
            Assert.That(_mealPage.GetCountByClassName("bg-warning") - 1, Is.EqualTo(7));
        }
    }
}