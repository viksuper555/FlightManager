using FlightManager.DataModels;
using Microsoft.AspNetCore.Html;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace FlightManager.ViewModels.Flight
{
    public class IndexFlightsViewModel
    {
        public List<FlightViewModel> Flights { get; set; }

        public int FlightsCount { get; set; }

        public int CurrentPage { get; set; }

        public int EndPage { get; set; }

        public Dictionary<City, List<City>> Connections { get; set; }
    }
}
