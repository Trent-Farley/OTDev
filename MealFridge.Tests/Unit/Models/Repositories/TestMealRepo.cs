using MealFridge.Models;
using MealFridge.Models.Interfaces;
using MealFridge.Models.Repositories;
using MealFridge.Tests.Utils;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MealFridge.Tests.Unit.Models.Repositories
{
    /// <summary>
    /// Tester: Trent Farley
    /// For stories: #177689443 , #177916812 ,  #177689443
    /// </summary>

    public class TestMealRepo
    {
        private List<Recipe> _recipes;
        private List<Meal> _meals;
        private Mock<MealFridgeDbContext> _mockContext;
        private IMealRepo _mealRepo;
        private TimeSpan _breakfast = TimeSpan.FromHours(8);
        private TimeSpan _lunch = TimeSpan.FromHours(12);
        private TimeSpan _dinner = TimeSpan.FromHours(5);

        [SetUp]
        public void SetUp()
        {
            _recipes = MockObjects.CreateRecipes(30).ToList();
            _meals = MockObjects.CreateMeals(10);
            var mockMealDbSet = MockObjects.GetMockDbSet<Meal>(_meals.AsQueryable());
            var mockRecipeDbSet = MockObjects.GetMockDbSet<Recipe>(_recipes.AsQueryable());

            //Mock meal dbset setup
            mockMealDbSet.Setup(d => d.Add(It.IsAny<Meal>()))
                .Callback<Meal>(d => _meals.Add(d));
            mockMealDbSet.Setup(d => d.Update(It.IsAny<Meal>()))
                .Callback<Meal>(d =>
                {
                    _meals.Remove(_meals.Where(f => f.AccountId == d.AccountId && f.RecipeId == d.RecipeId).FirstOrDefault());
                    _meals.Add(d);
                });

            //Mock recipe repo dbset setup
            mockRecipeDbSet.Setup(d => d.Add(It.IsAny<Recipe>()))
                .Callback<Recipe>(d => _recipes.Add(d));
            mockRecipeDbSet.Setup(d => d.Update(It.IsAny<Recipe>()))
                .Callback<Recipe>(d =>
                {
                    _recipes.Remove(_recipes.Where(f => f.Id == d.Id).FirstOrDefault());
                    _recipes.Add(d);
                });
            // Setup of mockcontext
            _mockContext = new Mock<MealFridgeDbContext>();
            _mockContext.Setup(ctx => ctx.Meals).Returns(mockMealDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Recipe>()).Returns(mockRecipeDbSet.Object);
            _mockContext.Setup(ctx => ctx.Set<Meal>()).Returns(mockMealDbSet.Object);
            _mealRepo = new MealRepo(_mockContext.Object);
        }

        //Test one meal breakfast / lunch / dinner
        [Test]
        public void MealPlan_GetOneBreakfast_Should_Return_A_Breakfast()
        {
            var today = DateTime.Now;
            today = today.Date + _breakfast;
            var meal = _mealRepo.GetMeal(today, "1");
            Assert.That(meal, Is.Not.Null);
            Assert.That(meal.MealType, Is.EqualTo("Breakfast"));
        }

        [Test]
        public void MealPlan_GetOneLunch_Should_Return_A_Lunch()
        {
            var today = DateTime.Now;
            today = today.Date + _lunch;
            var meal = _mealRepo.GetMeal(today, "1");
            Assert.That(meal, Is.Not.Null);
            Assert.That(meal.MealType, Is.EqualTo("Lunch"));
        }

        [Test]
        public void MealPlan_GetOneDinner_Should_Return_A_Dinner()
        {
            var today = DateTime.Now;
            today = today.Date + _dinner;
            var meal = _mealRepo.GetMeal(today, "1");
            Assert.That(meal, Is.Not.Null);
            Assert.That(meal.MealType, Is.EqualTo("Dinner"));
        }

        //Test Getting multiple recipes

        [Test]
        public void MealPlan_GetWeeksWorth_ShouldReturn7DInners()
        {
            var today = DateTime.Now;
            today = today.Date + _dinner;
            var meals = _mealRepo.GetMeals(today, "1", 7, true);
            Assert.That(meals, Is.Not.Empty);
            Assert.That(meals.Count(), Is.EqualTo(7));
            meals.ForEach(meal => Assert.That(meal.MealType, Is.EqualTo("Dinner")));
        }

        [Test]
        public void MealPlan_GetWeeksWorth_ShouldReturnsLunches()
        {
            var today = DateTime.Now;
            today = today.Date + _lunch;
            var meals = _mealRepo.GetMeals(today, "1", 7, true);
            Assert.That(meals, Is.Not.Empty);
            Assert.That(meals.Count(), Is.EqualTo(7));
            meals.ForEach(meal => Assert.That(meal.MealType, Is.EqualTo("Lunch")));
        }

        [Test]
        public void MealPlan_GetWeeksWorth_ShouldReturnsBreakfasts()
        {
            var today = DateTime.Now;
            today = today.Date + _breakfast;
            var meals = _mealRepo.GetMeals(today, "1", 7, true);
            Assert.That(meals, Is.Not.Empty);
            Assert.That(meals.Count(), Is.EqualTo(7));
            meals.ForEach(meal => Assert.That(meal.MealType, Is.EqualTo("Breakfast")));
        }

        // Tests with restrictions
        //Safe = non restrictions
        [Test]
        public void MealPlan_GetBreakfastWeekWithRestrictions_ShouldReturnSafeMeals()
        {
            var today = DateTime.Now;
            today = today.Date + _breakfast;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeals(today, "1", bans, dislikes, 7, true);
            Assert.That(result, Is.Not.Null);
            foreach (var m in result)
            {
                Assert.That(m.MealType, Is.EqualTo("Breakfast"));
                foreach (var ingredient in m.Recipe.Recipeingreds)
                {
                    Assert.That(bans, Does.Not.Contain(ingredient));
                }
            }
        }

        [Test]
        public void MealPlan_GetLunchWeekWithRestrictions_ShouldReturnSafeMeals()
        {
            var today = DateTime.Now;
            today = today.Date + _lunch;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeals(today, "1", bans, dislikes, 7, true);
            Assert.That(result, Is.Not.Null);
            foreach (var m in result)
            {
                Assert.That(m.MealType, Is.EqualTo("Lunch"));
                foreach (var ingredient in m.Recipe.Recipeingreds)
                {
                    Assert.That(bans, Does.Not.Contain(ingredient));
                }
            }
        }

        [Test]
        public void MealPlan_GetDinnerWeekWithRestrictions_ShouldReturnSafeMeals()
        {
            var today = DateTime.Now;
            today = today.Date + _dinner;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeals(today, "1", bans, dislikes, 7, true);
            Assert.That(result, Is.Not.Null);
            foreach (var m in result)
            {
                Assert.That(m.MealType, Is.EqualTo("Dinner"));
                foreach (var ingredient in m.Recipe.Recipeingreds)
                {
                    Assert.That(bans, Does.Not.Contain(ingredient));
                }
            }
        }

        // Test a single meal with restrictions
        [Test]
        public void MealPlan_GetDinnerWithRestrictions_ShouldReturnSafeMeal()
        {
            var today = DateTime.Now;
            today = today.Date + _dinner;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeal(today, "1", bans, dislikes);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MealType, Is.EqualTo("Dinner"));
            foreach (var ingredient in result.Recipe.Recipeingreds)
            {
                Assert.That(bans, Does.Not.Contain(ingredient));
            }
        }

        [Test]
        public void MealPlan_GetLunchWithRestrictions_ShouldReturnSafeMeal()
        {
            var today = DateTime.Now;
            today = today.Date + _lunch;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeal(today, "1", bans, dislikes);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MealType, Is.EqualTo("Lunch"));
            foreach (var ingredient in result.Recipe.Recipeingreds)
            {
                Assert.That(bans, Does.Not.Contain(ingredient));
            }
        }

        [Test]
        public void MealPlan_GetBreakfastWithRestrictions_ShouldReturnSafeMeal()
        {
            var today = DateTime.Now;
            today = today.Date + _breakfast;
            var bans = MockObjects.CreateIngredients(5);
            var dislikes = MockObjects.CreateIngredients(5);
            var result = _mealRepo.GetMeal(today, "1", bans, dislikes);
            Assert.That(result, Is.Not.Null);
            Assert.That(result.MealType, Is.EqualTo("Breakfast"));
            foreach (var ingredient in result.Recipe.Recipeingreds)
            {
                Assert.That(bans, Does.Not.Contain(ingredient));
            }
        }
    }
}