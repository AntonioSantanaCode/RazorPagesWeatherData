using RazorPagesUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesUI
{
    public class DataDB
    {
        public static void ReadCSV()
        {
            string[] resultSet = File.ReadAllLines(@"C:\Users\AntonioSantana\source\repos\RazorPagesDemo\RazorPagesUI\File\TemperaturData.csv");
            using (var context = new WeatherDbContext())
            {
                foreach (var data in resultSet)
                {
                    WeatherData tmp = new WeatherData();
                    string[] tmpString = data.Split(",");
                    tmp.DateAndTime = DateTime.Parse(tmpString[0]);
                    tmp.Location = tmpString[1];
                    tmp.Temperature = float.Parse(tmpString[2], CultureInfo.InvariantCulture);
                    tmp.Humidity = float.Parse(tmpString[3], CultureInfo.InvariantCulture);

                    context.Add(tmp);
                }
                context.SaveChanges();
            }
        }
    }
}
