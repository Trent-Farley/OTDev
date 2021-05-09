//Tests created by Chris Edwards covering User Story 177689552
using System;
using System.Collections.Generic;
using System.Text;
using MealFridge.Models;
using MealFridge.Utils;
using MealFridge.Models.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MealFridge.Models.Repositories;
using MealFridge.Tests.Utils;

namespace MealFridge.Tests.Models
{
    class TestRecipeRepo
    {
        [SetUp]
        public void SetUp()
        {

        }
        [Test]
        public void GetRecipesByName()
        {
            List<Recipe> recipes = new List<Recipe>
            {
                new Recipe{Title="apple pie", Id = 0, Meals = null, Savedrecipes = null, Recipeingreds = null},
                new Recipe{Title="ice cream", Id = 1},
                new Recipe{Title="Chocolate Cake", Id = 2},
                new Recipe{Title="peanut butter & jelly", Id = 3}
            };
            var mockRecipeDbSet = MockObjects.GetMockDbSet(recipes.AsQueryable());
            Mock<MealFridgeDbContext> mockContext = new Mock<MealFridgeDbContext>();
            mockContext.Setup(ctx => ctx.Set<Recipe>()).Returns(mockRecipeDbSet.Object);
            IRecipeRepo recipeRepo = new RecipeRepo(mockContext.Object);

            List<string> recipesToTest = new List<string>();
            recipesToTest.Add("apple pie");
            recipesToTest.Add("ice cream");

            var temp = recipeRepo.GetRecipesbyNames(recipesToTest, recipeRepo.GetAll());
            Assert.That(temp[0].Title == recipesToTest[0]);
        }
    }
}
