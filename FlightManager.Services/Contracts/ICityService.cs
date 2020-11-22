using FlightManager.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.Contracts
{
    public interface ICityService
    {
        public void Create(City city);
        public bool CityExists(string id);

        public List<string> GetCountriesByCity(string city);
        public (double, double) GetCoordinates(string cityName);
        public (double, double) GetCoordinates(string cityName, string countryName);

        public City GetCityByName(string name);
        public City GetCityByName(string name, string country);

        public City GetCityById(string id);

        public bool IsCityASingleton(string city);
        bool ValidCityName(string name);
    }
}

