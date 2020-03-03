using FlightManager.Services.Contracts;
using FlightManager.Services.ServiceModels;
using FlightManager.ViewModels.Flight;
using Microsoft.AspNetCore.Mvc;

namespace FlightManager.Controllers
{
    public class FlightController : Controller
    {
        private IFlightService flightService;
        private IReservationService reservationService;

        public FlightController(IFlightService flightService, IReservationService reservationService)
        {
            this.flightService = flightService;
            this.reservationService = reservationService;
        }

        public IActionResult Create()
        {
            var viewModel = new CreateFlightViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateFlightViewModel viewModel)
        {
             if (!ModelState.IsValid)
            {
                return Redirect("/Flight/Create");
            }
            if(viewModel.Arrival < viewModel.Departure)
            {
                return Redirect("/Flight/Create");
            }

            var flight = new FlightServiceModel
            {
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

            flightService.Create(flight);

            return Redirect("/Home/Index");
        }
    }
}