using MealFridge.Controllers;
using MealFridge.Models;
using MealFridge.Models.ViewModels;
using MealFridge.Tests.Utils;
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
        [Test]
        public async Task TestMealPlanRoute_WithRecipes_ShouldReturnPopulatedMeals()
        {
            //Arrange
            var savedRecipesFake = MockObjects.CreateSavedRecipeMock(10);
            var recipeRepoFake = MockObjects.CreateMockRecipeRepo(10);
            var userManagerFake = MockObjects.CreateUserMock();
            var mealRepoFake = MockObjects.CreateMealRepo(10);
            var resRepoFake = MockObjects.CreateRestrictionsRepo(10);
            var savedRepoFake = MockObjects.CreateSavedRecipes(10);
            var controller = new MealPlanController(recipeRepoFake.Object, userManagerFake.Object, savedRecipesFake.Object, mealRepoFake.Object, resRepoFake.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = MockObjects.CreateMockContext().Object
                }
            };
            //Act
            var results = await controller.MealPlan(3);
            var data = (results as PartialViewResult).Model as Meals;
            //assert
            Assert.That(data, Is.Not.Null);
            Assert.That(data.Breakfast.Count, Is.EqualTo(10));
            Assert.That(data.Lunch.Count, Is.EqualTo(10));
            Assert.That(data.Dinner.Count, Is.EqualTo(10));
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
            //Arrange
            var savedRecipesFake = MockObjects.CreateSavedRecipeMock(10);
            var recipeRepoFake = MockObjects.CreateMockRecipeRepo(10);
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
    }
}