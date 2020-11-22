using FlightManager.DataModels;
using FlightManager.ViewModels.Reservation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Flight
{
    public class IndexDetailsViewModel
    {
        public string Id { get; set; }
        public City Origin { get; set; }
        public City Destination { get; set; }
        public DateTime Departure { get; set; }
        public DateTime Arrival { get; set; }
        public string PlaneType { get; set; }
        public string PlaneNumber { get; set; }
        public string PilotName { get; set; }
        public int PassengerSeatsLeft { get; set; }
        public int BusinessClassSeatsLeft { get; set; }

        public List<ReservationViewModel> Reservations { get; set; } = new List<ReservationViewModel>();

    }
}
