using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Flight
{
    public class FlightViewModel
    {
        public string Id { get; set; }
        
        public string Origin { get; set; }
        
        public string Destination { get; set; }
        
        public DateTime Departure { get; set; }
        
        public TimeSpan FlightDuration { get; set; }
        
        public int PassengerSeatsLeft { get; set; }

        public int BusinessClassSeatsLeft { get; set; }

    }
}
