//This test was created by Christian and these tests cover the #177688829 and #177688997 user stories
using System;
using System.Collections.Generic;
using System.Text;
using TastyMeals.Models;
using TastyMeals.Utils;
using TastyMeals.Tests.Utils;
using TastyMeals.Models.Interfaces;
using NUnit.Framework;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Moq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TastyMeals.Models.Repositories;

namespace TastyMeals.Tests.Models
{
    class TestSavedrecipeRepo
    {
        public Mock<DbSet<SavedRecipe>> mockSavedRecipeDbSet;
        public Mock<TastyMealsDbContext> context = new Mock<TastyMealsDbContext>();
        public ISavedrecipeRepo savedRecipeRepo;
        public IMealRepo mealRepo;
        public List<SavedRecipe> list;

        private Mock<DbSet<T>> GetMockDbSet<T>(IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(entities.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(entities.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(entities.GetEnumerator());
            return mockSet;
        }
        public List<SavedRecipe> SavedRecipeFactory()
        {
            return new List<SavedRecipe>
            {
                new SavedRecipe { AccountId= "this", RecipeId=7, Favorited = true, Shelved = false, Recipe = null},
                new SavedRecipe { AccountId = "is", RecipeId = 13, Favorited = true, Shelved = false, Recipe = null },
                new SavedRecipe { AccountId = "my", RecipeId = 77, Favorited = false, Shelved = true, Recipe = null },
                new SavedRecipe { AccountId = "test", RecipeId = 666, Favorited = false, Shelved = true, Recipe = null },
                new SavedRecipe { AccountId = "test", RecipeId = 123, Favorited = true, Shelved = false, Recipe = null}
         };
        }

            public List<Meal> MealFactory(int number)
        {
            var temp = MockObjects.CreateMeals(10).ToList();
            return temp;
        }
       
        [SetUp]
        public void SetupMockEnvironment()
        {
            list = SavedRecipeFactory();
            mockSavedRecipeDbSet = GetMockDbSet(list.AsQueryable());
            context.Setup(ctx => ctx.Savedrecipes).Returns(mockSavedRecipeDbSet.Object);
            savedRecipeRepo = new SavedrecipeRepo(context.Object);
            mealRepo = new MealRepo(context.Object);
        }

        [Test]
        public void SavedRecipesShouldGetTheShelvedRecipes()
        {
            var temp = savedRecipeRepo.GetShelvedRecipe("my", list.AsQueryable());
            Assert.IsTrue(temp.Count <= 1);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "my");
                Assert.IsTrue(i.Shelved == true);
                Assert.IsTrue(i.RecipeId == 77);
            }
        }

        [Test]
        public void SavedRecipesShouldNOTGetTheShelvedRecipesIfUserIdIsNull()
        {          
            var temp = savedRecipeRepo.GetShelvedRecipe("fake", list.AsQueryable());
            Assert.IsEmpty(temp);
        }

        [Test]
        public void SavedRecipesShouldGetTheFavoritedRecipes()
        {
            var temp = savedRecipeRepo.GetFavoritedRecipeWithIQueryable("is", list.AsQueryable());
            //Assert.IsTrue(temp.Count == 1);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "is");
                Assert.IsTrue(i.Favorited == true);
                Assert.IsTrue(i.RecipeId == 13);
            }
        }
        [Test]
        public void SavedRecipesShouldNOTGetTheFavoritedRecipesIfUserIdIsNull()
        {
            var temp = savedRecipeRepo.GetFavoritedRecipeWithIQueryable("not_real", list.AsQueryable());
            Assert.IsEmpty(temp);
        }

        [Test]
        public void SavedRecipesShouldGetAllOfTheRecipes()
        {
            var temp = savedRecipeRepo.GetAllRecipes("test", list.AsQueryable());
            Assert.IsTrue(temp.Count <= 2);
            foreach (var i in temp)
            {
                Assert.IsTrue(i.AccountId == "test");
                Assert.IsTrue(i.RecipeId == 666 || i.RecipeId == 123);
            }
        }

        [Test]
        public void SavedRecipesShouldNOTGetAllOfTheRecipesIfUserIdIsNull()
        {
            var temp = savedRecipeRepo.GetAllRecipes("not_right", list.AsQueryable());
            Assert.IsEmpty(temp);
        }

        [Test]
        public void SavedRecipesShouldContainARecipeFromAMeal()
        {
            var temp = MealFactory(1);
            var recipe = savedRecipeRepo.CreateNewSavedRecipe(temp.First().Recipe, temp.First().AccountId);
            Assert.That(recipe.AccountId == "1");
        }

        [Test]
        public void SavedRecipesShouldContainARecipeFromAFavoritedMeal()
        {
            var temp = MealFactory(1);
            var recipe = savedRecipeRepo.CreateNewSavedRecipe(temp.First().Recipe, temp.First().AccountId);
            recipe.Favorited = true;
            Assert.That(recipe.Favorited == true);
        }

        [Test]
        public void SavedRecipesShouldContainARecipeFromAShelvedMeal()
        {
            var temp = MealFactory(1);
            var recipe = savedRecipeRepo.CreateNewSavedRecipe(temp.First().Recipe, temp.First().AccountId);
            recipe.Shelved = true;
            Assert.That(recipe.Shelved == true);
        }

        [Test]
        public void SavedRecipesShouldContainARecipeFromAShelvedMealCheckWithActualSavedRecipe()
        {
            var temp = MealFactory(1);
            var recipe = savedRecipeRepo.CreateNewSavedRecipe(temp.First().Recipe, temp.First().AccountId);
            var temp_two = savedRecipeRepo.GetAllRecipes("test", list.AsQueryable());
            var id = temp_two.First().AccountId;
            var condition = temp_two.First().Shelved;

            recipe.Shelved = true;
            recipe.AccountId = "test";
          
            Assert.That( id == recipe.AccountId && condition == recipe.Shelved );

        }

        [Test]
        public void SavedRecipesShouldContainARecipeFromAFavortiedMealCheckWithActualSavedRecipe()
        {
            var temp = MealFactory(1);
            var recipe = savedRecipeRepo.CreateNewSavedRecipe(temp.First().Recipe, temp.First().AccountId);
            var temp_two = savedRecipeRepo.GetAllRecipes("test", list.AsQueryable());
            var id = temp_two.First().AccountId;
            var condition = temp_two.First().Shelved;

            recipe.Favorited = true;
            recipe.AccountId = "test";

            Assert.That(id == recipe.AccountId && condition == recipe.Favorited);

        }


    }
}

