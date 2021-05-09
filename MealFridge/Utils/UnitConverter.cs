using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Utils
{
    public class UnitConverter
    {
        public class ConvertPair
        {
            public string Type { get; }
            public string DefType { get; }
            public double DefAmount { get; }
            public List<string> inputs { get; }

            public ConvertPair(string t, string d, double a, List<string> i)
            {
                Type = t;
                DefType = d;
                DefAmount = a;
                inputs = i;
            }
            public double ConvertToDef(double val)
            {//Big to small => decimal
                return val * DefAmount;
            }
            public double ConvertFromDef(double val)
            {
                return val / DefAmount;
            }

            public bool checkInput(string val)
            {
                bool check = false;
                if(inputs.Any(i => i == val))
                    check = true;
                return check;
            }
            
        }
        public static List<ConvertPair> massMetric = new List<ConvertPair>()
        {
            new ConvertPair("gram", "gram", 1, new List<string>() {"gram", "grams", "g" }),
            new ConvertPair("kilogram", "gram", 1000, new List<string>() { "kilogram", "kilograms", "kg" }),
            new ConvertPair("milligram", "gram", .001, new List<string>() { "milligram", "milligrams", "mg" })
        };
        public static List<ConvertPair> massUs = new List<ConvertPair>()
        {
            new ConvertPair("pound", "pound", 1, new List<string>() { "pound", "pounds", "lb" }),
            new ConvertPair("ounce", "pound", .0625, new List<string>() { "ounce", "ounces", "oz", "ozs" })
        };

        public static List<ConvertPair> volumeMetric = new List<ConvertPair>()
        {
            new ConvertPair("liter", "liter", 1, new List<string>() {"liter", "liters", "l" }),
            new ConvertPair("kiloliter", "liter", 1000, new List<string>() {"kiloliter", "kiloliters", "kl" }),
            new ConvertPair("milliliter", "liter", .001, new List<string>() {"milliliter", "milliliters", "ml" })
        };
        public static List<ConvertPair> volumeUs = new List<ConvertPair>()
        {
            new ConvertPair("cup", "cup", 1, new List<string>() {"cup","cups","c" }),
            new ConvertPair("dash", "cup", 1/384.0, new List<string>() {"dash","dashes"}),
            new ConvertPair("pinch", "cup", 1/768.0, new List<string>() {"pinch","pinches"}),
            new ConvertPair("smidge", "cup", 1/1536.0, new List<string>() {"smidge"}),
            new ConvertPair("teaspoon", "cup", 1/48.0, new List<string>() {"teaspoon","teaspoons","tsp" }),
            new ConvertPair("tablespoon", "cup", 0.0625, new List<string>() {"tablespoon","tablespoons","tbs", "tbsp", "tbsps", "t" }),
            new ConvertPair("pint", "cup", 2, new List<string>() {"pint","pints", "pt" }),
            new ConvertPair("quart", "cup", 4, new List<string>() {"quart","quarts","qt" }),
            new ConvertPair("gallon", "cup", 16, new List<string>() {"gallon","gallons","gal" })
        };

        public static double Convert(double amount, string fromType, string toType)
        {
            double result = 0.0;
            double temp = 0.0;
            if (massMetric.Any(c => c.checkInput(fromType)))
            {
                temp = massMetric.First(c => c.checkInput(fromType)).ConvertToDef(amount);
                if (massMetric.Any(c => c.checkInput(toType)))
                    result = massMetric.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                else if (massUs.Any(c => c.checkInput(toType)))
                {
                    temp /= 453.59237;
                    result = massUs.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (massUs.Any(c => c.checkInput(fromType)))
            {
                temp = massUs.First(c => c.checkInput(fromType)).ConvertToDef(amount);
                if (massUs.Any(c => c.checkInput(toType)))
                    result = massUs.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                else if (massMetric.Any(c => c.checkInput(toType)))
                {
                    temp *= 453.59237;
                    result = massMetric.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (volumeMetric.Any(c => c.checkInput(fromType)))
            {
                temp = volumeMetric.First(c => c.checkInput(fromType)).ConvertToDef(amount);
                if (volumeMetric.Any(c => c.checkInput(toType)))
                    result = volumeMetric.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                else if (volumeUs.Any(c => c.checkInput(toType)))
                {
                    temp *= 4.167;
                    result = volumeUs.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (volumeUs.Any(c => c.checkInput(fromType)))
            {
                temp = volumeUs.First(c => c.checkInput(fromType)).ConvertToDef(amount);
                if (volumeUs.Any(c => c.checkInput(toType)))
                    result = volumeUs.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                else if (volumeMetric.Any(c => c.checkInput(toType)))
                {
                    temp /= 4.22675;
                    result = volumeMetric.First(c => c.checkInput(toType)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (fromType == toType || fromType == "" || toType == "")
                result = amount;
            else
                return double.NaN;
            return result;
        }

        public static bool isVolume(string type)
        {
            if (volumeUs.Any(c => type.Contains(c.Type)) || volumeMetric.Any(c => type.Contains(c.Type)))
                return true;
            else 
                return false;
        }
        public static bool isMass(string type)
        {
            if (massUs.Any(c => type.Contains(c.Type)) || massMetric.Any(c => type.Contains(c.Type)))
                return true;
            else
                return false;
        }
        public static bool isMetric(string type)
        {
            if (massMetric.Any(c => type.Contains(c.Type)) || volumeMetric.Any(c => type.Contains(c.Type)))
                return true;
            else
                return false;
        }
        public static bool isUs(string type)
        {
            if (volumeUs.Any(c => type.Contains(c.Type)) || massUs.Any(c => type.Contains(c.Type)))
                return true;
            else
                return false;
        }
        public static KeyValuePair<string, double> RoundedAmount(double amount, string fromType)
        {
            string unit = "";
            double val = 0.0;
            if (isMass(fromType))
            {
                if (massMetric.Any(c => c.checkInput(fromType)))
                {
                    var kilo = Convert(amount, fromType, "kilogram");
                    if (kilo > 1)
                    {
                        unit += "kilogram";
                        val = kilo;
                    }
                }
                else
                {
                    var pound = Convert(amount, fromType, "pound");
                    if (pound > 1)
                    {
                        unit += "pound";
                        val = pound;
                    }                   
                }  
            }
            else if (isVolume(fromType))
            {
                if (volumeMetric.Any(c => c.checkInput(fromType)))
                {
                    var liter = Convert(amount, fromType, "liter");
                    if (liter > 1)
                    {
                        unit += "liter";
                        val = liter;
                    }
                }
                else
                {
                    var quart = Convert(amount, fromType, "quart");
                    if (quart > 1 && quart < 4)
                    {
                        unit += "quart";
                        val = quart;
                    }
                    else if (quart > 4)
                    {
                        unit += "gallon";
                        val = quart/4;
                    }
                    else if (quart > .5)
                    {
                        unit += "pint";
                        val = quart * 2;
                    }
                    else if (quart > .25)
                    {
                        unit += "cup";
                        val = quart * 4;
                    }
                }
            }
            if (val == 0.0)
            {
                unit += fromType;
                val = amount;
            }
            return new KeyValuePair<string, double>(unit, amount);
        }
    }
}
