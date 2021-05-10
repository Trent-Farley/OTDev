using MealFridge.Models;
using MealFridge.Utils;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace MealFridge.Tests.UtilTests
{
    [TestFixture]
    //Testing the JsonParser.cs in the Utils Folder  
    class JsonParserTests
    {
        //Testing the Dish Parsing Function -- Josh

        [Test]
        public void TestEmptyListJObjectLeavesDishTypeBoolsNull()
        {
            List<JToken> testObj = new List<JToken>();
            var testRecipe = new Recipe
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(testObj, testRecipe);
            Assert.IsNull(testRecipe.Breakfast);
            Assert.IsNull(testRecipe.Lunch);
            Assert.IsNull(testRecipe.Dinner);
            Assert.IsNull(testRecipe.Dessert);
            Assert.IsNull(testRecipe.Snack);
        }

        [Test]
        public void TestEmptyRecipeLeavesRecipeEmpty()
        {
            var j = JArray.Parse(@"['lunch','main course','main dish','dinner']");
            var testRecipe = new Recipe();
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsNull(testRecipe.Breakfast);
            Assert.IsNull(testRecipe.Lunch);
            Assert.IsNull(testRecipe.Dinner);
            Assert.IsNull(testRecipe.Dessert);
            Assert.IsNull(testRecipe.Snack);
        }
        [Test]
        public void TestNullRecipeLeavesRecipeNull()
        {
            var j = JArray.Parse(@"['lunch','main course','main dish','dinner']");
            Recipe testRecipe = null;
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsNull(testRecipe);
        }
        [Test]
        public void TestLunchRecipe()
        {
            var j = JArray.Parse(@"['lunch']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsTrue(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestBreakfastRecipe()
        {
            var j = JArray.Parse(@"['breakfast']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsTrue(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestDinnerRecipe()
        {
            var j = JArray.Parse(@"['dinner']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsTrue(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestDessertRecipe()
        {
            var j = JArray.Parse(@"['dessert']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsTrue(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestSnackRecipe()
        {
            var j = JArray.Parse(@"['snack']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsTrue(testRecipe.Snack);
        }
        [Test]
        public void TestBreakfastAndDinnerRecipe()
        {
            var j = JArray.Parse(@"['breakfast', 'dinner']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsTrue(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsTrue(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestLunchAndBreakfastRecipe()
        {
            var j = JArray.Parse(@"['breakfast', 'lunch']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsTrue(testRecipe.Breakfast);
            Assert.IsTrue(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestSupperForDinnerRecipe()
        {
            var j = JArray.Parse(@"['supper']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsTrue(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestWonkyCapitalizationRecipe()
        {
            var j = JArray.Parse(@"['dInnEr']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsTrue(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestNoMatchRecipe()
        {
            var j = JArray.Parse(@"['side dish']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
        [Test]
        public void TestPartialMatchRecipe()
        {
            var j = JArray.Parse(@"['side dish', 'snack']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsTrue(testRecipe.Snack);
        }
        [Test]
        public void TestJunkJsonArrayRecipe()
        {
            var j = JArray.Parse(@"['gobbledy gook']");
            var testRecipe = new Recipe()
            {
                Id = 1,
                Title = "Test Recipe"
            };
            JsonParser.ParseDishType(j.ToObject<List<JToken>>(), testRecipe);
            Assert.IsFalse(testRecipe.Breakfast);
            Assert.IsFalse(testRecipe.Lunch);
            Assert.IsFalse(testRecipe.Dinner);
            Assert.IsFalse(testRecipe.Dessert);
            Assert.IsFalse(testRecipe.Snack);
        }
    }
}
