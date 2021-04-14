using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using MealFridge.Models;

namespace MealFridge.Utils
{
    public class SearchSpnApi
    {
        private Query _query;
        public string Source { get; set; }
        private string Secret { get; set; }

        public SearchSpnApi(Query query)
        {
            _query = query;
        }
        public Ingredient IngredientDetails(Ingredient query, string searchType) //Not currently called
        {
            var jsonResponse = SendRequest(Source, Secret, query.Id.ToString(), searchType);
            var details = JObject.Parse(jsonResponse);
            query.Aisle = (string)details["aisle"];
            query.Cost = (decimal)details["estimatedCost"]["value"];
            if ((string)details["estimatedCost"]["unit"] == "US Cents")
            {
                query.Cost *= 10; //To show price in dollars, might want to track the Cost unit type though.
            }
            var nutrients = details["nutrition"]["nutrients"].ToList();
            JsonParser.ParseNutrition(nutrients, query);
            return query;
        }

        public List<Ingredient> SearchIngredients()
        {
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
                    Image = "https://spoonacular.com/cdn/ingredients_500x500/" + ingredient["image"]
                };
            }
            catch
            {
                return null;
            }
        }

        public List<Recipe> SearchAPI()
        {
            var jsonResponse = SendRequest();
            var output = new List<Recipe>();
            switch (_query.SearchType)
            {
                case "Recipe":
                    var recipes = JObject.Parse(jsonResponse);
                    //Test Start

                    if ((int)recipes["number"] == 0)
                        return null;

                    foreach (var recipe in recipes["results"])
                        output.Add(new Recipe
                        {
                            Id = (int)recipe["id"],
                            Title = (string)recipe["title"],
                            Image = "https://spoonacular.com/recipeImages/" + recipe["id"].ToString() + "-556x370." + recipe["imageType"].ToString()
                        });
                    //Test End
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
                    //Test End
                    break;

                case "Details":
                    var recipeDetails = JObject.Parse(jsonResponse);
                    //Test Start
                    var list = JsonParser.IngredientList(recipeDetails["extendedIngredients"].Value<JArray>());

                    var detailedRecipe = new Recipe
                    {
                        Id = recipeDetails["id"].Value<int>(),
                        Title = recipeDetails["title"].Value<string>(),
                        Cost = recipeDetails["pricePerServing"].Value<decimal>(),
                        Minutes = recipeDetails["readyInMinutes"].Value<int>(),
                        Image = "https://spoonacular.com/recipeImages/" + recipeDetails["id"].Value<int>() + "-556x370." + recipeDetails["imageType"].Value<string>(),
                        Summery = recipeDetails["sourceUrl"].Value<string>(),
                        Servings = recipeDetails["servings"].Value<int>(),
                        Recipeingreds = GetIngredients(recipeDetails["nutrition"]["ingredients"].Value<JArray>(), recipeDetails["id"].Value<int>(), list)
                    };
                    JsonParser.ParseDishType(recipeDetails["dishTypes"].ToObject<List<JToken>>(), detailedRecipe);
                    var nutrients = recipeDetails["nutrition"]["nutrients"].ToList();
                    JsonParser.ParseNutrition(nutrients, detailedRecipe);
                    output.Add(detailedRecipe);
                    //Test End
                    break;

                default:
                    Console.WriteLine("Never hit any case");
                    break;
            }
            return output;
        }
        //Put into json parser later
        private ICollection<Recipeingred> GetIngredients(JArray ingredients, int recipeId, List<Ingredient> list) //Can Test whole function
        {
            var retingredients = new List<Recipeingred>();
            foreach (var ing in ingredients)
            {
                int ingId;
                if (!int.TryParse(ing["id"].ToString(), out ingId))
                    continue;
                if (retingredients.Any(i => i.IngredId == ingId))
                {
                    retingredients.First(i => i.IngredId == ingId).Amount += ing["amount"]?.Value<double>();
                    continue;
                }
                var newRI = new Recipeingred();
                newRI.RecipeId = recipeId;
                newRI.IngredId = ingId;
                newRI.Amount = ing["amount"]?.Value<double>();
                newRI.ServingUnit = ing["unit"]?.Value<string>();
                newRI.Ingred = list.FirstOrDefault(i => i.Id == ingId);
                var nutrients = ing["nutrients"].ToList();
                JsonParser.ParseNutrition(nutrients, newRI);
                retingredients.Add(newRI);
            }
            return retingredients;
        }

        private string SendRequest()
        {
            try
            {
                HttpWebRequest request;
                request = (HttpWebRequest)WebRequest.Create(_query.GetUrl);
                request.Accept = "application/json";
                string jsonString = null;
                using (WebResponse response = request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(stream);
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

        private static string SendRequest(string url, string credentials, string query, string searchType)
        {

            //number selects the number of results to return from API (FOR FUTURE REFERENCE)
            HttpWebRequest request;
            switch (searchType)
            {
                case "Recipe":
                    request = (HttpWebRequest)WebRequest.Create(url + "?apiKey=" + credentials + "&query=" + query + "&number=10");
                    break;

                case "Ingredient":
                    request = (HttpWebRequest)WebRequest.Create(url + "?apiKey=" + credentials + "&ingredients=" + query + "&number=10" + "&ignorePantry=");
                    break;

                case "IngredientDetails":
                    request = (HttpWebRequest)WebRequest.Create(url + query + "/information?apiKey=" + credentials + "&amount=1&unit=serving");
                    break;


                default:
                    request = (HttpWebRequest)WebRequest.Create(url + "?apiKey=" + credentials + "&query=" + query + "&number=10");
                    break;
            }
            request.Accept = "application/json";
            string jsonString = null;
            using (WebResponse response = request.GetResponse())
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream);
                jsonString = reader.ReadToEnd();
                reader.Close();
                stream.Close();
            }
            return jsonString;
        }

    }
}