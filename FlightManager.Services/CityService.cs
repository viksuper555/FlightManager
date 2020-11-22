using FlightManager.Data;
using FlightManager.DataModels;
using FlightManager.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FlightManager.Services
{
    public class CityService : ICityService
    {
        private readonly FlightManagerDbContext context;
        private readonly IWebHostEnvironment _environment;
        public CityService(FlightManagerDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            _environment = environment;
        }

        public bool ValidCityName(string name)
        {
            var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(_environment.WebRootPath + "\\lib\\Json\\cities.json"));
            return cities.Any(x => x.Name == name);
        }
        public bool CityExists(string id)
        {
            return context.Cities.Any(x => x.Id == id);
        }
        public List<string> GetCountriesByCity(string city)
        {
            var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(_environment.WebRootPath + "\\lib\\Json\\cities.json"))
                .Where(x => x.Name.Equals(city)).ToList();

            return cities.Select(x => x.Country).ToList();
        }
        public (double, double) GetCoordinates(string cityName)
        {
            var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(_environment.WebRootPath + "\\lib\\Json\\cities.json"));
            var city = cities.FirstOrDefault(x => x.Name == cityName);
            if (city == null)
                return (0, 0);
            return (city.Latitude, city.Longitude);
        }
        public (double, double) GetCoordinates(string cityName, string countryName)
        {
            var cities = JsonConvert.DeserializeObject<List<City>>(File.ReadAllText(_environment.WebRootPath + "\\lib\\Json\\cities.json"));
            var city = cities.FirstOrDefault(x => x.Name == cityName && x.Country == countryName);
            return (city.Latitude, city.Longitude);
        }

        public City GetCityByName(string name)
        {
            try
            {
                return context.Cities.SingleOrDefault(x => x.Name == name);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public City GetCityByName(string name, string country)
        {
            try
            {
                var cities = context.Cities.Where(x => x.Name == name);
                return cities.FirstOrDefault(x => x.Country == country);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public City GetCityById(string id)
        {
            if (CityExists(id))
            {
                return context.Cities.SingleOrDefault(x => x.Id == id);
            }
            throw new ArgumentException("Invalid id!");
        }

        public bool IsCityASingleton(string city)
        {
            return GetCountriesByCity(city).Count() == 1;
        }

        public void Create(City city)
        {
            context.Cities.Add(city);
            context.SaveChanges();
        }
    }
}
