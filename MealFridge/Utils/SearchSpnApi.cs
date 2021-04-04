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
        public Ingredient IngredientDetails(Ingredient query, string searchType)
        {
            var jsonResponse = SendRequest(Source, Secret, query.Id.ToString(), searchType);
            var details = JObject.Parse(jsonResponse);
            query.Aisle = (string)details["aisle"];
            query.Cost = (decimal)details["estimatedCost"]["value"];
            if ((string)details["estimatedCost"]["unit"] == "US Cents")
            {
                query.Cost *= 10; //To show price in dollars, might want to track the Cost unit type though.
            }
            return query;
        }

        public List<Ingredient> SearchIngredients()
        {
            var jsonResponse = SendRequest();
            var output = new List<Ingredient>();
            var ingredients = JObject.Parse(jsonResponse);
            foreach (var i in ingredients["results"])
            {
                var temp = new Ingredient
                {
                    Id = (int)i["id"],
                    Name = (string)i["name"],
                    Image = "https://spoonacular.com/cdn/ingredients_500x500/" + i["image"]
                };
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
                if (!int.TryParse(ing["id"].ToString(), out _))
                    continue;
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
