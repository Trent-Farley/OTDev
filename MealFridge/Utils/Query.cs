using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Utils
{
    public class Query
    {
        public string Url { get; set; }
        public string QueryName { get; set; }
        public string QueryValue { get; set; }
        public string Credentials { get; set; }
        public string SearchType { get; set; }
        public bool Refine { get; set; }
        public int PageNumber { get; set; } = 0;

        private readonly string Number = "10";

        public string GetUrl
        {
            get
            {
                string u;
                switch (SearchType)
                {
                    case "IngredientDetails":
                        u = Url + QueryValue + "/information?apikey=" + Credentials + "&amount=1&unit=serving";
                        break;

                    case "Ingredient":
                        u = Url + "?apiKey=" + Credentials + "&" + QueryName + "=" + QueryValue + "&number=" + Number + "&ignorePantry=" + Refine.ToString().ToLower()
                             + "&offset=" + (10 * PageNumber);
                        break;

                    case "Details":
                        u = Url + "?apiKey=" + Credentials + "&includeNutrition=true";
                        break;

                    case "Random":
                        u = Url + "?apiKey=" + Credentials + $"&type={QueryValue}&sort=random&addRecipeInformation=true&addRecipeNutrition=true&fillIngredients=true" + ApiConstants.RandomRecipeAmount;
                        break;

                    default:
                        u = Url + "?apiKey=" + Credentials + "&" + QueryName + "=" + QueryValue + "&number=" + Number + "&offset=" + (10 * PageNumber);
                        break;
                }
                return u;
            }
        }
    }
}