﻿using System;
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
                    default:
                        u = Url + "?apiKey=" + Credentials + "&" + QueryName + "=" + QueryValue + "&number=" + Number;
                        break;
                }
                return u;
            }
        }
    }
}