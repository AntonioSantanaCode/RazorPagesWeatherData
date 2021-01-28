using RazorPagesUI.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesUI
{
    public class QueryMethods
    {
        public static List<string> MoldRiskQuery(bool location)
        {
            string output = location ? "ute" : "inne"; // Ternary for data by location

            using (var context = new WeatherDbContext())
            {
                List<string> moldRiskList = new List<string>();

                var resultSet = context.WeatherData //Linq method for extracting the moldrisk inside and outside by location.
                    .Where(x => x.Location == output)
                    .GroupBy(x => x.DateAndTime.Date)
                    .Select(x => new { DateAndTime = x.Key, avgTemp = x.Average(x => x.Temperature), avgHum = x.Average(x => x.Humidity) })
                    .OrderBy(x => ((x.avgHum - 78) * (x.avgTemp / 15) / 0.22));

                var firstTen = resultSet //takes the first 10 values in resultSet
                    .Take(10);
                var lastTen = resultSet //Takes the last 10 in resultSet
                    .OrderByDescending(x => ((x.avgHum - 78) * (x.avgTemp / 15) / 0.22))
                    .Take(10)
                    .OrderBy(x => ((x.avgHum - 78) * (x.avgTemp / 15) / 0.22));

                foreach (var data in firstTen) //Adding the 10 values as strings from firstTen to moldRiskList
                {
                    if (Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22) <= 0)
                    {
                        moldRiskList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22)} % (No risk.)");
                    }
                    else
                    {
                        moldRiskList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22), 2} %");
                    }
                }
                moldRiskList.Add("--------------");
                foreach (var data in lastTen) //Adding the 10 values as strings from lastTen to moldRiskList
                {
                    if (Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22) <= 0)
                    {
                        moldRiskList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22), 2} % (No risk.)");
                    }
                    else
                    {
                        moldRiskList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round((data.avgHum - 78) * (data.avgTemp / 15) / 0.22), 2} %");
                    }
                }
                return moldRiskList;
            }
        }
        public static List<string> MeteorologicalFallWinterQuery(bool fall)
        {
            int daysToCheck = 5;
            int i = 0;
            int j = 4;

            using (var context = new WeatherDbContext())
            {
                List<WeatherData> MeteorFallList = new List<WeatherData>();
                List<string> MeteorFallList2 = new List<string>();

                int start, temp;
                start = (fall) ? 08 : 01;
                temp = (fall) ? 10 : 00;
                string output = (fall) ? "Fall" : "Winter";

                var resultSet = context.WeatherData
                    .Where(x => (x.DateAndTime.Month >= start && x.DateAndTime.Month <= 12 && x.Location == "ute" || (x.DateAndTime.Month >= start && x.DateAndTime.Month <= 03 && x.Location == "ute")))
                    .GroupBy(x => x.DateAndTime.Date)
                    .Select(x => new { DateAndTime = x.Key, avg = x.Average(x => x.Temperature) })
                    .OrderBy(x => x.DateAndTime.Date);

                foreach (var data in resultSet)
                {
                    WeatherData tmp = new WeatherData();

                    tmp.DateAndTime = data.DateAndTime;
                    tmp.Temperature = data.avg;

                    if (data.avg <= temp)
                    {
                        MeteorFallList.Add(tmp);
                    }

                    if (MeteorFallList.Count < 5)
                        continue;

                    try
                    {
                        TimeSpan ts = MeteorFallList[j].DateAndTime - MeteorFallList[i].DateAndTime; //
                        if (ts.Days == (daysToCheck - 1))
                        {
                            for (int m = i; m < MeteorFallList.Count; m++)
                            {
                                MeteorFallList2.Add($" Date: {MeteorFallList[m].DateAndTime:yyyy/MM/dd} || Temperature: {Math.Round(MeteorFallList[m].Temperature, 1)} °C");
                            }
                            MeteorFallList2.Add("--------------");
                            MeteorFallList2.Add($"{output} starts: {MeteorFallList[i].DateAndTime:yyyy/MM/dd} || With a temperature of: {Math.Round(MeteorFallList[i].Temperature, 1)} °C");
                            break;
                        }
                    }
                    catch
                    {
                        string message = "There is no squence of 5 days with 0.0°C or lower....";
                        MeteorFallList2.Add(message);
                        break;
                    }

                    i++;
                    j++;
                }
                return MeteorFallList2;
            }
        }
        public static List<string> AvgHumQuery(bool location)
        {
            string output = location ? "ute" : "inne";

            using (var context = new WeatherDbContext())
            {
                List<string> humidityAvgList = new List<string>();

                var resultSet = context.WeatherData
                    .Where(x => x.Location == output)
                    .GroupBy(x => x.DateAndTime.Date)
                    .Select(x => new { DateAndTime = x.Key, avg = x.Average(x => x.Humidity) })
                    .OrderBy(x => x.avg);

                var firstTen = resultSet
                    .Take(10);
                var lastTen = resultSet
                    .OrderByDescending(x => x.avg)
                    .Take(10)
                    .OrderBy(x => x.avg);

                foreach (var data in firstTen)
                {
                    humidityAvgList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round(data.avg, 1)}% Luftfuktighet.");
                }

                humidityAvgList.Add("--------------");

                foreach (var data in lastTen)
                {
                    humidityAvgList.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round(data.avg, 1)}% Luftfuktighet.");
                }

                return humidityAvgList;
            }
        }
        public static List<string> HotToColdestQuery(bool location)
        {
            string output = location ? "ute" : "inne";

            using (var context = new WeatherDbContext())
            {
                List<string> temperaturDataHotToCold = new List<string>();

                var resultSet = context.WeatherData
                    .Where(x => x.Location == output)
                    .GroupBy(x => x.DateAndTime.Date)
                    .Select(x => new { DateAndTime = x.Key, avg = x.Average(x => x.Temperature) })
                    .OrderByDescending(x => x.avg);

                var firstTen = resultSet
                    .Take(10);
                var lastTen = resultSet
                    .OrderBy(x => x.avg)
                    .Take(10)
                    .OrderByDescending(x => x.avg);

                foreach (var data in firstTen)
                {
                    temperaturDataHotToCold.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round(data.avg, 1)}°C");
                }

                temperaturDataHotToCold.Add("--------------");

                foreach (var data in lastTen)
                {
                    temperaturDataHotToCold.Add($"{data.DateAndTime:yyyy/MM/dd} || {Math.Round(data.avg, 1)}°C");
                }
                return temperaturDataHotToCold;
            }
        }
        public static List<string> AverageTempUserInput(DateTime start, bool location)
        {
            string output = location ? "inne" : "ute";

            using (var context = new WeatherDbContext())
            {
                List<string> avgTemperatures = new List<string>();

                var resultSet = context.WeatherData

                    .Where(x => x.DateAndTime >= start && x.DateAndTime <= start.AddDays(1) && x.Location == output)
                    .GroupBy(x => x.DateAndTime.Date)
                    .Select(x => new { DateAndTime = x.Key, avg = x.Average(x => x.Temperature) });

                foreach (var data in resultSet)
                {
                    avgTemperatures.Add($"DATE: {data.DateAndTime:yyyy/MM/dd} || AVG TEMPERATURE: {string.Format("{0:0.#}", data.avg)}°C");
                }
                if (avgTemperatures.Count == 0)
                {
                    avgTemperatures.Add("No data for this date exist.");
                    return avgTemperatures;
                }
                else
                {
                    return avgTemperatures;
                }
            }
        }
    }
}
