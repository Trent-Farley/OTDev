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
        public string Source { get; set; }
        private string Secret { get; set; }

        public SearchSpnApi(string endpoint, string key)
        {
            Source = endpoint;
            Secret = key;
        }
        public List<Recipe> SearchAPI(string query, string searchType)
        {
            var jsonResponse = SendRequest(Source, Secret, query, searchType);
            Debug.WriteLine(jsonResponse);
            var output = new List<Recipe>();
            switch (searchType)
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
            }
            return output;
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
                    request = (HttpWebRequest)WebRequest.Create(url + "?apiKey=" + credentials + "&ingredients=" + query + "&number=10");
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
