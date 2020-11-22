using FlightManager.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Flight
{
    public class FlightViewModel
    {
        public string Id { get; set; }
        
        public City Origin { get; set; }
        
        public City Destination { get; set; }
        
        public DateTime Departure { get; set; }
        
        public TimeSpan FlightDuration { get; set; }
        
        public int PassengerSeatsLeft { get; set; }

        public int BusinessClassSeatsLeft { get; set; }

    }

}
