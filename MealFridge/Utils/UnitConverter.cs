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
            {
                return val * DefAmount;
            }
            public double ConvertFromDef(double val)
            {
                return val / DefAmount;
            }
            
        }
        public static List<ConvertPair> massMetric = new List<ConvertPair>()
        {
            new ConvertPair("gram", "gram", 1, new List<string>() {"gram", "grams", "g" }),
            new ConvertPair("kilogram", "gram", .001, new List<string>() { "kilogram", "kilograms", "kg" }),
            new ConvertPair("milligram", "gram", 1000, new List<string>() { "milligram", "milligrams", "mg" })
        };
        public static List<ConvertPair> massUs = new List<ConvertPair>()
        {
            new ConvertPair("pound", "pound", 1, new List<string>() { "pound", "pounds", "lb" }),
            new ConvertPair("ounce", "pound", .0625, new List<string>() { "ounce", "ounces", "oz", "ozs" })
        };

        public static List<ConvertPair> volumeMetric = new List<ConvertPair>()
        {
            new ConvertPair("liter", "liter", 1, new List<string>() {"liter", "liters", "l" }),
            new ConvertPair("kiloliter", "liter", .001, new List<string>() {"kiloliter", "kiloliters", "kl" }),
            new ConvertPair("milliliter", "liter", 1000, new List<string>() {"milliliter", "milliliters", "ml" })
        };
        public static List<ConvertPair> volumeUs = new List<ConvertPair>()
        {
            new ConvertPair("cup", "cup", 1, new List<string>() {"cup","cups","c" }),
            new ConvertPair("dash", "cup", 1.0/384.0, new List<string>() {"dash","dashes"}),
            new ConvertPair("pinch", "cup", 1.0/768.0, new List<string>() {"pinch","pinches"}),
            new ConvertPair("smidge", "cup", 1.0/1536.0, new List<string>() {"smidge"}),
            new ConvertPair("teaspoon", "cup", 1.0/48.0, new List<string>() {"teaspoon","teaspoons","tsp" }),
            new ConvertPair("tablespoon", "cup", 1.0/16.0, new List<string>() {"tablespoon","tablespoons","tbs", "tbsp", "tbsps", "t" }),
            new ConvertPair("pint", "cup", 2, new List<string>() {"pint","pints", "pt" }),
            new ConvertPair("quart", "cup", 4, new List<string>() {"quart","quarts","qt" }),
            new ConvertPair("gallon", "cup", 16, new List<string>() {"gallon","gallons","gal" })
        };

        public static double Convert(double amount, string fromType, string toType)
        {
            double result = 0.0;
            double temp = 0.0;
            if (massMetric.Any(c => fromType.Contains(c.Type)))
            {
                temp = massMetric.First(c => fromType.Contains(c.Type)).ConvertToDef(amount);
                if (massMetric.Any(c => toType.Contains(c.Type)))
                    result = massMetric.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                else if (massUs.Any(c => toType.Contains(c.Type)))
                {
                    temp /= 454;
                    result = massUs.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (massUs.Any(c => fromType.Contains(c.Type)))
            {
                temp = massUs.First(c => fromType.Contains(c.Type)).ConvertToDef(amount);
                if (massUs.Any(c => toType.Contains(c.Type)))
                    result = massUs.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                else if (massMetric.Any(c => toType.Contains(c.Type)))
                {
                    temp *= 454;
                    result = massMetric.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (volumeMetric.Any(c => fromType.Contains(c.Type)))
            {
                temp = volumeMetric.First(c => fromType.Contains(c.Type)).ConvertToDef(amount);
                if (volumeMetric.Any(c => toType.Contains(c.Type)))
                    result = volumeMetric.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                else if (volumeUs.Any(c => toType.Contains(c.Type)))
                {
                    temp *= 4.167;
                    result = volumeUs.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                }
                else
                {
                    return double.NaN;
                }
            }
            else if (volumeUs.Any(c => fromType.Contains(c.Type)))
            {
                temp = volumeUs.First(c => fromType.Contains(c.Type)).ConvertToDef(amount);
                if (volumeUs.Any(c => toType.Contains(c.Type)))
                    result = volumeUs.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
                else if (volumeMetric.Any(c => toType.Contains(c.Type)))
                {
                    temp /= 4.167;
                    result = volumeMetric.First(c => toType.Contains(c.Type)).ConvertFromDef(temp);
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
    }
}
