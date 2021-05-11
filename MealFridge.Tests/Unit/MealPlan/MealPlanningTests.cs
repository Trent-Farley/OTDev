using MealFridge.Controllers;
using MealFridge.Models;
using MealFridge.Models.ViewModels;
using MealFridge.Tests.Utils;
using MealFridge.Utils;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Tests.MealPlan
{
    /// <summary>
    /// Tester: Trent Farley
    /// Story Id: #177689339 and #177689438
    /// </summary>
    [TestFixture]
    public class MealPlanningTests
    {
        private MealPlanController _mealPlanController;
        private TimeSpan _breakfast = TimeSpan.FromHours(8);
        private TimeSpan _lunch = TimeSpan.FromHours(12);
        private TimeSpan _dinner = TimeSpan.FromHours(5);

        [SetUp]
        public void SetUp()
        {
            //Arrange
            var savedRecipesFake = MockObjects.CreateSavedRecipeMock(10);
            var recipeRepoFake = MockObjects.CreateMockRecipeRepo(10);
            var userManagerFake = MockObjects.CreateUserMock();
            var mealRepoFake = MockObjects.CreateMealRepo(7);
            var resRepoFake = MockObjects.CreateRestrictionsRepo(10);
            _mealPlanController = new MealPlanController(recipeRepoFake.Object, userManagerFake.Object, savedRecipesFake.Object, mealRepoFake.Object, resRepoFake.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
        }

        [Test]
        public async Task TestMealPlanRoute_WithRecipes_ShouldReturnPopulatedMeals()
        {
            //Act
            var results = await _mealPlanController.MealPlan(3);
            var data = (results as PartialViewResult).Model as Meals;
            //assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Breakfast.Count, Is.EqualTo(7));
            Assert.That(data.Lunch.Count, Is.EqualTo(7));
            Assert.That(data.Dinner.Count, Is.EqualTo(7));
        }

        [Test]
        public async Task TestMealPlanRoute_WithoutRecipes_ShouldReturnNoMeals()
        {
            //Arrange
            var savedRecipesFake = MockObjects.CreateSavedRecipeMock(0);
            var recipeRepoFake = MockObjects.CreateMockRecipeRepo(0);
            var userManagerFake = MockObjects.CreateUserMock();
            var mealRepoFake = MockObjects.CreateMealRepo(10);
            var resRepoFake = MockObjects.CreateRestrictionsRepo(10);
            var controller = new MealPlanController(recipeRepoFake.Object, userManagerFake.Object, savedRecipesFake.Object, mealRepoFake.Object, resRepoFake.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
            //Act
            var results = await controller.MealPlan(0);
            var data = (results as PartialViewResult).Model as Meals;
            //assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Breakfast.Count, Is.EqualTo(10));
            Assert.That(data.Lunch.Count, Is.EqualTo(10));
            Assert.That(data.Dinner.Count, Is.EqualTo(10));
        }

        [Test]
        public async Task TestGetSavedRecipes_Should_ReturnListOfSavedRecipes()
        {
            //Act
            var results = await _mealPlanController.GetFavoritses();
            var data = (results as PartialViewResult).Model as IEnumerable<Savedrecipe>;
            //assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count(), Is.EqualTo(10));
        }

        [Test]
        public async Task TestGetSavedRecipes_WithNoRecipesShould_ReturnListOfSavedRecipes()
        {
            //Arrange
            var savedRecipesFake = MockObjects.CreateSavedRecipeMock(0);
            var recipeRepoFake = MockObjects.CreateMockRecipeRepo(0);
            var userManagerFake = MockObjects.CreateUserMock();
            var mealRepoFake = MockObjects.CreateMealRepo(10);
            var resRepoFake = MockObjects.CreateRestrictionsRepo(10);
            var controller = new MealPlanController(recipeRepoFake.Object, userManagerFake.Object, savedRecipesFake.Object, mealRepoFake.Object, resRepoFake.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
            //Act
            var results = await controller.GetFavoritses();
            var data = (results as PartialViewResult).Model as IEnumerable<Savedrecipe>;
            //assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Count(), Is.EqualTo(0));
        }

        //Test as of: 5/10/2021 - Trent Farley
        // Was not part of a story, but I did add this functionality
        [Test]
        public async Task TestIndex_WithMeals_ShouldReturnMeals()
        {
            var result = await _mealPlanController.Index();
            var data = (result as ViewResult).Model as Meals;

            Assert.That(data, Is.Not.Null);
            Assert.That(data.Breakfast.Count(), Is.EqualTo(7));
            Assert.That(data.Lunch.Count(), Is.EqualTo(7));
            Assert.That(data.Dinner.Count(), Is.EqualTo(7));
        }

        //For story: #177689443 Regenerate a new meal
        [Test]
        public async Task MealPlan_Should_Return_New_Meal()
        {
            var today = DateTime.Now;
            today = today.Date + _breakfast;
            var result = await _mealPlanController.RegenerateMeal(today.ToString());
            var data = (result as PartialViewResult).Model as Meal;
            Assert.That(data, Is.Not.Null);
            Assert.That(data.AccountId, Is.EqualTo("1"));
            Assert.That(data.RecipeId, Is.EqualTo(0));
        }

        // For story:  #177916812 See the details of a meal
        [Test]
        public async Task MealPlan_Should_Return_DetailedMeal()
        {
            var q = new Query() { QueryValue = "1" };
            var result = await _mealPlanController.MealDetails(q);
            var data = (result as PartialViewResult).Model as Meal;
            Assert.That(data.RecipeId, Is.EqualTo(1));
        }
    }
}