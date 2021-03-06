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

        public SearchSpnApi(string endpoint, string key)
        {
            Source = endpoint;
            Secret = key;
        }
        public SearchSpnApi(Query query)
        {
            _query = query;
        }
        public List<Ingredient> SearchIngredientsApi(string query, string searchType)
        {


            if (Source != null)
            {
                var temp = new Query
                {
                    Url = Source,
                    Credentials = Secret,
                    QueryName = "query",
                    QueryValue = query
                };
                _query = temp;
            }

            var jsonResponse = SendRequest();
            var output = new List<Ingredient>();
            var ingredients = JObject.Parse(jsonResponse);
            foreach (var i in ingredients["results"])
            {
                var temp = new Ingredient();
                temp.Id = (int)i["id"];
                temp.Name = (string)i["name"];
                temp.Image = "https://spoonacular.com/cdn/ingredients_250x250/" + i["image"];
                output.Add(temp);

            }
            return output.ToList();
        }
        public List<Recipe> SearchAPI()
        {
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
                    JArray recipesByIngredients = JArray.Parse(jsonResponse);

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
                    var detailedRecipe = new Recipe
                    {
                        Id = recipeDetails["id"].Value<int>(),
                        Title = recipeDetails["title"].Value<string>(),
                        Image = "https://spoonacular.com/recipeImages/" + recipeDetails["id"].Value<int>() + "-556x370." + recipeDetails["imageType"].Value<string>(),
                        Summery = recipeDetails["sourceUrl"].Value<string>(),
                        Recipeingreds = GetIngredients(recipeDetails["extendedIngredients"].Value<JArray>())

                    };
                    output.Add(detailedRecipe);
                    break;
                default:
                    Console.WriteLine("Never hit any case");
                    break;
            }
            return output;
        }

        private ICollection<Recipeingred> GetIngredients(JArray ingredients)
        {
            var retingredients = new List<Recipeingred>();
            foreach (var ing in ingredients)
            {

                retingredients.Add(new Recipeingred
                {
                    //Or amount + unit to get each component i.e 1.0 tbsp butter
                    Amount = ing["original"]?.Value<string>(),
                    Ingred = new Ingredient
                    {
                        Name = ing["name"]?.Value<string>(),
                        Id = ing["id"].Value<int>()
                    }
                });

            }
            return retingredients;
        }

        private string SendRequest()
        {
            try
            {
                //number selects the number of results to return from API (FOR FUTURE REFERENCE)
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
    }
}
