using MealFridge.Models.Interfaces;
using MealFridge.Utils;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace MealFridge.Models.Repositories
{
    public class SpnApiService : ISpnApiService
    {
        private Query _query;

        public Ingredient IngredientDetails(Ingredient query, string searchType) //Not currently called
        {
            //Find a new way to do this using standardized SendRequest() function
            //var jsonResponse = SendRequest(Source, Secret, query.Id.ToString(), searchType);
            var jsonResponse = "";
            var details = JObject.Parse(jsonResponse);
            query.Aisle = (string)details["aisle"];
            query.Cost = (decimal)details["estimatedCost"]["value"];
            if ((string)details["estimatedCost"]["unit"] == "US Cents")
            {
                query.Cost *= 10; //Rip this part out
            }
            var nutrients = details["nutrition"]["nutrients"].ToList();
            JsonParser.ParseNutrition(nutrients, query);
            return query;
        }

        public List<Ingredient> SearchIngredients(Query query)
        {
            _query = query;
            var jsonResponse = SendRequest();
            var output = new List<Ingredient>();
            var ingredients = JObject.Parse(jsonResponse);
            //Test Start
            foreach (var ingredient in ingredients["results"])
            {
                var temp = ParseIngredient(ingredient as JObject);
                if (temp != null)
                    output.Add(temp);
            }
            //Test End
            return output.ToList();
        }

        public static Ingredient ParseIngredient(JObject ingredient)
        {
            if (ingredient == null)
                return null;
            try
            {
                return new Ingredient
                {
                    Id = (int)ingredient["id"],
                    Name = (string)ingredient["name"],
                    Image = ApiConstants.IngredientImageUrl + (string)ingredient["image"]
                };
            }
            catch
            {
                return null;
            }
        }

        public List<Recipe> SearchApi(Query query)
        {
            _query = query;
            var jsonResponse = SendRequest();
            var output = new List<Recipe>();
            switch (_query.SearchType)
            {
                case "Recipe":
                    var recipes = JObject.Parse(jsonResponse);
                    if ((int)recipes["number"] == 0)
                        return null;

                    foreach (var recipe in recipes["results"])
                        output.Add(new Recipe
                        {
                            Id = (int)recipe["id"],
                            Title = (string)recipe["title"],
                            Image = "https://spoonacular.com/recipeImages/" + recipe["id"].ToString() + "-556x370." + recipe["imageType"].ToString()
                        });
                    break;

                case "Ingredient":
                    JArray recipesByIngredients = new JArray();
                    if (jsonResponse[0] == '{')
                    {
                        var res = JObject.Parse(jsonResponse);
                        recipesByIngredients = res["results"] as JArray;
                    }
                    else
                    {
                        var res = JArray.Parse(jsonResponse);
                        recipesByIngredients = res;
                    }
                    if (recipesByIngredients.Count <= 0)
                        return null;
                    for (var i = 0; i < recipesByIngredients.Count; ++i)
                    {
                        output.Add(new Recipe
                        {
                            Id = (int)recipesByIngredients[i]["id"],
                            Title = (string)recipesByIngredients[i]["title"],
                            Image = "https://spoonacular.com/recipeImages/" + (string)recipesByIngredients[i]["id"] + "-556x370." + (string)recipesByIngredients[i]["imageType"]
                        });
                    }
                    break;

                case "Details":
                    var recipeDetails = JObject.Parse(jsonResponse);
                    Recipe detailedRecipe = GetDetailRecipe(recipeDetails);
                    output.Add(detailedRecipe);
                    break;

                case "Random":
                    recipes = JObject.Parse(jsonResponse);
                    foreach (JObject recipe in recipes["recipes"])
                    {
                        output.Add(GetDetailRecipe(recipe));
                    }
                    break;

                default:
                    return null;
            }
            return output;
        }

        private static Recipe GetDetailRecipe(JObject recipeDetails)
        {
            var list = JsonParser.IngredientList(recipeDetails["extendedIngredients"].Value<JArray>());
            var detailedRecipe = new Recipe();
            try
            {
                detailedRecipe.Id = recipeDetails["id"].Value<int>();
                detailedRecipe.Title = recipeDetails["title"].Value<string>();
                detailedRecipe.Cost = recipeDetails["pricePerServing"].Value<decimal>();
                detailedRecipe.Minutes = recipeDetails["readyInMinutes"].Value<int>();
                detailedRecipe.Image = "https://spoonacular.com/recipeImages/" + recipeDetails["id"].Value<int>() + "-556x370." + recipeDetails["imageType"].Value<string>();
                detailedRecipe.Summery = recipeDetails["sourceUrl"].Value<string>();
                detailedRecipe.Servings = recipeDetails["servings"].Value<int>();
                JsonParser.ParseDishType(recipeDetails["dishTypes"].ToObject<List<JToken>>(), detailedRecipe);
                var nutrients = recipeDetails["nutrition"]["nutrients"].ToList();
                JsonParser.ParseNutrition(nutrients, detailedRecipe);
                detailedRecipe.Recipeingreds = JsonParser.GetIngredients(recipeDetails["nutrition"]["ingredients"].Value<JArray>(), recipeDetails["id"].Value<int>(), list);
            }
            catch
            {
                return detailedRecipe;
            }
            return detailedRecipe;
        }

        private string SendRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(_query.GetUrl);
            request.Accept = "application/json";
            string jsonString = null;
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    var stream = response.GetResponseStream();
                    var reader = new StreamReader(stream);
                    jsonString = reader.ReadToEnd();
                    reader.Close();
                    stream.Close();
                }
                return jsonString;
            }
            catch
            {
                return null;
            }
        }
    }
}