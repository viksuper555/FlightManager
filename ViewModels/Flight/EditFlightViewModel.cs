using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Flight
{
    public class EditFlightViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string Origin { get; set; }

        [Required]
        [MaxLength(40)]
        public string Destination { get; set; }

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
        [Range(0, 10000)]
        public int PassengerSeatsLeft { get; set; }

        [Required]
        [Range(0, 10000)]
        public int BusinessClassSeatsLeft { get; set; }
    }
}
