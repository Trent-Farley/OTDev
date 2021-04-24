using MealFridge.Models;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MealFridge.Utils;

namespace MealFridge.Tests
{
    [TestFixture]
    //INside of SearchSpnApi.cs, Line ~56
    //This function is being used when search for ingredients is happening. This was implemented last sprint, with a few
    //additions this sprint. Looks like this was originally implemented for this story #176855434, the function was refactored
    //a bit to be testable.
    public class TestParseIngredients
    {
        [Test]
        public void ParseIngredientEmptyList_Should_ReturnEmptyList()
        {
            var result = SpnApiService.ParseIngredient(null);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void ParseIngredientsSingleIngredient_Should_ReturnOneIngredientWithId()
        {
            var ingredientAsJson = new JObject
            {
                { "id", 1002 },
                { "name", "Test Ingredient" },
                { "image", "apple.jpg" }
            };
            var result = SpnApiService.ParseIngredient(ingredientAsJson);

            Assert.That(result.Id, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1002));
        }

        [Test]
        public void ParseIngredientsSingleIngredient_Should_ReturnOneIngredientWithName()
        {
            var ingredientAsJson = new JObject
            {
                { "id", 1002 },
                { "name", "Test Ingredient" },
                { "image", "apple.jpg" }
            };
            var result = SpnApiService.ParseIngredient(ingredientAsJson);

            Assert.That(result.Name, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Ingredient"));
        }

        [Test]
        public void ParseIngredientsSingleIngredient_Should_ReturnOneIngredientWithImage()
        {
            var ingredientAsJson = new JObject
            {
                { "id", 1002 },
                { "name", "Test Ingredient" },
                { "image", "apple.jpg" }
            };
            var result = SpnApiService.ParseIngredient(ingredientAsJson);

            Assert.That(result.Image, Is.Not.Null);
            Assert.That(result.Image, Is.EqualTo("https://spoonacular.com/cdn/ingredients_500x500/" + "apple.jpg"));
        }

        [Test]
        public void ParseIngredient_Should_ReturnNull()
        {
            var ingredientAsJson = new JObject
            {
                { "iddd", 1002 },
                { "nameee", "Test Ingredient" },
                { "imageee", "apple.jpg" }
            };
            var result = SpnApiService.ParseIngredient(ingredientAsJson);
            Assert.That(result, Is.Null);
        }
    }
}