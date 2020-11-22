using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FlightManager.DataModels
{
    public class Flight
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public DateTime Departure { get; set; }

        [Required]
        public DateTime Arrival { get; set; }

        [Required]
        public string PlaneType { get; set; }

        [Required]
        public string PlaneNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string PilotName { get; set; }

        [Required]
        [Range(0,10000)]
        public int PassengerSeatsLeft { get; set; }

        [Required]
        [Range(0, 10000)]
        public int BusinessClassSeatsLeft { get; set; }

        public City Origin { get; set; }

        [Required]
        public string OriginId { get; set; }

        public City Destination { get; set; }

        [Required]
        public string DestinationId { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new HashSet<Reservation>();

    }
}
