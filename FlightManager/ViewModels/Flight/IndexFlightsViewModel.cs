using FlightManager.DataModels;
using System;
using System.Collections.Generic;
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
    }
}
