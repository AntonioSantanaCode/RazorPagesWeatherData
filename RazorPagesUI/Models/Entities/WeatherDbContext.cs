﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorPagesUI.Models.Entities
{
    class WeatherDbContext : DbContext
    {
        private string connectionString;
        public WeatherDbContext() : base()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("sqlConnection");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            optionBuilder.UseSqlServer(connectionString);
        }
        public DbSet<WeatherData> WeatherData { get; set; }
    }
}
