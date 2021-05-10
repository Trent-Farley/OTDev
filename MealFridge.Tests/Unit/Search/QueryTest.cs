using MealFridge.Utils;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace MealFridge.Tests
{
    [TestFixture]
    class QueryTest
    {
        private static Query MakeValidIngredientQuery()
        {
            return new Query
            {
                QueryName = "ingredients",
                QueryValue = "rice, water, flour",
                Credentials = "11111111111111111111111111111111",
                SearchType = "Ingredient",
                Refine = true,
                Url = "https://api.spoonacular.com/recipes/findByIngredients"
            };
        }
        private static Query MakeInvalidIngredientQuery()
        {
            return new Query
            {
                QueryName = null,
                QueryValue = null,
                Credentials = null,
                SearchType = null,
                Url = null
            };
        }

        private static Query MakeValidRecipeNameQuery()
        {
            return new Query
            {
                QueryName = "recipe",
                QueryValue = "eggs benedict",
                Credentials = "11111111111111111111111111111111",
                SearchType = "Recipe",
                Url = "https://api.spoonacular.com/recipes/complexSearch"
            };
        }
        private static Query MakeInvalidRecipeNameQuery()
        {
            return new Query
            {
                QueryName = null,
                QueryValue = null,
                Credentials = null,
                SearchType = null,
                Url = null
            };
        }

        private static Query MakeValidIngredientDetailsQuery()
        {
            return new Query
            {
                QueryName = "id",
                Credentials = "11111111111111111111111111111111",
                Url = "https://api.spoonacular.com/recipes/123/information",
                SearchType = "Details"
            };
        }
        private static Query MakeInvalidIngredientDetailsQuery()
        {
            return new Query
            {
                QueryName = null,
                Credentials = null,
                Url = null,
                SearchType = null
            };
        }
        public bool ValidateIngredient(string url)
        {
            var urlExpression = "https://api.spoonacular.com/recipes/findByIngredients\\?apiKey=11111111111111111111111111111111&ingredients=[a-z].+?(?=&)&(.*)";
            var regex = new Regex(urlExpression);
            return regex.IsMatch(url);
        }
        public bool ValidateRecipeName(string url)
        {
            var urlExpression = "https://api.spoonacular.com/recipes/complexSearch\\?apiKey=11111111111111111111111111111111&recipe=[a-z].+?(?=&)&(.*)";
            var regex = new Regex(urlExpression);
            return regex.IsMatch(url);
        }
        public bool ValidateIngredientDetails(string url)
        {
            var urlExpression = "https://api.spoonacular.com/recipes/[0-10].+?(?=/)/information\\?apiKey=11111111111111111111111111111111(.*)";
            var regex = new Regex(urlExpression);
            return regex.IsMatch(url);
        }
        [Test]
        public void GetUrlIngredientIsValid()
        {
            var temp = MakeValidIngredientQuery();
            Assert.IsTrue(ValidateIngredient(temp.GetUrl));
        }
        [Test]
        public void GetUrlIngredientIsNotValid()
        {
            var temp = MakeInvalidIngredientQuery();
            Assert.IsFalse(ValidateIngredient(temp.GetUrl));
        }
        [Test]
        public void GetUrlRecipeByNameIsValid()
        {
            var temp = MakeValidRecipeNameQuery();
            Assert.IsTrue(ValidateRecipeName(temp.GetUrl));
        }
        [Test]
        public void GetUrlRecipeByNameIsNotValid()
        {
            var temp = MakeInvalidRecipeNameQuery();
            Assert.IsFalse(ValidateRecipeName(temp.GetUrl));
        }
        [Test]
        public void GetUrlIngredientDetailsIsValid()
        {
            var temp = MakeValidIngredientDetailsQuery();
            Assert.IsTrue(ValidateIngredientDetails(temp.GetUrl));
        }
        [Test]
        public void GetUrlIngredientDetailsIsNotValid()
        {
            var temp = MakeInvalidIngredientDetailsQuery();
            Assert.IsFalse(ValidateIngredientDetails(temp.GetUrl));
        }
    }
}
