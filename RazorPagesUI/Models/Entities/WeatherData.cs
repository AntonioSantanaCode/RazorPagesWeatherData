using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesUI.Models.Entities
{
    public partial class WeatherData
    {
        public int ID { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public float Temperature { get; set; }
        public float Humidity { get; set; }
    }
}
