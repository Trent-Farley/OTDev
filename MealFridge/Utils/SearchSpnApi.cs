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

        public SearchSpnApi(Query query)
        {
            _query = query;
        }

        public Ingredient IngredientDetails()
        {
            var jsonResponse = SendRequest();
            var details = JObject.Parse(jsonResponse);
            return Ingredient.CreateIngredient(details);
        }

        public List<Ingredient> SearchIngredients()
        {
            var jsonResponse = SendRequest();
            var output = new List<Ingredient>();
            var ingredients = JObject.Parse(jsonResponse);
            //Test Start
            foreach (var i in ingredients["results"])
            {
                var temp = Ingredient.CreateIngredient(i);
                if (temp != null)
                    output.Add(temp);
            }
            //Test End
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
                    //Test Start
                    if ((int)recipes["number"] == 0)
                        return null;

                    foreach (var recipe in recipes["results"])
                    {
                        var temp = Recipe.CreateRecipe(recipe);
                        if (temp != null)
                            output.Add(temp);
                    }
                    //Test End
                    break;

                case "Ingredient":
                    var recipesByIngredients = new JArray();
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
                    foreach (var recipe in recipesByIngredients)
                    {
                        var temp = Recipe.CreateRecipe(recipe);
                        if (temp != null)
                            output.Add(temp);
                    }
                    //Test End
                    break;

                case "Details":
                    var recipeDetails = JObject.Parse(jsonResponse);
                    var detailedRecipe = Recipe.CreateRecipe(recipeDetails);
                    output.Add(detailedRecipe);
                    //Test End
                    break;

                default:
                    Console.WriteLine("Never hit any case");
                    break;
            }
            return output;
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
    }
}