using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManager.ViewModels.Flight
{
    public class CreateFlightViewModel
    {
        public CreateFlightViewModel()
        {
            Origin = "Paris";
            Destination = "London";
        }

        [Required]
        [MaxLength(40)]

        public string Origin { get; set; }

        [Required]
        [MaxLength(40)]
        [Remote(action: "VerifyCity", controller: "Flight", AdditionalFields = nameof(DestinationCountry))]

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

        public string OriginCountry { get; set; }
        public string DestinationCountry { get; set; }

        public List<string> OriginCountries { get; set; }
        public List<string> DestinationCountries { get; set; }

        //TODO:Validate city's existence
        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Genre == Genre.Classic && ReleaseDate.Year > _classicYear)
        //    {
        //        yield return new ValidationResult(
        //            $"Classic movies must have a release year no later than {_classicYear}.",
        //            new[] { nameof(ReleaseDate) });
        //    }
        //}
    }
}
