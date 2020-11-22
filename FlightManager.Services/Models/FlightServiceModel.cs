using FlightManager.DataModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlightManager.Services.ServiceModels
{
    public class FlightServiceModel
    {
        public string Id {get; set;}
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneType { get; set; }
        public string PlaneNumber { get; set; }
        public string PilotName { get; set; }
        public int PassengerSeatsLeft { get; set; }
        public int BusinessClassSeatsLeft { get; set; }
        public City Origin { get; set; }
        public string OriginId { get; set; }
        public City Destination { get; set; }
        public string DestinationId { get; set; }

    }
}
