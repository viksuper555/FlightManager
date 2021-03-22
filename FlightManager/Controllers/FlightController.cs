using FlightManager.DataModels;
using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using FlightManager.ViewModels.Flight;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FlightManager.Controllers
{
    public class FlightController : Controller
    {
        private readonly IFlightService flightService;
        private readonly IReservationService reservationService;
        private readonly ICityService cityService;

        public FlightController(IFlightService flightService, IReservationService reservationService, ICityService cityService)
        {
            this.cityService = cityService;
            this.flightService = flightService;
            this.reservationService = reservationService;
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
                return Redirect("/Home/Index");
            var viewModel = new CreateFlightViewModel();
            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateFlightViewModel viewModel)
        {
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager"))
                return Redirect("/Home/Index");
            if (!ModelState.IsValid)
            {
                return Redirect("/Flight/Create");
            }
            if(viewModel.Arrival < viewModel.Departure)
            {
                return Redirect("/Flight/Create");
            }

            if (!cityService.ValidCityName(viewModel.Origin))
            {
                viewModel.Origin = "";
                return Redirect("/Flight/Create");
            }
            if (!cityService.ValidCityName(viewModel.Destination))
            {
                viewModel.Destination = "";
                return Redirect("/Flight/Create");
            }

            bool valid = true;
            if (!cityService.IsCityASingleton(viewModel.Origin) && viewModel.OriginCountry == null)
            {
                var countries = cityService.GetCountriesByCity(viewModel.Origin);
                viewModel.OriginCountries = countries;
                valid = false;
            }

            if (!cityService.IsCityASingleton(viewModel.Destination) && viewModel.DestinationCountry == null)
            {
                var countries = cityService.GetCountriesByCity(viewModel.Destination);
                viewModel.DestinationCountries = countries;
                valid = false;
            }
            if(!valid)
                return View(viewModel);

            City origin;

            if (viewModel.OriginCountry != null)
                origin = cityService.GetCityByName(viewModel.Origin, viewModel.OriginCountry);
            else
                origin = cityService.GetCityByName(viewModel.Origin);
            if (origin == null)
            {
                origin = new City { Name = viewModel.Origin, Country = viewModel.OriginCountry };
                cityService.Create(origin);
            }



            City destination;
            if (viewModel.DestinationCountry != null)
                destination = cityService.GetCityByName(viewModel.Destination, viewModel.DestinationCountry);
            else
                destination = cityService.GetCityByName(viewModel.Destination);
            if (destination == null)
            {
                destination = new City { Name = viewModel.Destination, Country = viewModel.DestinationCountry };
                cityService.Create(destination);
            }



            var flight = new FlightServiceModel
            {   
                Origin = origin,
                Destination = destination,
                Departure = viewModel.Departure,
                Arrival = viewModel.Arrival,
                PlaneType = viewModel.PlaneType,
                PlaneNumber = viewModel.PlaneNumber,
                PilotName = viewModel.PilotName,
                PassengerSeatsLeft = viewModel.PassengerSeatsLeft,
                BusinessClassSeatsLeft = viewModel.BusinessClassSeatsLeft
            };

            FlightSetCity(flight.Origin);
            FlightSetCity(flight.Destination);
            flightService.Create(flight);

            return Redirect("/Home/Index");
        }
        
        public IActionResult IndexFlights(int page)
        {
            int flightsCount = flightService.GetCount();
            int flightsPerPage = 8;

            if (page < 1)
                return Redirect("/Home/Index");

            var endPage = (flightsCount / flightsPerPage) + 1;

            if (endPage > 1)
                endPage--;

            if (page > endPage)
                return Redirect("/Home/Index");

            var viewModel = new IndexFlightsViewModel
            {
                FlightsCount = flightsCount,
                Flights = new List<FlightViewModel>(),
                CurrentPage = page,
                EndPage = endPage,
                Connections = new Dictionary<City, List<City>>()

            };

            var flights = flightService.GetAllByPage(page);     

            foreach (var flight in flights)
            {
                //TimeSpan GMTdiff = TimeSpan.FromHours(flightService.GetGMTDifference(flight));
                //TimeSpan flightDuration = flight.Arrival.Subtract(flight.Departure + GMTdiff);
                //TODO: Fix time diff
                if(viewModel.Connections.ContainsKey(flight.Origin))
                {
                    viewModel.Connections[flight.Origin].Add(flight.Destination);
                }
                else
                {
                    viewModel.Connections.Add(flight.Origin, new List<City> { flight.Destination });
                }

                viewModel.Flights.Add(new FlightViewModel
                {
                    Id = flight.Id,
                    Origin = flight.Origin,
                    Destination = flight.Destination,
                    Departure = flight.Departure,
                    FlightDuration = flight.Arrival-flight.Departure,
                    PassengerSeatsLeft = flight.PassengerSeatsLeft,
                    BusinessClassSeatsLeft = flight.BusinessClassSeatsLeft,
                } );
            }

            return View(viewModel);

        }
        public IActionResult IndexDetails(string id)
        {
            if(!flightService.FlightExists(id))
            {
                return Redirect("IndexFlights?page=1");
            }

            var flight = flightService.GetFlightById(id);
            var reservations = reservationService.GetAllReservationsByFlightId(id);

            var viewModel = new IndexDetailsViewModel()
            {
                Id = flight.Id,
                Origin = flight.Origin,
                Destination = flight.Destination,
                Departure = flight.Departure,
                Arrival = flight.Arrival,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                PassengerSeatsLeft = flight.PassengerSeatsLeft,
                BusinessClassSeatsLeft = flight.BusinessClassSeatsLeft
            };

            foreach(var res in reservations)
            {
                viewModel.Reservations.Add(new ViewModels.Reservation.ReservationViewModel
                {
                    Id = res.Id,
                    FirstName = res.FirstName,
                    SecondName = res.SecondName,
                    LastName = res.LastName,
                    EGN = res.EGN,
                    PhoneNumber = res.PhoneNumber,
                    Nationality = res.Nationality,
                    TicketType = res.TicketType,
                    TicketsCount = res.TicketsCount,
                    Email = res.Email,
                    IsConfirmed = res.IsConfirmed
                });
            }
            return View(viewModel);
        }
    
        //[Authorize(Roles = "Admin")]
        public IActionResult Edit(string id)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");
            if (!flightService.FlightExists(id))
            {
                return Redirect("IndexFlights?page=1");
            }

            var flight = flightService.GetFlightById(id);

            var viewModel = new EditFlightViewModel()
            {
                Id = flight.Id,
                Origin = flight.Origin,
                Destination = flight.Destination,
                Departure = flight.Departure,
                Arrival = flight.Arrival,
                PlaneType = flight.PlaneType,
                PlaneNumber = flight.PlaneNumber,
                PilotName = flight.PilotName,
                PassengerSeatsLeft = flight.PassengerSeatsLeft,
                BusinessClassSeatsLeft = flight.BusinessClassSeatsLeft
            };

            return View(viewModel);
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditFlightViewModel viewModel)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");
            if (flightService.FlightExists(viewModel.Id) && ModelState.IsValid)
            {
                var flight = new FlightServiceModel
                { 
                    Id = viewModel.Id,
                    Origin = viewModel.Origin,
                    Destination = viewModel.Destination,
                    Departure = viewModel.Departure,
                    Arrival = viewModel.Arrival,
                    PlaneType = viewModel.PlaneType,
                    PlaneNumber = viewModel.PlaneNumber,
                    PilotName = viewModel.PilotName,
                    PassengerSeatsLeft = viewModel.PassengerSeatsLeft,
                    BusinessClassSeatsLeft = viewModel.BusinessClassSeatsLeft
                };
                flightService.Edit(flight);
            }
            return Redirect("IndexFlights?page=1");
        }

        //[Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            if (!User.IsInRole("Admin"))
                return Redirect("/Home/Index");
            if (flightService.FlightExists(id))
            {
                flightService.DeleteById(id);
            }
            return Redirect("IndexFlights?page=1");
        }

        private void FlightSetCity(City city)
        {
            if (city.Country != null)
                (city.Latitude, city.Longitude) = cityService.GetCoordinates(city.Name, city.Country);
            else
                (city.Latitude, city.Longitude) = cityService.GetCoordinates(city.Name);

            var originCountries = cityService.GetCountriesByCity(city.Name);

            if (originCountries.Count == 1)
            {
                city.Country = originCountries.First();
            }
        }

        //TODO:Validate city's existence

        //[AcceptVerbs("POST")]
        //public IActionResult VerifyCity(string city, string country)
        //{
        //    if(country != null)
        //    {
        //        if (cityService.GetCityByName(city, country) == null)
        //           return Json($"A city named {city} in {country} doesn't exist.");
        //    }
        //    else if (cityService.GetCityByName(city) == null)
        //        return Json($"A city named {city} doesn't exist.");

        //    return Json(true);
        //}


    }
}