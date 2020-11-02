    using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.ServiceModels
{
    public class FlightServiceModel
    {
        public string Id {get; set;}
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneType { get; set; }
        public string PlaneNumber { get; set; }
        public string PilotName { get; set; }
        public int PassengerSeatsLeft { get; set; }
        public int BusinessClassSeatsLeft { get; set; }

    }
}
