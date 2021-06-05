//Tests created by Chris Edwards covering User Story 177689552
using System;
using System.Collections.Generic;
using System.Text;
using TastyMeals.Models;
using TastyMeals.Utils;
using TastyMeals.Models.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TastyMeals.Models.Repositories;
using TastyMeals.Tests.Utils;

namespace TastyMeals.Tests.Models
{
    class TestRecipeRepo
    {
        private Mock<DbSet<Recipe>> _mockRecipeDbSet;
        private List<Recipe> _recipes;
        Mock<TastyMealsDbContext> _mockContext;
        IRecipeRepo _recipeRepo;
        [SetUp]
        public void SetUp()
        {
            _recipes = new List<Recipe>
            {
                new Recipe{Title="apple pie", Id = 0, Meals = null, Savedrecipes = null, Recipeingreds = null},
                new Recipe{Title="ice cream", Id = 1},
                new Recipe{Title="Chocolate Cake", Id = 2},
                new Recipe{Title="peanut butter & jelly", Id = 3}
            };
            _mockRecipeDbSet = MockObjects.GetMockDbSet(_recipes.AsQueryable());
            _mockContext = new Mock<TastyMealsDbContext>();
            _mockContext.Setup(ctx => ctx.Set<Recipe>()).Returns(_mockRecipeDbSet.Object);
            _recipeRepo = new RecipeRepo(_mockContext.Object);

        }
        [Test]
        public void GetRecipesByNameTest_CheckForRecipesIntheDatabase_ShouldReturnRecipes()
        {
            SetUp();
            List<string> recipesToTest = new List<string>();
            recipesToTest.Add("apple pie");
            recipesToTest.Add("ice cream");
            var temp = _recipeRepo.GetRecipesbyNames(recipesToTest, _recipeRepo.GetAll());
            Assert.That(temp[0].Title == recipesToTest[0]);
            Assert.That(temp[1].Title == recipesToTest[1]);
        }

        [Test]
        public void GetRecipesByNameTest_CheckForRecipesNOTInTheDB_ShouldReturnEmptyList()
        {
            SetUp();
            List<string> recipesToTest = new List<string>();
            recipesToTest.Add("Sandwich");
            recipesToTest.Add("Good Salad");
            var temp = _recipeRepo.GetRecipesbyNames(recipesToTest, _recipeRepo.GetAll());
            Assert.That(temp.Count < 1);
        }
    }
}
