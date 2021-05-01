using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MealFridge.Utils
{
    public static class DatesGenerator
    {
        private static List<string> Days = new List<string>()
        {
            "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"
        };

        public static List<string> GetDays(int numOfDays)
        {
            var newDays = new List<string>();
            int day = 0;
            while (numOfDays > 0)
            {
                if (day > 7)
                    day = 0;
                newDays.Add(Days[day]);
                day++;
                --numOfDays;
            }
            return newDays;
        }
    }
}